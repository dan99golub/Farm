using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DTO
{
    public class DB : MonoBehaviour
    {
        private bool CanEdit => Application.isPlaying;

        public SettingGame SettingGame => _settingGame;
        [SerializeReference, ShowInInspector, EnableIf("CanEdit")] private SettingGame _settingGame;
        
        public Progress Progress => _progress;
        [SerializeReference, ShowInInspector, EnableIf("CanEdit")] private Progress _progress;

        [Button]public void Load()
        {
            _settingGame = LoadPref<SettingGame>("Setting");
            _progress = LoadPref<Progress>("SaveData");
        }

        [Button, EnableIf("CanEdit")] public void Save()
        {
            SavePref( _settingGame, "Setting");
            SavePref(_progress, "SaveData");
        }
        
        public void ClearAll()
        {
            _settingGame = new SettingGame();
            _progress = new Progress();
            Save();
        }

        private T LoadPref<T>(string id) => JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(id, "{}"));

        private void SavePref(object obj, string id) => PlayerPrefs.SetString(id, JsonConvert.SerializeObject(obj));
    }
}