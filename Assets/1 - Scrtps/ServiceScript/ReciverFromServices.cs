using UnityEngine;

namespace ServiceScript
{
    public class ReciverFromServices : MonoBehaviour
    {
        public T Request<T>() => Services<T>.S.Get();
        
        public T Request<T>(string id) => ServicesID<T>.S.Get(id);
        
        public T RequestDefaultId<T>() => ServicesID<T>.S.Get();
    }
}