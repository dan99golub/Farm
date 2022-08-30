using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FieldMover : AbsFieldMover
    {
        public override event Action<(Tile from, Tile to)> Moving;
        public override event Action<Tile> ReachPoint;
        public override event Action<DirectionType> NewDir;

        public override DirectionType Direction => _direction;
        public float DurationUnitMove { get => _durationUnitMove; set => _durationUnitMove = value; }
        
        [Min(0.1f)][SerializeField] private float _durationUnitMove;
        [Min(0.1f)][SerializeField] private float _verticalOffset;
        [SerializeField] private Field _field;

        [ShowInInspector, ReadOnly] private DirectionType _direction;
        [ShowInInspector, ReadOnly] private Tile _currentTile;
        private Tween _tween;
        private Action _customCallbackTween;
        private Tile _nextTile;
        private bool СanChangeDir => _direction == DirectionType.None ? true : _canChangeDir;
        private bool _canChangeDir=true;

        private void OnDisable() => _tween?.Kill();

        public override void Init(Field field)
        {
            ReachPoint += x => _canChangeDir = true;
            _field = field;
            _currentTile = _field.Tiles.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
            transform.position = GetPosition(_currentTile);
        }

        [Button] public override void Move(DirectionType d)
        {
            if(enabled==false || _field==null || CorrectChangeDiraction(d)==false || СanChangeDir == false) return;
            
            if (_tween != null) // смена направления на следующей точки
            {
                var act = _tween.onComplete;
                _customCallbackTween = null;
                _tween?.Kill();
                act();
                _tween?.Kill();
                Move(d);
                _canChangeDir = false;
                return;
            }
            
            NewDir?.Invoke(_direction = d);
            if (d == DirectionType.None) // остановка
                return;

            _nextTile = GetTileForMove(_currentTile);
            if (_nextTile == null || IsCorrectTile(_nextTile) == false)
            {
                Move(DirectionType.None);
                return;
            }
            
            _customCallbackTween = ()=>Move(_direction);
            _tween = MakeTweenMove(_nextTile);
        }

        private Tile GetTileForMove(Tile from) => _field.GetTile(Tile.GetGuid(from.Position + ConvertEnumToVector2Int(_direction)));

        private Tween MakeTweenMove(Tile target)
        {
            var targetPos = GetPosition(target);
            return _tween = transform.DOMove(targetPos, GetDuration(transform.position, targetPos, _durationUnitMove)).SetEase(Ease.Linear).OnComplete(() =>
            {
                _currentTile = target;
                _tween = null;
                ReachPoint?.Invoke(_currentTile);
                _customCallbackTween?.Invoke();
            }).OnStart(() =>
            {
                Moving?.Invoke((_currentTile, target));
            });
        }

        private float GetDuration(Vector3 transformPosition, Vector3 targetPos, float durationUnitMove) => Vector3.Distance(transformPosition, targetPos) * durationUnitMove;

        private bool IsCorrectTile(Tile moveToTile)
        {
            return moveToTile.Content != null;
        }

        private Vector3 GetPosition(Tile t)
        {
            return  new Vector3(0,_verticalOffset,0)+_field.TilePosToGlobalUnityPos(t.Position);
        }

        private bool CorrectChangeDiraction(DirectionType newDir)
        {
            if (_direction == DirectionType.None) return true;
            else if (_direction == DirectionType.Left && newDir == DirectionType.Right) return false;
            else if (_direction == DirectionType.Right && newDir == DirectionType.Left) return false;
            else if (_direction == DirectionType.Down && newDir == DirectionType.Up) return false;
            else if (_direction == DirectionType.Up && newDir == DirectionType.Down) return false;
            return true;
        }

            
        private Vector2Int ConvertEnumToVector2Int(DirectionType e)
        {
            switch (e)
            {
                case DirectionType.None: return Vector2Int.zero;
                case DirectionType.Right: return new Vector2Int(1,0);
                case DirectionType.Left: return new Vector2Int(-1,0);
                case DirectionType.Up: return new Vector2Int(0, 1);
                case DirectionType.Down: return new Vector2Int(0, -1);
                default: return Vector2Int.zero;
            }
        }
    }
}