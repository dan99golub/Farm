using DefaultNamespace.Game;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class FlyRandomPoint : StateAction
    {
        [Min(0)]public float H;
        [Min(0)]public float Speed;
        public Ease MoveEase;
        public UltEvent Finished;
        private Tween _moveRandomTween;
        public Transform RootTransform;
        
        private Field MainField => ServicesID<Field>.S.Get();
        
        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                var pos = MainField.GetTile(Tile.GetGuid(new Vector2Int(Random.Range(0, MainField.Size.x), Random.Range(0, MainField.Size.y)))).transform.position+Vector3.up*H;
                _moveRandomTween = RootTransform.DOMove(pos, RootTransform.position.CalculateDuration(pos, Speed)).SetEase(MoveEase).OnComplete(()=>Finished.Invoke());
            };
            s.Exited.DynamicCalls += () => _moveRandomTween?.Kill();
        }
    }
}