using System;
using System.Collections.Generic;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class BaseAI : MonoBehaviour
    {
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly] protected State _currentState;
        protected List<string> _corutines = new List<string>();

        public State CurrentState => _currentState;
        
        protected virtual void OnDisable()
        {
            _corutines.ForEach(x=>CorutineGame.Instance.StopWait(x));
            _corutines.Clear();
            _currentState?.Exit();
            _currentState = null;
        }
        
        [Button]public void ChangeStaete(State s)
        {
            if(!enabled) return;
            _currentState?.Exit();
            _currentState = s;
            _currentState?.Enter();
        }
    }
}