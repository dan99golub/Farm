using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public class ChanceEvent : MonoBehaviour
    {
        [Range(0,1f)]public float Chance;

        public UltEvent Event;

        public void Invoke()
        {
            if(Random.Range(0,1f)<Chance) Event.Invoke();
        }
    }
}