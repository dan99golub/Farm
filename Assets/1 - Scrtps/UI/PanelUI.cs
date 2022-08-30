using System;
using System.Collections.Generic;
using Lean.Transition;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public class PanelUI : MonoBehaviour
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private bool _updateOnStart;
        
        [SerializeField] private GroupAction _on;
        [SerializeField] private GroupAction _off;
        [SerializeField] private Dictionary<string, GroupAction> _customActions=new Dictionary<string, GroupAction>();
        [SerializeField] private List<GroupAction> _actions;

        private void Start()
        {
            _actions.ForEach(x=>_customActions.Add(x.Name, x));
            if(_updateOnStart) MakeDefaultAction();
        }

        [Button] public void SetActive(bool isActive)
        {
            _isOn = isActive;
            MakeDefaultAction();
        }

        public void TryCustomAction(string name)
        {
            if(_customActions.TryGetValue(name, out var r)) r.Activated();
        }
        
        private void MakeDefaultAction()
        {
            if(_isOn) _on.Activated();
            else _off.Activated();
        }
        
        
        [System.Serializable]
        public class GroupAction
        {
            public string Name;
            public LeanPlayer LeanAnim;
            public UltEvent Event;

            public void Activated()
            {
                Event.Invoke();
                LeanAnim.Begin();
            }
        }
    }
}