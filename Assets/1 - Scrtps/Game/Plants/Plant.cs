using System;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace.Game.Plants
{
    public class Plant : MonoBehaviour
    {
        public CacheField CacheByField => Services<CacheField>.S.Get();

        private void Start() => Spawn();
        
        private void Spawn()
        {
            var pos = Tile.GetTilePos(transform.position);
             Instantiate(CacheByField.PlantDistribute.GetPlant(pos), transform.position, Quaternion.identity, transform);
        }
    }
}