using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MaoUtility.Converse.Core
{
    public class ConversePanelCreator : SerializedMonoBehaviour
    {
        public Dictionary<string, ConversePanel> Prefabs;

        [ShowInInspector, ReadOnly]private Dictionary<string, ConversePanel> _currents = new Dictionary<string, ConversePanel>();

        private void Awake() => GetComponentsInChildren<ConversePanel>().ForEach(x => _currents.Add(x.Name, x));

        public ConversePanel CreateById(string id, string name)
        {
            if (Prefabs.ContainsKey(id) == false)
            {
                Debug.LogWarning("Не могу получить панель по id - "+id, this);
                return null;
            }

            return CreateByPrefab(Prefabs[id], name);
        }
        
        public ConversePanel CreateByPrefab(ConversePanel prefab, string name)
        {
            if (_currents.ContainsKey(name))
            {
                Debug.LogWarning($"Панел под именем {name} уже есть", this);
                return _currents[name];
            }
            var result = Instantiate(prefab);
            result.transform.SetParent(transform, false);
            result.Init(name);
            _currents.Add(name, result);
            return result;
        }

        public ConversePanel Get(string name)
        {
            _currents.TryGetValue(name, out var r);
            return r;
        }

        public void SetSibling(ConversePanel panel, int index) => panel?.transform.SetSiblingIndex(index);

        public void ClearByName(string name)
        {
            if(_currents.TryGetValue(name, out var r)) Destroy(r.gameObject);
            _currents.Remove(name);
        }

        public void ClearByObject(ConversePanel panel) => ClearByName(panel.Name);

        public void ClearAll() => _currents.Values.ToList().ForEach(x=>ClearByObject(x));
    }
}