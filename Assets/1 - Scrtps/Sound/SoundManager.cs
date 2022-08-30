using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class SoundManager : SerializedMonoBehaviour
    {
        public List<SourceObj> Sources;

        public AudioSource Play(SoundChanel chanel, AudioClip clip) => Sources.FirstOrDefault(x=>x.SoundChanel==chanel)?.Play(clip);

        [System.Serializable]
        public abstract class SourceObj
        {
            public SoundChanel SoundChanel;

            public AudioSource Play(AudioClip clip)
            {
                var source = GetSource();
                source.Stop();
                source.time = 0;
                source.clip = clip;
                source.Play();
                return source;
            }

            protected abstract AudioSource GetSource();
        }

        public class SingleSource : SourceObj
        {
            public AudioSource Source;
            
            protected override AudioSource GetSource() => Source;
        }
    }
}