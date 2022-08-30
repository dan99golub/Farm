using System;
using DefaultNamespace.Game.UFOs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.SM
{
    [RequireComponent(typeof(State))]
    public abstract class StateAction : MonoBehaviour
    {
        public State State;

        private void Awake() => Register(State);

        protected abstract void Register(State s);

        [Button]
        private void OnValidate() => State = GetComponent<State>();
    }
}