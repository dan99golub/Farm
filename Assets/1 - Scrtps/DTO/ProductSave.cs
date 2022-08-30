using System.Collections.Generic;
using Menu.Shop_Component;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DTO
{
    [System.Serializable]
    public class ProductSave<T> where T : MonoBehaviour
    {
        [SerializeField] [ShowInInspector] [JsonProperty] private List<string> _guids = new List<string>();

        public void Add(Product<T> content)
        {
            if(!Has(content)) _guids.Add(content.GUID);
        }

        public bool Has(Product<T> content) => _guids.Contains(content.GUID);

        public void Remove(Product<T> content) => _guids.Remove(content.GUID);
    }
}