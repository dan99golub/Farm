using DefaultNamespace.Game;
using UnityEngine;

namespace DefaultNamespace
{
    public class RandomPointOnFieldInZone : RandomPointOnField
    {
        public override (Vector3 movePos, Vector2Int posTile) GetPoint()
        {
            return ReturnPointFromTiles(GetUnsortedTile());
        }
    }
}