using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.AnimScripts
{
    public class RandomizeAnimation : MonoBehaviour
    {
        [SerializeReference] public List<Act> Acts;
        public MonoClipTransition ClipTransition;

        public void Randomaize() => Acts.ForEach(x=>x.Make(ClipTransition));

        [System.Serializable]
        public abstract class Act
        {
            public abstract void Make(MonoClipTransition transition);
        }
        
        [System.Serializable]
        public class SetRandomTime : Act
        {
            [Range(0,1f)] public float _min;
            [Range(0,1f)] public float _max;
            
            public override void Make(MonoClipTransition transition)
            {
                transition.State.NormalizedTime = Random.Range(_min, _max);
            }
            
        }
        
        [System.Serializable]
        public class SetRandomSpeed : Act
        {
            public float _min;
            public float _max;
            
            public override void Make(MonoClipTransition transition)
            {
                var v = Random.Range(_min, _max);
                transition.Clip.Speed = v;
                transition.State.Speed = v;
            }
            
        }
    }
}