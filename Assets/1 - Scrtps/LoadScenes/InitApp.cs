using DefaultNamespace;
using DTO;
using Menu;
using Menu.Shop_Component;
using ServiceScript;
using SO;
using UnityEngine;
using UnityEngine.Audio;

namespace LoadScenes
{
    public class InitApp : RegisterServicesScene
    {
        public AudioMixer Mixer;
        public GroupLevel GroupLevel;
        public ProductContainer Products;
        public DB DB;
        public PanelUI FadeScreen;
        public SoundManager SoundMan;
        public MixerController.DTONameChanel NameForMixerControl;
        public int TargetFps;
        
        public override void Register()
        {
            Add(Mixer);
            Add(GroupLevel);
            Add(Products);
            Add(SoundMan);
            
            AddId(FadeScreen, ConstGame.FadeScreen);
            Services<DB>.S.Set(DB);
            Application.targetFrameRate = TargetFps;
            DB.Load();
            
            if (Services<MixerController>.S.Get() == null)
            {
                var mixerController = new MixerController(Mixer, NameForMixerControl);
                Add(mixerController);
            }

            CorutineGame.Instance.WaitFrame(1, () =>
            {
                Services<MixerController>.S.Get().SetValueFromSetting(DB.SettingGame);
            });
        }

        public override void Unregister()
        {
            DB.Save();
            Services<SoundManager>.S.Set(null);
            Services<DB>.S.Set(null);
        }

        private void Add<T>(T instancec)
        {
            if (Services<T>.S.Get() == null) Services<T>.S.Set(instancec);
        }
        
        private void AddId<T>(T instancec, string id)
        {
            if (ServicesID<T>.S.Get(id) == null) ServicesID<T>.S.Set(instancec, id);
        }
    }
}