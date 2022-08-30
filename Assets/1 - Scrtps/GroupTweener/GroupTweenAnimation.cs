using UnityEngine;

namespace GroupTweener
{
    public abstract class GroupTweenAnimation : MonoBehaviour
    {
        public abstract float Duration { get; }

        public abstract void Prepare(GameObject target);
        
        public abstract void Play(GameObject target);
    }
}