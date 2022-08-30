using System;
using ServiceScript;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class C_PlayerHealth : MonoBehaviour
    {
        public IntValueContainer Container => _valueContainer;
        [SerializeField] private IntValueContainer _valueContainer;
        [Min(0)] [SerializeField] private int _maxHp;
        [Min(0)] [SerializeField] private float _timeUndamaged;

        private int lastCurrentValue;

        [ShowInInspector, ReadOnly] private string _state;

        private bool CanShow() => Application.isPlaying;

        private void Awake()
        {
            _valueContainer.SetMin(()=>0);
            _valueContainer.SetMax(()=>_maxHp);
            _valueContainer.Current = _maxHp;
            MakeDamaged();
        }

        private void MakeDamaged()
        {
            _valueContainer.SetValid(null);
            lastCurrentValue = _valueContainer.Current;
            _valueContainer.NewCurrent.DynamicCalls += OnDamageEvent;
            _state = "Can Damage";
        }

        private void OnDamageEvent(int newV)
        {
            if(newV>=lastCurrentValue) return;
            _valueContainer.NewCurrent.DynamicCalls -= OnDamageEvent;
            _valueContainer.SetValid(UndamageValid);
            _state = "Can t Damage";
            CorutineGame.Instance.Wait(_timeUndamaged, MakeDamaged);
        }

        private int UndamageValid((int min, int max, int oldV, int newV) arg)
        {
            return arg.oldV;
        }
    }
}