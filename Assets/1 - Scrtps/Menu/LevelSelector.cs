using System;
using System.Text.RegularExpressions;
using DTO;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;

namespace Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public GroupLevel Levels => Services<GroupLevel>.S.Get();
        public Level CurrentLevel => _currentLevel; 
        public event Action<Level> NewSelected;
        
        [ReadOnly, ShowInInspector] private Level _currentLevel;
        private DB Db => Services<DB>.S.Get();

        private void Start() => Select(Levels.Levels[0]);

        [Button]public void Select(Level level)
        {
            _currentLevel = level;
            NewSelected?.Invoke(_currentLevel);
        }

        [Button]
        public bool Next()
        {
            if (!CanSelectNext()) return false;
            var newIndex = Levels.Levels.IndexOf(_currentLevel)+1;
            Select(Levels.Levels[newIndex]);
            return true;
        }

        [Button]
        public void Prev()
        {
            var newIndex = Levels.Levels.IndexOf(_currentLevel)-1;
            if(newIndex<0) return;
            Select(Levels.Levels[newIndex]);
        }
        
        public bool CanSelectNext() => Db.Progress.LevelIsPass(CurrentLevel) &&  Levels.Levels.IndexOf(_currentLevel)+1 < Levels.Levels.Count;
    }
}