using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using UltEvents;
using UnityEngine;

namespace Menu
{
    public class PointLevel : MonoBehaviour
    {
        public float DurationMove = 3;
        public StopPointPointLevel CurrentView { get; private set; }

        public UltEvent StartMove;
        public UltEvent EndMove;

        private Tween _tweeen;

        private DeskSwitcher Desks => Services<DeskSwitcher>.S.Get();

        public void InstanceMove(StopPointPointLevel newTarget)
        {
            _tweeen?.Kill();
            var p = newTarget.ParentDesk.Path.GetPoint(newTarget.ParentDesk.Datas.FirstOrDefault(x => x.View == newTarget).PointGUID);
            CurrentView = newTarget;
            transform.position = p.Position;
            transform.LookAt(p.Neighbours[0].Position, Vector3.up);
        }
        
        [Button] public bool MoveTo(StopPointPointLevel newTarget, Action OnComlete = null)
        {
            if (newTarget == null || newTarget == CurrentView) return false;
            _tweeen?.Kill();
            
            
            var path = newTarget.ParentDesk.Path.MakePath(CurrentView.ParentDesk.GetNode(CurrentView), newTarget.ParentDesk.GetNode(newTarget));
            var startPoint = path.startPoint;
            if (path.canBeBuilded == false) return false;

            
            var pointTemp = startPoint;
            float fullDistance=Vector3.Distance(pointTemp.target.Position, transform.position);
            while (pointTemp!=null)
            {
                if(pointTemp.Next!=null)
                    fullDistance += Vector3.Distance(pointTemp.target.Position, pointTemp.Next.target.Position);
                pointTemp = pointTemp.Next;
            }

            StartMove.Invoke();
            Move(fullDistance, startPoint, DurationMove, () =>
            {
                OnComlete?.Invoke();
                CurrentView = newTarget;
                EndMove.Invoke();
            });
            
            return true;
        }

        private void Move(float fullDistance, Path.PathPoint pointTemp, float duration, Action onComlete)
        {
            
            var partDuration = Vector3.Distance(transform.position, pointTemp.target.Position) / fullDistance * duration;
            transform.LookAt(pointTemp.target.Position, Vector3.up);
            _tweeen = transform.DOMove(pointTemp.target.Position, partDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if(pointTemp.Next==null) onComlete?.Invoke();
                else Move(fullDistance, pointTemp.Next, duration, onComlete);
            });
        }

        public void MoveToShop(Action action)
        {
            var target = CurrentView.ParentDesk.Datas.FirstOrDefault(x => x.View is ShopView).View;
            MoveTo(target, action);
        }
    }
}