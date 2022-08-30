using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Group Level", menuName = "Game/Group Level", order = 51)]
    public class GroupLevel : ScriptableObject
    {
        [SerializeField] private List<Level> _levels;
        public ReadOnlyCollection<Level> Levels => _levels.AsReadOnly();

        public Level NextLevelOrNull(Level prev)
        {
            var  i =_levels.IndexOf(prev);
            if (i >= _levels.Count - 1) return null;
            return _levels[i + 1];
        }

        public int NumberOrderLevel(Level currentLevel)
        {
            return _levels.IndexOf(currentLevel) + 1;
        }

        public Level GetPrev(Level level)
        {
            var i = _levels.IndexOf(level)-1;
            if (i >= 0) return _levels[i];
            return null;
        }
    }
}