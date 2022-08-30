using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Menu.Shop_Component
{
    public abstract class Product<T> : ScriptableObject where T : MonoBehaviour
    {
        [SerializeField] public T Content;
        [SerializeField] public Sprite Icon;
        [SerializeField] public string NameContent;
        public int Cost;
        [ReadOnly] [SerializeField] public string GUID;

        [Button]
        private void GenerateGUID() => GUID = Guid.NewGuid().ToString();
    }
}