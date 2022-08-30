using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game.Plants;
using DefaultNamespace.SM;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class UfoTransiter : MonoBehaviour
    {
        public UltEvent StartMoved;
        
        private HashSet<MoveAnimalByUfo> _animalsForTransit = new HashSet<MoveAnimalByUfo>();
        private HashSet<MoveAnimalByUfo> _animalsToPick = new HashSet<MoveAnimalByUfo>();

        private CageManager CageManager => Services<CacheField>.S.Get().CageManager;

        public bool TryAddAnimalMark(MoveAnimalByUfo mark)
        {
            var targetCage = CageManager.GetCage(x => x.IsBuild && x.TargetId == mark.Mark.Id);
            if (!targetCage) return false;
            _animalsToPick.Add(mark);
            return true;
        }
        
        public MoveAnimalByUfo GetFirstForPickUp() => _animalsToPick.First();
        
        public MoveAnimalByUfo GetFirstForDrop() => _animalsForTransit.First();
        
        public bool HasTargetToDrop() => _animalsForTransit.Count() > 0;
        
        public bool HasTargetToPickUp() => _animalsToPick.Count() > 0;

        public void Drop(MoveAnimalByUfo mark) => _animalsForTransit.Remove(mark);
        
        public void PickUpByMark(MoveAnimalByUfo mark)
        {
            _animalsForTransit.Add(mark);
            _animalsToPick.Remove(mark);
        }

        public void StartMove()
        {
            StartMoved.Invoke();
        }
    }
}