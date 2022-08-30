using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game.UFOs
{
    public class UFOMark : MonoBehaviour
    {
        private static List<UFOMark> _marks = new List<UFOMark>();
        public static ReadOnlyCollection<UFOMark> Marks => _marks.AsReadOnly();
        
        public bool CanMove { get => _canMove; set => _canMove = value; }
        [SerializeField]private bool _canMove;
        
        
        public AnimalMark Mark;
        public Transform MoveTransform => Mark.transform;
        
        public UltEvent Picked;
        public UltEvent Droped;

        private void Awake() => _marks.Add(this);

        private void OnDestroy() => _marks.Remove(this);

        public void Pick() => Picked.Invoke();

        public void Drop() => Droped.Invoke();
    }
}