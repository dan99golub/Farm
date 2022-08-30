using System.Runtime.InteropServices;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class GreeningZone : MonoBehaviour
    {
        [Min(1)] public int W;
        [Min(1)] public int H;
        public bool IsOnClockwise;

        private Field MainField => ServicesID<Field>.S.Get();
        private C_GameScene Controller => Services<C_GameScene>.S.Get();
        
        public void Set(Dir d, bool isOnClockwise)
        {
            var startDir = d;
            Field.TilePoint firstPoint = new Field.TilePoint(null, MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position))));
            var currentPoint = firstPoint;
            var currentVector = currentPoint.Content.transform.position;
            do
            {
                for (int i = 0; i < GetIndex(d); i++)
                {
                    var vectorNextTile = NextVector(currentVector, d);
                    var nextTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(vectorNextTile)));
                    currentVector = vectorNextTile;
                    if(nextTile==null) continue;
                    if(nextTile.Content==null) continue;
                    currentPoint = new Field.TilePoint(currentPoint, nextTile);
                }
                
                d = NextDir(d, isOnClockwise);
            } while (d != startDir);
            currentPoint.InitPrevMe();
            Controller.SetFenceByPath((firstPoint, currentPoint));
        }

        public Dir NextDir(Dir current, bool isOnClockwise)
        {
            if (isOnClockwise)
            {
                switch (current)
                {
                    case Dir.XMinus: return Dir.ZPlus;
                    case Dir.XPlus: return Dir.ZMinus;
                    case Dir.ZPlus: return Dir.XPlus;
                    case Dir.ZMinus: return Dir.XMinus;
                }
            }
            else
            {
                switch (current)
                {
                    case Dir.XMinus: return Dir.ZMinus;
                    case Dir.XPlus: return Dir.ZPlus;
                    case Dir.ZPlus: return Dir.XMinus;
                    case Dir.ZMinus: return Dir.XPlus;
                }
            }

            return current;
        }

        private int GetIndex(Dir d)
        {
            if (d == Dir.XPlus || d == Dir.XMinus) return W;
            return H;
        }

        public void Set(AbsFieldMover.DirectionType moverDirection)
        {
            if(moverDirection== FieldMover.DirectionType.Down) Set(Dir.ZMinus,GetIsOnClockWise(Dir.ZMinus));
            else if(moverDirection== FieldMover.DirectionType.Up) Set(Dir.ZPlus,GetIsOnClockWise(Dir.ZPlus));
            else if(moverDirection== FieldMover.DirectionType.Right) Set(Dir.XPlus,GetIsOnClockWise(Dir.XPlus));
            else if(moverDirection== FieldMover.DirectionType.Left) Set(Dir.XMinus, GetIsOnClockWise(Dir.XMinus));
        }

        private bool GetIsOnClockWise(Dir dir)
        {
            var rotDir = NextDir(dir, IsOnClockwise);
            var currentTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position)));
            var currentVector = transform.position;
            for (int i = 0; i < GetIndex(rotDir); i++)
            {
                currentVector = NextVector(currentVector, rotDir);
                currentTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(currentVector)));
                if (currentTile == null) return !IsOnClockwise;
            }
            return IsOnClockwise;
        }

        public Vector3 NextVector(Vector3 start, Dir d)
        {
            if (d == Dir.XMinus) return start + Vector3.right * -1;
            if (d == Dir.XPlus) return start + Vector3.right;
            if (d == Dir.ZPlus) return start + Vector3.forward;
            else return start + Vector3.forward * -1;
        }

        public enum Dir
        {
            XPlus, XMinus, ZPlus, ZMinus,  
        }
    }
}