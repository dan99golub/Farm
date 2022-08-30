using System.Collections.Generic;
using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using UnityEngine;

namespace Game.Dirty
{
    public class UpdateGreenQuarter : BaseGreenUpdateTex
    {
        public List<Data> Datas;
        public GreenMark MyMark;
        
        protected override void OnInit(Tile t)
        {
            UpdateTex();
        }

        public override void UpdateTex()
        {
            Datas.ForEach(x=>x.Update(InitedTile.Tile.Position, ServicesID<Field>.S.Get(), MyMark));
        }

        [System.Serializable]
        public class Data
        {
            public Vertical Vert;
            public Horizontal Horz;

            public Material MiniCorner;
            public Material VertLine;
            public Material HorLine;
            public Material Corner;

            public MeshRenderer Target;

            public void Update(Vector2Int currentPos, Field field, GreenMark myMark)
            {
                bool cornerGreen = GetGreen(field.GetTile(Tile.GetGuid(currentPos + new Vector2Int((int) Horz, (int) Vert))), myMark);
                bool vert = GetGreen(field.GetTile(Tile.GetGuid(currentPos + new Vector2Int(0, (int) Vert))),myMark);
                bool hor = GetGreen(field.GetTile(Tile.GetGuid(currentPos + new Vector2Int((int) Horz, 0))),myMark);

                var mat = GetMat(cornerGreen, vert, hor);
                if (mat) Target.material = mat;
                else Target.materials = new Material[0];
            }

            private Material GetMat(bool cornerGreen, bool vert, bool hor)
            {
                if ((cornerGreen && vert && hor) || (!cornerGreen && vert && hor)) return Corner;
                else if (cornerGreen && !vert && !hor) return MiniCorner;
                else if ((cornerGreen && hor && !vert) || (!cornerGreen && hor && !vert)) return VertLine;
                else if ((cornerGreen && !hor && vert) || (!cornerGreen && !hor && vert)) return HorLine;
                return null;
            }

            private bool GetGreen(Tile t, GreenMark myMark)
            {
                if (t.Content == null) return false;
                var mark = t.Content.GetComponent<GreenMark>();
                if (mark == null) return false;
                if (mark.IDGreen == myMark.IDGreen) return false;
                return mark.IsGreen;
            }


            public enum Vertical { Up = 1, Down = -1 }
            
            public enum Horizontal { Right = 1, Left = -1}
        }
    }
}