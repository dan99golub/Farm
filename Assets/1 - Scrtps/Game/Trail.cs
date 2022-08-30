using System;
using System.Collections.Generic;
using System.Linq;
using LoadScenes;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class Trail : MonoBehaviour
    {
        public CheckMoveNotOnBorder Checker;
        public Fence PrefabPoint;

        private List<Fence> _spawnedPoint;

        private Field _fieldFence;
        private Field FieldFence => _fieldFence ??= ServicesID<Field>.S.Get(GameSceneLoad.FieldFenceId);

        private void Start()
        {
            Checker.StartMove += OnStartMove;
            Checker.Broken -= OnBroke;
            Checker.Broken += OnBroke;
            Checker.StopCheck += ClearTrailPart;
        }

        private void OnBroke(GameObject obj)
        {
            ClearTrailPart();
        }

        private void OnStartMove(Tile obj)
        {
            ClearTrailPart();
            Checker.StartMove -= OnStartMove;
            Checker.ReachTile += OnReachTile;
            Checker.Finish += OnFinis;
        }

        private void OnFinis((Field.TilePoint startPoint, Field.TilePoint finishPoint) obj)
        {
            Checker.ReachTile -= OnReachTile;
            Checker.Finish -= OnFinis;
            ClearTrailPart();
            CorutineGame.Instance.WaitFrame(1, Start);
        }

        private void ClearTrailPart()
        {
            if(_spawnedPoint==null) _spawnedPoint = new List<Fence>();
            _spawnedPoint.ForEach(x=>
            {
                if(x) Destroy(x.TileMediator.gameObject);
            });
            _spawnedPoint.Clear();
        }
        
        private void OnReachTile(Tile obj)
        {
            var newFence = Instantiate(PrefabPoint, transform.position, Quaternion.identity);
            var targetTile = FieldFence.GetTile(Tile.GetGuid(obj.Position));
            targetTile.ReplaceContent(newFence.TileMediator);
            _spawnedPoint.Add(newFence);
            newFence.UpdateFork();
            if(_spawnedPoint.Count()-2>=0)
                _spawnedPoint[_spawnedPoint.Count()-2].UpdateFork();
        }
    }
}