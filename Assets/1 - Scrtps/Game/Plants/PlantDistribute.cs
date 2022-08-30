using System;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.Game.Plants
{
    public abstract class PlantDistribute : MonoBehaviour
    {
        public abstract GameObject GetPlant(Vector2Int posTile);
    }
}