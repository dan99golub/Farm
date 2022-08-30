using System;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public abstract class ValueContainer<T> : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public T Min => _getMin==null ? _min : _getMin();
        [SerializeField] private T _min;
        
        [ShowInInspector, ReadOnly] public T Max => _getMax == null ? _max : _getMax();
        [SerializeField] private T _max;

        private Func<(T min, T max, T oldV, T newV), T> _valid;
        private Func<T> _getMin;
        private Func<T> _getMax;

        private ValueContainerType _typeValue;
        public ValueContainerType TypeContainer => _typeValue;
        
        public T Current
        {
            get=>_current;
            set
            {
                var old = _current;
                if (_valid == null) _current = DefaultValid(value, Min, Max);
                else _current = _valid.Invoke((Min, Max, old, value));
                InvokeEvent();
            }
        }
        [SerializeField] private T _current;
        
        public event Action Changed;
        public UltEvent<T> NewCurrent;

        public void SetMin(Func<T> f)
        {
            _getMin = f;
        }

        public void SetMax(Func<T> f)
        {
            _getMax = f;
        }

        public void SetValid(Func<(T min, T max, T oldV, T newV), T> f) => _valid = f;

        [Button]
        private void InvokeEvent()
        {
            Changed?.Invoke();
            NewCurrent.Invoke(_current);
        }

        [Button]
        public void Valid() => Current = Current;

        protected abstract T DefaultValid(T current, T min, T max);
    }
}