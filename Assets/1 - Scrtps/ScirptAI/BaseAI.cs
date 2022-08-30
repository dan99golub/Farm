using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.ScirptAI
{
    public abstract class BaseAI : MonoBehaviour
    {
        public UltEvent<string> Entered;
        public UltEvent<string> Exited;
        
        [Header("States AI")]
        [ShowInInspector, SerializeField, ReadOnly] protected string CurrentState;

        public void ChangeState(string newState)
        {
            OnExit(CurrentState);
            CurrentState = newState;
            OnEnter(newState);
        }
        
        protected virtual void OnEnter(string state)
        {
            Entered.Invoke(state);
        }

        protected virtual void OnExit(string state)
        {
            Exited.Invoke(state);
        }
    }
}