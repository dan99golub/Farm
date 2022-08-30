using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using ServiceScript;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class CheckMoveNotOnBorder : MonoBehaviour
    {
        public AbsFieldMover FieldMover;
        public TriggerCheckMover Trigger;

        public UltEvent<Tile> StartMove;
        public UltEvent<Tile> MovingTo;
        public UltEvent<Tile> ReachTile;
        public UltEvent<GameObject> Broken;
        public UltEvent<(Field.TilePoint startPoint, Field.TilePoint finishPoint)> Finish;
        public UltEvent StopCheck;
        
        [ShowInInspector, ReadOnly] private State _stateCheck;
        [ShowInInspector, ReadOnly] private Field.TilePoint _firstPointPath;
        [ShowInInspector, ReadOnly] private Field.TilePoint _currentPointPath;
        private List<TriggerCheckMover> _marks = new List<TriggerCheckMover>();

        private void Start()
        {
            StartMove += OnStartMoveHandler;
            Finish += OnFinishHandler;
        }

        private void OnEnable()
        {
            FieldMover.Moving += OnMove;
            _firstPointPath = _currentPointPath = null;
            _stateCheck = State.NoneRecord;
        }

        private void OnDisable()
        {
            FieldMover.Moving -= OnMove;
            FieldMover.ReachPoint -= OnReachPoint;
            _stateCheck = State.NoneRecord;
            StopCheck.Invoke();
            ClearMarks();
        }

        private void OnFinishHandler((Field.TilePoint startPoint, Field.TilePoint finishPoint) obj)
        {
            ClearMarks();
            MovingTo -= OnMoveHandler;
        }

        private void OnStartMoveHandler(Tile obj)
        {
            ClearMarks();
            //SpawnMark(obj.transform.position);
            CorutineGame.Instance.WaitFrame(1, () => MovingTo += OnMoveHandler);
        }

        private void OnDamage(GameObject obj)
        {
            ClearMarks();
            MovingTo -= OnMoveHandler;
            _firstPointPath = _currentPointPath = null;
            _stateCheck = State.NoneRecord;
            FieldMover.ReachPoint -= OnReachPoint;
            Broken?.Invoke(obj);
        }

        private void ClearMarks()
        {
            _marks.ForEach(x => Destroy(x.gameObject));
            _marks.Clear();
        }

        // Реакция на эвент движения у меня, спавн новой марки
        private void OnMoveHandler(Tile obj)
        {
            //SpawnMark(obj.transform.position);
        }

        // Логика движения
        private void OnMove((Tile from, Tile to) eventData)
        {
            var mark = eventData.to.Content.GetComponent<CheckMoveMark>();
            bool hasBorder = true;
            if (mark) hasBorder = mark.IsBorder;
            
            switch (_stateCheck)
            {
                case State.NoneRecord:
                {
                    if (hasBorder) // nothing
                    {

                    }
                    else // start move
                    {
                        _stateCheck = State.Record;
                        _firstPointPath = _currentPointPath = new Field.TilePoint(null, eventData.to);
                        StartMove.Invoke(eventData.to);
                        MovingTo.Invoke(eventData.to);
                        FieldMover.ReachPoint += OnReachPoint;
                    }
                }
                    break;
                case State.Record:
                {
                    if (hasBorder) // stop move
                    {
                        _stateCheck = State.NoneRecord;
                        _currentPointPath.InitPrevMe();
                        Finish.Invoke((_firstPointPath, _currentPointPath));
                        FieldMover.ReachPoint -= OnReachPoint;
                    }
                    else // continui move
                    {
                        MovingTo.Invoke(eventData.to);
                        //MakeNextPathPoint(eventData.to);
                    }
                }
                    break;
            }
        }

        private void MakeNextPathPoint(Tile tile)
        {
            var newPointPath = new Field.TilePoint(_currentPointPath, tile);
            _currentPointPath = newPointPath;
        }

        // Спавн марке
        private void SpawnMark(Vector3 transformPosition)
        {
            var m = Instantiate(Trigger, transformPosition, Quaternion.identity);
            m.Damaged += OnDamage;
            _marks.Add(m);
        }

        // Вызов эвента при достижении точки
        private void OnReachPoint(Tile obj)
        {
            ReachTile.Invoke(obj);
            MakeNextPathPoint(obj);
            SpawnMark(obj.transform.position);
        }

        public enum State
        {
            NoneRecord, Record
        }
    }
}