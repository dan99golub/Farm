using UnityEngine;

namespace DefaultNamespace.Game
{
    public class ProgressMark : MonoBehaviour
    {
        public State CurrentState => _state;
        [SerializeField] private State _state;

        public void Set(State s) => _state = s;

        public State GetState(State s) => s;
        
        public enum State
        {
            Ignore, Complete, Incomplete
        }
    }
}