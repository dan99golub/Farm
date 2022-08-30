using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LookAt : MonoBehaviour
    {
        public Transform Target;

        private void Update()
        {
            if(Target) transform.LookAt(Target);
        }
    }
}