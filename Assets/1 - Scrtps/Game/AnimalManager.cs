using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class AnimalManager : MonoBehaviour
    {
        [SerializeField, ReadOnly]private List<AnimalMark> _animals = new List<AnimalMark>();
        public IReadOnlyCollection<AnimalMark> Animals => _animals.AsReadOnly();

        private void Awake() => _animals = GetComponentsInChildren<AnimalMark>().ToList();
        
        public List<AnimalMark> GetAnimalsInZone(ZoneManager.Zone zone) => _animals.Where(x => zone.HasTile(x.GetPositionOnMap())).ToList();
    }
}