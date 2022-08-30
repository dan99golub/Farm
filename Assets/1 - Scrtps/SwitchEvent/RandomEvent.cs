using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SwitchEvent
{
    public class RandomEvent : MonoBehaviour
    {
        [Range(0,1f)]public float ChanceTrue;

        public UltEvent True;
        public UltEvent False;

        public void Invoke()
        {
            if(Random.Range(0,(float)1)<ChanceTrue) True.Invoke();
            else False.Invoke();
        }
    }
}