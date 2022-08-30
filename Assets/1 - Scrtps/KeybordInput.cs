using System;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public class KeybordInput : MonoBehaviour
    {
        [SerializeField] private List<Key> _kyes = new List<Key>();

        public void Update()
        {
            _kyes.ForEach(x=>x.MakeInput());
        }

        [System.Serializable]
        public class Key
        {
            public KeyCode Code;
            public UltEvent Down;
            public UltEvent Hold;
            public UltEvent Up;

            public void MakeInput()
            {
                if(Input.GetKeyDown(Code)) Down.Invoke();
                if(Input.GetKey(Code)) Hold.Invoke();
                if(Input.GetKeyUp(Code)) Up.Invoke();
            }
        }
    }
}