using System;
using System.Runtime.ConstrainedExecution;
using DG.Tweening;
using MaoUtility.Converse.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MaoUtility.Converse.Core.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class MoveUIItem : BaseConverseComponent
    {
        public override string PrefixAlias => "MoveItem";

        public float Duration;
        
        public UnityEvent StartMove;
        public UnityEvent StopMove;
        
        private Camera _camera;
        private Camera Camera => _camera ??= Camera.main;

        [ShowInInspector]
        public Vector2 CurrentViewPos
        {
            get
            {
                if (Rect) return Camera.ScreenToViewportPoint(Rect.localPosition);
                else return Vector2.zero;
            }
        }
        
        private RectTransform _rect;
        private RectTransform Rect => _rect ??= GetComponent<Transform>() as RectTransform;

        private bool CanShow => Application.isPlaying;

        private Tween _tween;
        [SerializeField] private Ease _ease;

        [Button]
        public void MoveToView(Vector2 to)
        {
            Move(Camera.ViewportToScreenPoint(new Vector3(to.x, to.y)));
        }

        [Button]
        public void MoveToMark(UIMark mark) => MoveToGameObject(mark.gameObject);

        [Button]
        public void MoveToGameObject(GameObject obj)
        {
            if (obj.transform is RectTransform)
            {
                Move(obj.transform.position);
            }
            else
            {
                Move(Camera.WorldToScreenPoint(obj.transform.position));
            }
        }

        private void Move(Vector3 targetPoint)
        {
            Stop(true);
            targetPoint /= ParentCanvas.scaleFactor;
            if (Duration > 0)
            {
                _tween = DOTween.To(() => Rect.anchoredPosition,
                    x =>
                    {
                        Rect.anchoredPosition = x;
                    }, (Vector2)targetPoint, Duration).SetEase(_ease).OnComplete(()=>
                {
                    StopMove.Invoke();
                    Rect.anchoredPosition = targetPoint;
                });
                StartMove.Invoke();
            }
            else
            {
                StartMove?.Invoke();
                Rect.anchoredPosition = targetPoint;
                StopMove?.Invoke();
            }
        }

        private void OnDestroy() => Stop(false);

        public void Stop(bool isEvent) => _tween?.Kill(isEvent);
    }
}