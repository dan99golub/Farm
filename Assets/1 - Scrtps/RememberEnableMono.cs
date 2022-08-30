using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

namespace DefaultNamespace
{
    public class RememberEnableMono : MonoBehaviour
    {
        private Dictionary<MonoBehaviour, bool> _list = new Dictionary<MonoBehaviour, bool>();
        private Dictionary<GameObject, bool> _listGo = new Dictionary<GameObject, bool>();

        public void Remember(MonoBehaviour component)
        {
            if (_list.ContainsKey(component)) _list[component] = component.enabled;
            else _list.Add(component, component.enabled);
        }

        public void RememberAndSet(MonoBehaviour component, bool value)
        {
            Remember(component);
            SetActiveComponent(component, value);
        }
        
        public void Set(MonoBehaviour component)
        {
            if (_list.TryGetValue(component, out var r)) component.enabled = r;
        }

        public void SetActiveComponent(MonoBehaviour component, bool state) => component.enabled = state;
        
        public void Remember(GameObject go)
        {
            if (_listGo.ContainsKey(go)) _listGo[go] = go.activeSelf;
            else _listGo.Add(go, go.activeSelf);
        }

        public void RememberAndSet(GameObject go, bool value)
        {
            Remember(go);
            SetActiveComponent(go, value);
        }
        
        public void Set(GameObject go)
        {
            if (_listGo.TryGetValue(go, out var r)) go.SetActive(r);
        }

        public void SetActiveComponent(GameObject go, bool state) => go.SetActive(state);
    }
}