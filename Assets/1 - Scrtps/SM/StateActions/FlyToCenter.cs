using DefaultNamespace.Game;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class FlyToCenter : StateAction
    {
        [Min(0)]public float H;
        [Min(0)]public float Speed;
        public Ease MoveEase;
        public UltEvent InCenter;
        public Transform RootTransform;
        private Tween _moveCenterTween;
        
        
        private Field MainField => ServicesID<Field>.S.Get();
        
        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                var pos = MainField.GetTile(Tile.GetGuid(MainField.Size / 2)).transform.position+Vector3.up*H;
                _moveCenterTween = RootTransform.DOMove(pos, RootTransform.position.CalculateDuration(pos, Speed)).SetEase(MoveEase).OnComplete(()=>InCenter.Invoke());
            };
            s.Exited.DynamicCalls += () => _moveCenterTween?.Kill();
        }
    }
}