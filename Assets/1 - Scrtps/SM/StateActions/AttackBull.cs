using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class AttackBull : StateAction
    {
        public float SpeedAttack;
        public StateAI Ai;
        public S_Agresive Agresive;
        public UltEvent EndMove;
        
        private Coroutine _corutineAttack;
        private Coroutine _actionChangeTile;

        private ZoneManager Zones => Services<ZoneManager>.S.Get();

        protected override void Register(State s)
        {
            s.Entered.DynamicCalls += () =>
            {
                StartCoroutine(Agresive.SetVelocity(transform.forward*SpeedAttack+Vector3.up*0.1f, () => Ai.CurrentState != s));
                _corutineAttack = StartCoroutine(Agresive.StopBackMove(0.35f, 0.1f, EndMove.Invoke));
                _actionChangeTile = StartCoroutine(Agresive.TryReplaceTile(0.15f));
            };
            s.Exited.DynamicCalls += () =>
            {
                StopCoroutine(_corutineAttack);
                StopCoroutine(_actionChangeTile);
                Zones.ReInit();
            };
        }
    }
}