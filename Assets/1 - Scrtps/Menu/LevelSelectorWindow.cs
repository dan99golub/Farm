using System;
using System.Collections.Generic;
using DTO;
using ServiceScript;
using Sirenix.Utilities;
using SO;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelectorWindow : MonoBehaviour
    {
        public Button PrefabButton;
        public Transform ContentPoint;
        public RectTransform PointerSelectedLevel;
        public UltEvent ExtraActionForLevelbtn;

        private Dictionary<Level, Button> _buttons = new Dictionary<Level, Button>();
        
        private LevelSelector LevelSelector => Services<LevelSelector>.S.Get();
        private Progress Progres => Services<DB>.S.Get().Progress;

        private void Awake()
        {
            Level prevLevel = null; 
            LevelSelector.Levels.Levels.ForEach(x =>
            {
                var newButton = Instantiate(PrefabButton, ContentPoint);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = x.name;
                newButton.onClick.AddListener(()=>
                {
                    LevelSelector.Select(x);
                    ExtraActionForLevelbtn.Invoke();
                });
                newButton.gameObject.SetActive(true);
                
                _buttons.Add(x, newButton);
                if (Progres.LevelIsPass(x) || prevLevel==null)
                    newButton.interactable = true;
                else
                    newButton.interactable = Progres.LevelIsPass(prevLevel);
                
                prevLevel = x;
            });
            
            LevelSelector.NewSelected += OnNewLevelSelected;
            OnNewLevelSelected(LevelSelector.CurrentLevel);
        }

        private void OnNewLevelSelected(Level obj)
        {
            _buttons.TryGetValue(obj, out var b);
            PointerSelectedLevel.SetParent(b.transform);
            PointerSelectedLevel.anchoredPosition = Vector2.zero;
        }
    }
}