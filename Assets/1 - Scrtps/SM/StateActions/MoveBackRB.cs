using DefaultNamespace.Game;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class MoveBackRB : StateAction
    {
        public UltEvent RichPointBack;
        public float SpeedWalkBack;
        public S_Agresive Agresive;
        public StateAI Ai;
        
        private Tile _tileForMoveBack;
        private Coroutine _corutineMoveBack;
        
        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                _tileForMoveBack = Agresive.GetTileBehindYou();
                //Debug.Log($"Angry move from {transform.position} to {_tileForMoveBack.transform.position}");
                //Agent.SetDestination(_tileForMoveBack.transform.position);
                //RichPointMoveBack.Point = _tileForMoveBack.transform.position;
                //IsRichMoveBack(3, ()=>_currentState==MoveBack);
                
                Agresive.GetVectorMoveBack();
                //StartCoroutine(SetVelocity(GetNext(Vector3.zero, GetDir())*-1*SpeedWalkBack, () => _currentState != MoveBack));
                StartCoroutine(Agresive.SetVelocity(transform.forward*-1*SpeedWalkBack+Vector3.up*0.1f, () => Ai.CurrentState != s));
                _corutineMoveBack = StartCoroutine(Agresive.StopBackMove(0.25f, 0.1f, RichPointBack.Invoke));
            };
            s.Exited.DynamicCalls += () =>
            {
                StopCoroutine(_corutineMoveBack);
            };
        }
    }
}