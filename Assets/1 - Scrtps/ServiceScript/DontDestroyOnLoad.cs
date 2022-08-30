using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ServiceScript
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static Dictionary<string, DontDestroyOnLoad> _objects = new Dictionary<string, DontDestroyOnLoad>();
        [SerializeField, ReadOnly] private string _guid = Guid.NewGuid().ToString();
        
        private void Awake()
        {
            if (_objects.ContainsKey(_guid))
            {
                if (_objects[_guid] != null)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            _objects.Remove(_guid);
            _objects.Add(_guid, this);
            DontDestroyOnLoad(gameObject);
        }

        [Button] private void NewGuid() => _guid = Guid.NewGuid().ToString();
    }
}