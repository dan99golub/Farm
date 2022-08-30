using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

namespace Menu
{
    [RequireComponent(typeof(Path), typeof(LevelDeskEdit))]
    public class LevelDesk : MonoBehaviour
    {
        public Path Path => GetComponent<Path>();
        
        [SerializeField] private List<Data> _datas;

        public ReadOnlyCollection<Data> Datas => _datas.AsReadOnly();

        public List<Level> Init(ReadOnlyCollection<Level> levels)
        {
            List<Level> result = new List<Level>();

            int i = 0;
            int iLevel = 0;
            while (i<_datas.Count())
            {
                if(iLevel>=levels.Count) break;
                if (_datas[i].Init(levels[iLevel], this))
                {
                    result.Add(levels[iLevel]);
                    iLevel++;    
                }
                i++;
            }
            
            return result;
        }

        public Path.Node GetNode(StopPointPointLevel view) => Path.GetPoint(_datas.FirstOrDefault(x => x.View == view).PointGUID);

        [Button]private void UpdateData()
        {
            _datas= new List<Data>();
            GetComponentsInChildren<StopPointPointLevel>().ForEach(x => _datas.Add(new Data(x)));
        }
        
        [System.Serializable]
        public class Data
        {
            [SerializeField, Sirenix.OdinInspector.ReadOnly] private StopPointPointLevel _view;
            [ShowInInspector, Sirenix.OdinInspector.ReadOnly]private Level _levelSo;
            [SerializeField, EditID("Point"), Sirenix.OdinInspector.ReadOnly] private string pointGuid;

            public StopPointPointLevel View => _view;
            public Level LevelSo => _levelSo;
            public string PointGUID => pointGuid;
            
            public Data(StopPointPointLevel view) => _view = view;

            public bool Init(Level levelSo, LevelDesk desk)
            {
                _view.SetDesk(desk);
                if (_view is LevelView)
                {
                    _levelSo = levelSo;
                    (_view as LevelView).Init(levelSo);
                    return true;
                }
                return false;
            }
        }
    }
}