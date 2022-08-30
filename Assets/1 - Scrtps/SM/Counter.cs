using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class Counter : MonoBehaviour
    {
        [Min(0)]public int Target;
        [Min(0)]public int Current;

        public UltEvent Loop;
        public UltEvent End;

        public void Zero() => Current = 0;

        public void Next()
        {
            Current++;
            if(Current==Target) End.Invoke();
            else Loop.Invoke();
        }
    }
}