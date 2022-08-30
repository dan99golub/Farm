using System;
using DTO;
using Menu;
using ServiceScript;
using UnityEngine;
using UnityEngine.Audio;

namespace DefaultNamespace
{
    public class SettingControl : MonoBehaviour
    {
        public SettingGame Setting => Services<DB>.S.Get() ? Services<DB>.S.Get().SettingGame : null;
        public MixerController MixerController => Services<MixerController>.S.Get();

        public PanelUI VibrationBtn;
        public PanelUI MusicBtn;
        public PanelUI SoundBtn;
        public PanelUI InputBtn;

        private void Awake()
        {
            CorutineGame.Instance.WaitFrame(2, ()=> SetPanelActive());
            Setting.Signals.Register<Updated>(Handler);
        }

        public void ChangeValue(TypeSetting setting)
        {
            switch (setting)
            {
                case TypeSetting.Music:
                    Setting.Music = !Setting.Music;
                    MixerController.SetValueFromSetting(Setting);
                    break;
                case TypeSetting.Sound:
                    Setting.Sound = !Setting.Sound;
                    MixerController.SetValueFromSetting(Setting);
                    break;
                case TypeSetting.Vibation:
                    Setting.Vibration = !Setting.Vibration;
                    break;
                case TypeSetting.InputType:
                    if (Setting.InputType == SettingGame.InputKey.Button) Setting.InputType = SettingGame.InputKey.Swipe;
                    else Setting.InputType = SettingGame.InputKey.Button;
                        break;
            }
            Setting.Signals.Fire(new Updated());
        }

        private void OnDestroy() => Setting?.Signals.UnRegister<Updated>(Handler);
        
        private void Handler(Updated obj) => SetPanelActive();

        private void SetPanelActive()
        {
            VibrationBtn?.SetActive(Setting.Vibration);
            MusicBtn?.SetActive(Setting.Music);
            SoundBtn?.SetActive(Setting.Sound);
            InputBtn?.SetActive(Setting.InputType == SettingGame.InputKey.Swipe);
        }
        
        public enum TypeSetting
        {
            Music, Sound, Vibation, InputType
        }
    }

    public class MixerController
    {
        private readonly AudioMixer _mixer;
        private DTONameChanel _names;

        
        public MixerController(AudioMixer mixer, DTONameChanel namesChanel)
        {
            _mixer = mixer;
            _names = namesChanel;
        }

        public void SetValueFromSetting(SettingGame setting)
        {
            SetMusic(setting.Music ? 0 : -80);
            SetSound(setting.Sound ? 0 : -80);
        }
        
        public void SetMusic(float value) => _mixer.SetFloat(_names.Music, value);
        
        public void SetSound(float value) => _mixer.SetFloat(_names.Sound, value);
        
        [System.Serializable]
        public struct DTONameChanel
        { 
            public string Music;
            public string Sound;
        }
    }
}