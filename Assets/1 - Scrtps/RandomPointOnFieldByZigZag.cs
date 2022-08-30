using System;
using System.Collections;
using System.Linq;
using DefaultNamespace.Game;
using Menu;
using UnityEngine;

namespace DefaultNamespace
{
    public class RandomPointOnFieldByZigZag : RandomPointOnField
    {
        [Min(0)] public float MaxDistance;

        public override (Vector3 movePos, Vector2Int posTile) GetPoint()
        {
            var tiles = GetUnsortedTile().Where(x => Vector3.Distance(x.transform.position, transform.position) < MaxDistance);
            return ReturnPointFromTiles(tiles);
        }
    }
}