using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Dirty
{
    [CreateAssetMenu(order = 51, menuName = "Game/DirtyRule")]
    public class GreenTextRule : ScriptableObject
    {
        [SerializeField] private List<Data> _datas;

        public Material GetMatOrNull(TilesIsGreen rule)
        {
            foreach (var data in _datas)
            {
                var m =data.TryGet(rule);
                if (m != null) return m;
            }
            return null;
        }

        [System.Serializable]
        public class Data
        {
            public List<TilesIsGreen> Rules;
            
            public Material Mat;
            [PreviewField(ObjectFieldAlignment.Center), ReadOnly]
            public Texture Text;

            public Material TryGet(TilesIsGreen rule)
            {
                foreach (var tilesIsGreen in Rules)
                {
                    if (tilesIsGreen.Compare(rule)) return Mat;
                }
                return null;
            }

            [Button]public void OnValid()
            {
                if(Mat)
                    Text = Mat.GetTexture("_MainTex");
            }
        }

        [System.Serializable]
        public class TilesIsGreen
        {
            [HorizontalGroup("S")] [LabelWidth(20)] [VerticalGroup("S/Left")]
            public bool UL, ML, DL;

            [VerticalGroup("S/Center")] [LabelWidth(20)]
            public bool UC, MC, DC;


            [VerticalGroup("S/Right")] [LabelWidth(20)]
            public bool UR, MR, DR;

            public bool Compare(TilesIsGreen other)
            {
                var r = other.UL == UL && other.ML == ML && other.DL == DL &&
                        other.UC == UC && other.MC == MC && other.DC == DC &&
                        other.UR == UR && other.MR == MR && other.DR == DR;
                return r;
            }
        }
        
        private void OnValidate() => _datas.ForEach(x=>x.OnValid());
    }
}