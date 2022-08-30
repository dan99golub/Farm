using DefaultNamespace.Game;
using DefaultNamespace.Game.Plants;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class PickUpAnimal : StateAction
    {
        [Min(0)]public float H;
        public Ease MoveEase;
        [Min(0)]public float DurationFlyToPickUp;
        [Min(0)] public float DurationPick;
        public Transform PickUpPoint;
        public UltEvent EndPickUp;
        public S_UfoAI Ufo;
        public Transform RootTransform;
        
        private Field MainField => ServicesID<Field>.S.Get();
        private CacheField CacheField => Services<Game.Plants.CacheField>.S.Get();
        private Vector3 _startPoint;
        
        
        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                float progress = 0;
                var startPos = RootTransform.position;
                DOTween.To(() => progress, x => progress = x, 1, DurationFlyToPickUp).OnUpdate(() =>
                {
                    RootTransform.position = Vector3.Lerp(startPos, Ufo.TargetMark.transform.position + Vector3.up*H, progress);
                }).SetEase(MoveEase).OnComplete(() =>
                {
                    Ufo.TargetMark.Pick();
                    Ufo.TargetMark.MoveTransform.SetParent(PickUpPoint);
                    Ufo.TargetMark.MoveTransform.DOMove(PickUpPoint.transform.position, DurationPick).OnComplete(() => EndPickUp.Invoke());
                });
            };
        }
    }
}