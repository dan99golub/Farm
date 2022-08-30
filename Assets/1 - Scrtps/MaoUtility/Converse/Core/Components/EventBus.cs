using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace MaoUtility.Converse.Core.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class EventBus : BaseConverseComponent
    {
        public override string PrefixAlias => "Events";

        [SerializeField] private Data[] _datas;

        private Dictionary<string, Data> Events => _events!=null ? _events : _events = CreateDict();
        private Dictionary<string, Data> _events;

        private Dictionary<string, Data> CreateDict()
        {
            var events = new Dictionary<string, Data>();
            _datas.ForEach(x => events.Add(x.Name, x));
            return events;
        }

        [Button]
        public void Send(string nameEvent) => Get(nameEvent).Invoke();

        public void Sub(string nameEvent, Action callback) => Get(nameEvent).Sub(callback);

        public void Unsub(string nameEvent, Action callback) => Get(nameEvent).Unsub(callback);

        private Data Get(string name)
        {
            if (Events.ContainsKey(name)) return Events[name];
            else
            {
                var r = new Data();
                Events.Add(name, r);
                return r;
            }
        }

        [System.Serializable]
        public class Data
        {
            public string Name;
            [SerializeField] private event Action Event;
            [SerializeField] private UnityEvent EventU;
            [SerializeField] private UltEvent EventUlt;

            public void Invoke()
            {
                EventU?.Invoke();
                EventUlt?.Invoke();
                Event?.Invoke();
            }

            public void Sub(Action callback) => Event += callback;
            
            public void Unsub(Action callback) => Event -= callback;
        }
    }
}