using System;
using System.Collections.Generic;

namespace DTO
{
    public class SignalBus
    {
        private Dictionary<Type, object> _publisher = new Dictionary<Type, object>();
        
        public void Fire<T>(T obj) => Get<T>().Fire(obj);

        public void Register<T>(Action<T> callback) => Get<T>().Sub(callback);

        public void UnRegister<T>(Action<T> callback) => Get<T>().Unsub(callback);

        private Publisher<T> Get<T>()
        {
            _publisher.TryGetValue(typeof(T), out var r);
            if (r != null) return r as Publisher<T>;
            else
            {
                var newR = new Publisher<T>();
                _publisher.Add(typeof(T), newR);
                return newR;
            }
        }
        
        private class Publisher<T>
        {
            public event Action<T> Event;

            public void Fire(T obj) => Event?.Invoke(obj);

            public void Sub(Action<T> act) => Event += act;
            
            public void Unsub(Action<T> act) => Event -= act;
        }
    }
}