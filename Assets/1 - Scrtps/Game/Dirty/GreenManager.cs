using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Dirty
{
    public class GreenManager : MonoBehaviour
    {
        private HashSet<BaseGreenUpdateTex> _tex = new HashSet<BaseGreenUpdateTex>();
        
        public void Add(BaseGreenUpdateTex updateTex) => _tex.Add(updateTex);

        public void Remove(BaseGreenUpdateTex updateTex) => _tex.Remove(updateTex);

        [Button]public void UpdateTex() => _tex.ForEach(x => x.UpdateTex());

        public void UpdateNeirBus(InitedTile initedTile)
        {
            var updateTex = initedTile.GetComponentInChildren<BaseGreenUpdateTex>();
            if(updateTex==null) return;
            updateTex.UpdateTex();
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(-1, -1));
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(0, -1));
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(1, -1));
            
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(-1, 0));
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(1, 0));
            
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(-1, 1));
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(0, 1));
            FindTileByOffsetAndUpdate(updateTex, new Vector2Int(1, 1));
        }

        private void FindTileByOffsetAndUpdate(BaseGreenUpdateTex updateTex, Vector2Int offset)
        {
            _tex.FirstOrDefault(x => x.InitedTile.Tile.Position == updateTex.InitedTile.Tile.Position -offset)?.UpdateTex();
        }
    }
}