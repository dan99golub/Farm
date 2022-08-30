using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SwitchEvent
{
    public abstract class EventSwitch<T> : MonoBehaviour
    {
        public UltEvent<T> Default;
        public List<Data> Datas;
        private Dictionary<T, Data> _val;
        public Dictionary<T, Data> Val => _val??= CreateDict();

        private Dictionary<T, Data> CreateDict()
        {
            Dictionary<T, Data> result = new Dictionary<T, Data>();
            Datas.ForEach(x=>result.Add(x.Target, x));
            return result;
        }

        public void Invoke(T v)
        {
            Default.Invoke(v);
            if(Val.TryGetValue(v, out var r)) r.Event.Invoke();
        }

        [System.Serializable]
        public class Data
        {
            public T Target;
            public UltEvent Event;
        }
    }
}