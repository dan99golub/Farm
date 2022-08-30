using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game;
using UnityEngine;

namespace DefaultNamespace
{
    public class RandomPointOnFieldOnLine : RandomPointOnField
    {
        [Min(0)] public int SearchDistance;
        
        public override (Vector3 movePos, Vector2Int posTile) GetPoint()
        {
            var t = Field.GetTile(Tile.GetGuid(CurrentPosition));
            var dict = GetUnsortedTile().ToDictionary(tile => tile.Position);

            List<Tile> _points=new List<Tile>();
            for (int x = t.Position.x-SearchDistance; x <= t.Position.x+SearchDistance; x++)
            {
                if(dict.TryGetValue(new Vector2Int(x, t.Position.y), out var r))
                    _points.Add(r);
            }
            
            for (int y = t.Position.x-SearchDistance; y <= t.Position.x+SearchDistance; y++)
            {
                if(dict.TryGetValue(new Vector2Int(t.Position.x, y), out var r))
                    _points.Add(r);
            }

            return ReturnPointFromTiles(_points);
        }
    }
}