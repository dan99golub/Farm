using System;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SwitchEvent
{
    public class EventByEnable: MonoBehaviour
    {
        public bool TargetMyEnable;
        public UltEvent Event;

        private void OnEnable()
        {
            
        }

        public void TryInvokeEvent()
        {
            if(TargetMyEnable==enabled) Event.Invoke();
        }
    }
}