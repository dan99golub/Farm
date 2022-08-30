using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game.Plants
{
    public class CacheField : MonoBehaviour
    {
        public Field Field;
        public CageManager CageManager;
        public PlantDistribute PlantDistribute;
        public AnimalManager AnimalManager;

        [Button]private void FindAll()
        {
            Field = GetComponent<Field>();
            CageManager = GetComponent<CageManager>();
            PlantDistribute = GetComponent<PlantDistribute>();
            AnimalManager = GetComponent<AnimalManager>();
        }
    }
}