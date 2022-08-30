using DefaultNamespace.Game;
using DefaultNamespace.Game.Plants;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class DropAnimal : StateAction
    {
        [Min(0)]public float H;
        [Min(0)]public float Speed;
        public Ease MoveEase;
        public UltEvent EndDrop;
        [Min(0)] public float DurationDrop;
        public S_UfoAI Ufo;
        public Transform RootTransform;

        private Field MainField => ServicesID<Field>.S.Get();
        private CacheField CacheField => Services<Game.Plants.CacheField>.S.Get();
        
        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                var pos = Ufo.TargetCage.PointAnimal.transform.position + Vector3.up * H;
                RootTransform.DOMove(pos, RootTransform.position.CalculateDuration(pos, Speed)).SetEase(MoveEase).OnComplete(() =>
                {
                    Ufo.TargetMark.MoveTransform.SetParent(Ufo.LastParentTarget);
                    Ufo.TargetMark.MoveTransform.DOMove(Ufo.TargetCage.PointAnimal.position, DurationDrop).OnComplete(() =>
                    {
                        Ufo.TargetMark.Drop();
                        //Ufo.TargetCage.TryTakeAnimal(Ufo.TargetMark.Mark);
                        Ufo.LastParentTarget = null;
                        EndDrop.Invoke();
                    });
                });
            };
        }
    }
}