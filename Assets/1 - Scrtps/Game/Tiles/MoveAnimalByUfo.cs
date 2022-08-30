using System;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class MoveAnimalByUfo : MoveMethodAnimal
    {
        public UltEvent OnPickup;
        public UltEvent OnDrop;
        
        public Transform Root;
        public AnimalMark Mark;
        
        private Action _callback;
        private UfoTransiter Ufo => Services<UfoTransiter>.S.Get();
        
        public void Pick()
        {
            OnPickup.Invoke();
        }

        public void Drop()
        {
            _callback?.Invoke();
            OnDrop.Invoke();
        }
        
        public override void Move(Cage cage, Action action)
        {
            _callback = action;
            Ufo.TryAddAnimalMark(this);
            Ufo.StartMove();
        }
    }
}