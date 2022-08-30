using System;
using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Dirty
{
    public class GreenUpdateTex : BaseGreenUpdateTex
    {
        public MeshRenderer MeshRenderer;
        public GreenTextRule Rule;

        public Material[] DefaultMat;
        private int _matIndex;

        private Field MainField => ServicesID<Field>.S.Get();

        protected override void OnInit(Tile obj)
        {
            _matIndex = Random.Range(0, DefaultMat.Length);
            UpdateTex();
        }

        [SerializeField] private GreenTextRule.TilesIsGreen _currentRules;
        [Button]public override void UpdateTex()
        {
            var newRule = new GreenTextRule.TilesIsGreen();
            newRule.DL = IsGreen(new Vector2Int(-1, -1), "DL");
            newRule.DC = IsGreen(new Vector2Int(0, -1), "DC");
            newRule.DR = IsGreen(new Vector2Int(1, -1), "DR");
            
            newRule.ML = IsGreen(new Vector2Int(-1, 0), "ML");
            newRule.MC = IsGreen(new Vector2Int(0, 0), "MC");
            newRule.MR = IsGreen(new Vector2Int(1, 0), "MR");
            
            newRule.UL = IsGreen(new Vector2Int(-1, 1), "UL");
            newRule.UC = IsGreen(new Vector2Int(0, 1), "UC");
            newRule.UR = IsGreen(new Vector2Int(1, 1), "UR");

            _currentRules = newRule;

            var mat = Rule.GetMatOrNull(_currentRules);
            SetMat(mat!=null ? mat : DefaultMat[_matIndex]);
        }

        private void SetMat(Material mat) => MeshRenderer.material = mat;

        private bool IsGreen(Vector2Int dir, string def = "")
        {
            var t = MainField.GetTile(Tile.GetGuid(InitedTile.Tile.Position + dir));
            if (t == null) return false;
            if (t.Content == null) return false;
            var mark = t.Content.GetComponent<GreenMark>();
            if (mark == null) return false;
            return mark.IsGreen;
        }
    }
}