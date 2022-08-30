using System;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.ScirptAI
{
    public class ChikenAI : BaseAI
    {
        public RandomPointOnField randomPointOnField;
        public NavMeshAgent Agent;
        [Min(0)] public float WaitTime;
        [Min(0)] public float DistanceForFinishMove;

        [Header("In Game prop")]
        [ShowInInspector, ReadOnly]private (Vector3 movePos, Vector2Int posTile) _moveTo;
        [ShowInInspector] private const string SearchPoint = "Search";
        [ShowInInspector] private const string StartMove = "StartMove";
        [ShowInInspector] private const string Move = "Move";
        [ShowInInspector] private const string Wait = "Wait";
        [ShowInInspector] private const string Sleep = "Sleep";

        private void Start() => CorutineGame.Instance.WaitFrame(2, () => ChangeState(SearchPoint)); 
        
        protected override void OnEnter(string state)
        {
            base.OnEnter(state);
            switch (state)
            {
                case SearchPoint:
                    _moveTo = randomPointOnField.GetPoint();
                    if (_moveTo.posTile == randomPointOnField.CurrentPosition)
                    {
                        ChangeState(Sleep);
                        break;
                    }
                    ChangeState(StartMove);
                    break;
                case StartMove:
                    ChangeState(Move);
                    break;
                case Move:
                    Agent.SetDestination(_moveTo.movePos);
                    CheckEndPath();
                    break;
                case Wait:
                    CorutineGame.Instance.Wait(WaitTime, () => ChangeState(SearchPoint));
                    break;
                case Sleep:
                    break;
            }
            
        }

        private void CheckEndPath()
        {
            if(Vector3.Distance(transform.position, _moveTo.movePos)<DistanceForFinishMove) ChangeState(Wait);
            else CorutineGame.Instance.WaitFrame(5, CheckEndPath);
        }

        protected override void OnExit(string state)
        {
            if(state==null) return;
            base.OnExit(state);
            switch (state)
            {
                
            }
        }
    }
}