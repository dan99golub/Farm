using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class RandomPointOnField : MonoBehaviour
    {
        public Vector2Int CurrentPosition => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        public ZoneManager ZoneManager => Services<ZoneManager>.S.Get();
        public Field Field => ServicesID<Field>.S.Get();
        public bool InAllField;
        
        public abstract (Vector3 movePos, Vector2Int posTile) GetPoint();

        protected IEnumerable<Tile> GetUnsortedTile()
        {
            if (InAllField) return Field.Tiles.Where(x=>x.Content!=null);
            else return ZoneManager.GetZoneByTile(Field.GetTile(Tile.GetGuid(CurrentPosition))).Tiles;
        }
        
        protected (Vector3 movePos, Vector2Int posTile) ReturnPointFromTiles(IEnumerable<Tile> Tiles)
        {
            if(Tiles.Count()==0) return (transform.position, CurrentPosition);
            var t = Tiles.GetRandom();
            return (t.transform.position, t.Position);
        }
    }
}