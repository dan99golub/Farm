using System;
using System.Collections.Generic;
using PowerUps;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class C_SpeedPlayer : MonoBehaviour
    {
        [SerializeField]private FloatValueContainer _speedContainer;
        public FloatValueContainer SpeedContainer => _speedContainer;
        [SerializeField] private PowerUpContainer _container;
        [SerializeField] private float _baseValue;

        [SerializeField] private List<ChangeSpeed> _changeres;

        private void Awake()
        {
            SpeedContainer.SetMin(()=>0);
            SpeedContainer.SetMax(() =>
            {
                var currentValue = _baseValue;
                _changeres.ForEach(x =>
                {
                    if (_container.HasPowerUp(x.TypePower)) currentValue = x.Change(currentValue);
                });
                return currentValue;
            });
            SpeedContainer.SetValid(FuncValud);
            CorutineGame.Instance.WaitFrame(1, SpeedContainer.Valid);

            _container.Added += OnChangeContainer;
            _container.Removed += OnChangeContainer;
        }

        private float FuncValud((float min, float max, float oldV, float newV) arg) => arg.max;

        private void OnChangeContainer(PowerUp.Data obj) => SpeedContainer.Valid();

        [System.Serializable]
        public class ChangeSpeed
        {
            public PowerUp TypePower;
            public Action Act;
            public float Value;

            public float Change(float v)
            {
                switch (Act)
                {
                    case Action.Add: return v + Value;
                    case Action.Minus: return v - Value;
                    case Action.Mul: return v * Value;
                    case Action.Div: return v / Value;
                    case Action.Set: return Value;
                    default: return v;
                }
            }
            
            public enum Action
            {
                Add, Minus, Mul, Div, Set
            }
        }
    }
}