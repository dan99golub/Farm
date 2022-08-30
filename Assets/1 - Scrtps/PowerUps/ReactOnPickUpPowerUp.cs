using System;
using System.Linq;
using System.Runtime.InteropServices;
using UltEvents;
using UnityEngine;

namespace PowerUps
{
    public class ReactOnPickUpPowerUp : MonoBehaviour
    {
        public PowerUp[] Types;

        [SerializeField] protected PowerUpContainer _container;
        
        public UltEvent OnPick;
        public UltEvent OnRemoved;

        protected virtual void Awake() { }

        private void OnEnable() => Init(_container);

        private void OnDisable()
        {
            if (_container==null) return;  
            _container.Added -= OnAdd;
            _container.Removed -= OnRemove;
        }

        public void Init(PowerUpContainer container)
        {
            if (container==null) return;  
            _container = container;
            _container.Added += OnAdd;
            _container.Removed += OnRemove;
        }

        private void OnRemove(PowerUp.Data obj)
        {
            if(Types.Contains(obj.TypePowerUp)) OnRemoved.Invoke();
        }

        private void OnAdd(PowerUp.Data obj)
        {
            if(Types.Contains(obj.TypePowerUp)) OnPick.Invoke();
        }
    }
}