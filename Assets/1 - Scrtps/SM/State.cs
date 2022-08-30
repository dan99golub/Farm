using System;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    [System.Serializable]
    public class State : MonoBehaviour
    {
        public UltEvent Entered;
        public UltEvent Exited;

        public void Enter()
        {
            Entered.Invoke();
        }

        public void Exit()
        {
            Exited.Invoke();
        }
    }
}