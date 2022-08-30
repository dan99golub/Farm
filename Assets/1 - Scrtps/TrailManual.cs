using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TrailManual : MonoBehaviour
    {
        public Transform PointSpawn;
        public Transform TargetRotation;
        public GameObject Prefab;

        private List<GameObject> _elements = new List<GameObject>();

        public void ClearAll()
        {
            _elements.ForEach(x=>Destroy(x.gameObject));
            _elements.Clear();
        }

        public void SpawnElement()
        {
            var newObj = Instantiate(Prefab, transform.position, TargetRotation.rotation);
            _elements.Add(newObj);
        }
    }
}