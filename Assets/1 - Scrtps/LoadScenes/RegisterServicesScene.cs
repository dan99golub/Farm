using UnityEngine;

namespace LoadScenes
{
    public abstract class RegisterServicesScene : MonoBehaviour
    {
        public abstract void Register();
        
        public abstract void Unregister();
    }
}