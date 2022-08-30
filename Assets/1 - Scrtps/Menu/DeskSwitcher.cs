using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;

namespace Menu
{
    public class DeskSwitcher : MonoBehaviour
    {
        public ReadOnlyCollection<LevelDesk> Desks => _desks.AsReadOnly();
        
        [SerializeField] private List<LevelDesk> _desks;
        [ShowInInspector, ReadOnly]private LevelDesk _currentDesk;

        private void Start()
        {
            var levels = Services<GroupLevel>.S.Get().Levels;
            var setedLevel = new List<Level>();
            _currentDesk = _desks[0];
            _desks.ForEach(x=> setedLevel.AddRange( x.Init(levels.Except(setedLevel).ToList().AsReadOnly())));
            Services<PointLevel>.S.Get().InstanceMove(_desks[0].Datas.First(x=>x.View is LevelView).View);
            
            _desks.ForEach(x=>x.gameObject.SetActive(false));
            SwapDesk(_desks[0]);
        }

        public void SwapDesk(LevelDesk newDesk)
        {
            _currentDesk?.gameObject.SetActive(false);
            _currentDesk = newDesk;
            _currentDesk.transform.position = Vector3.zero;
            _currentDesk.gameObject.SetActive(true);
        }

        public LevelView GetViewByLevel(Level l)
        {
            var targetDesk = _desks.FirstOrDefault(d => d.Datas.FirstOrDefault(s => s.LevelSo == l) != null);
            if (targetDesk) return targetDesk.Datas.FirstOrDefault(x =>
            {
                if(x.View is LevelView)
                    return x.LevelSo == l;
                return false;
            }).View as LevelView;
            return null;
        }
    }
}