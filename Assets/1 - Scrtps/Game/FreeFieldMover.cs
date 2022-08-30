using System;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FreeFieldMover : AbsFieldMover
    {
        public float SpeedMove
        {
            get => Speed;
            set => Speed = value;
        }
        
        [Min(0.01f)]public float Speed;
        [Min(0.01f)]public float H;
        
        public override event Action<(Tile @from, Tile to)> Moving;
        public override event Action<Tile> ReachPoint;
        public override event Action<DirectionType> NewDir;
        
        private Field _f;
        private Vector3 _max;
        private Vector3 _min;

        private DirectionType _currentMoveType;
        private float _pastDir;

        public override DirectionType Direction => _currentMoveType;

        public override void Init(Field field)
        {
            _f = field;
            _min = new Vector3(0, 0, 0);
            _max = new Vector3(field.Size.x-0.9f, 0, field.Size.y-0.9f);
            transform.position = new Vector3(transform.position.x, H, transform.position.z);
            Move(DirectionType.None);
        }

        private bool _blockMove =false;

        private void OnEnable() => _blockMove = false;

        private void OnDisable()
        {
            Move(DirectionType.None);
            _blockMove = true;
        }

        public override void Move(DirectionType d)
        {
            if(_blockMove) return;
            if (CanMove(d) == false)
            {
                if(_currentMoveType!=DirectionType.None)
                    Move(DirectionType.None);
                return;
            }
            if(_currentMoveType == InvertDir(d)) return;

                NewDir.Invoke(_currentMoveType = d);
            InvokeRichPoint();
            if (_currentMoveType != DirectionType.None)
            {
                InvokeMovingEvent();
            }

            _pastDir = 0;
        }

        private DirectionType InvertDir(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.Right: return DirectionType.Left;
                case DirectionType.Left: return DirectionType.Right;
                case DirectionType.Up: return DirectionType.Down;
                case DirectionType.Down: return DirectionType.Up;
                default: return DirectionType.None;
            }
        }

        private bool CanMove(DirectionType dir) => _f.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position + DirToVector(dir)))) != null;

        private void Update()
        {
            if(_currentMoveType!=DirectionType.None)
                MoveAt(_currentMoveType);
        }

        private void MoveAt(DirectionType d)
        {
            Vector3 dir = DirToVector(d);
            var pastDir = dir * Time.deltaTime * SpeedMove;
            transform.position += pastDir;
            _pastDir += pastDir.magnitude;
            if (_pastDir > 1)
            {
                _pastDir-=1;
                InvokeRichPoint();  
                if(CanMove(_currentMoveType))
                    InvokeMovingEvent();
            }
            CheckMoveBorder();
        }

        private void CheckMoveBorder()
        {
            if (transform.position.x < _min.x || transform.position.x > _max.x || transform.position.z < _min.z || transform.position.z > _max.z)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, _min.x+0.1f, _max.x-0.1f), H, Mathf.Clamp(transform.position.z, _min.z+0.1f, _max.z-0.1f));
                Move(DirectionType.None);
            }
        }

        private void InvokeRichPoint(int countTileBack)
        {
            ReachPoint?.Invoke(_f.GetTile(Tile.GetGuid(Tile.GetTilePos(DirToVector(_currentMoveType)*countTileBack+transform.position))));
        }

        private void InvokeRichPoint()
        {
            var current = _f.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position)));
            ReachPoint?.Invoke(current);
        }

        private void InvokeMovingEvent()
        {
            var current = _f.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position)));
            var next = _f.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position+DirToVector(_currentMoveType))));
            Moving?.Invoke((current, next));
        }

        private Vector3 DirToVector(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.Down: return Vector3.forward * -1;
                case DirectionType.Up: return Vector3.forward;
                case DirectionType.Right: return Vector3.right;
                case DirectionType.Left: return Vector3.right * -1;
                default: return Vector3.zero;
            }
        }
    }
}