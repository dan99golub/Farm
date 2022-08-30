using DefaultNamespace.Game;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class ReturnUfo : StateAction
    {
        [Min(0)]public float H;
        [Min(0)]public float Speed;
        public Ease MoveEase;
        public UltEvent Returned;
        private Vector3 _startPoint;
        public Transform RootTransform;
        
        private Field MainField => ServicesID<Field>.S.Get();

        protected override void Register(State s)
        {
            _startPoint = new Vector3(MainField.Size.x/2, H, MainField.Size.y/2);
            RootTransform.position = _startPoint;

            s.Entered.DynamicCalls += () =>
            {
                RootTransform.DOMove(_startPoint, RootTransform.position.CalculateDuration(_startPoint, Speed)).SetEase(MoveEase).OnComplete(Returned.Invoke);
            };
        }
    }
}