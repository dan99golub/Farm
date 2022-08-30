using System.Collections.Generic;
using Lean.Gui;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlaySoundButton : MonoBehaviour
    {
        public List<LeanButton> Buttons;
        public SoundChanel chanel;
        public AudioClip clip;
        
        public SoundManager SoundManag => Services<SoundManager>.S.Get();

        private void Awake()
        {
            Buttons.ForEach(x => x.OnClick.AddListener(Play));
        }

        public void Play()
        {
            SoundManag.Play(chanel, clip);
        }
    }
}