using System.Collections.Generic;

namespace ServiceScript
{
    public class ServicesID<T>
    {
        private Dictionary<string, T> _services = new Dictionary<string, T>();

        private static ServicesID<T> _i;
        public static ServicesID<T> S=>_i??=new ServicesID<T>();

        public T Get(string id = "Default")
        {
            _services.TryGetValue(id, out var r);
            return r;
        }

        public T Set(T service, string id = "Default")
        {
            if (service == null) _services.Remove(id);
            else _services.Add(id, service);
            return service;
        }
    }
}