using Sirenix.Utilities;
using UnityEngine;

namespace LoadScenes
{
    public class OnLoad : MonoBehaviour
    {
        public RegisterServicesScene[] Registers;

        private void Awake() => Registers.ForEach(x => x.Register());

        private void OnDestroy() => Registers.ForEach(x => x.Unregister());
    }
}