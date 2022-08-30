using System;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReciverSoundManager : MonoBehaviour
    {
        public SoundChanel chanel;
        public AudioClip clip;

        public SoundManager SoundManag => Services<SoundManager>.S.Get();

        public AudioSource Play() => Play(chanel, clip);
        
        public void PlayVoid() => Play(chanel, clip);
        
        public AudioSource Play(SoundChanel c, AudioClip cl) => SoundManag.Play(c, cl);

        public void SetPosition(AudioSource source, Vector3 pos) => source.transform.position = pos;
        
        public void SetLocalPosition(AudioSource source, Vector3 pos) => source.transform.localPosition = pos;
    }
}