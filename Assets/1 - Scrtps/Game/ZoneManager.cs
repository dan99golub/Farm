using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class ZoneManager : MonoBehaviour
    {
        [SerializeField, ShowIf("CanShow")] private List<Zone> _zones = new List<Zone>();
        private Field _filed;

        private bool CanShow => Application.isPlaying;

        public event Action<List<Zone>> NewZones;

        public Zone GetZoneByTile(Tile t) => _zones.FirstOrDefault(z => z.HasTile(t));
        
        public void Init(Field field)
        {
            _filed = field;
            _zones = CreateZones(_filed.Tiles);
        }

        public bool UpdateByTile(Tile t)
        {
            var zone = _zones.FirstOrDefault(x => x.HasTile(t));
            if (zone == null) return false;
            _zones.Remove(zone);
            var newZones = CreateZones(zone.Tiles);
            _zones.AddRange(newZones);
            NewZones?.Invoke(newZones);
            return true;
        }

        [Button] public void ReInit()
        {
            _zones = CreateZones(_filed.Tiles);
        }
        
        private List<Zone> CreateZones(IEnumerable<Tile> tiles)
        {
            tiles = tiles.Where(IsCorrectTile);

            List<Zone> result = new List<Zone>();
            while (tiles.Count()>0)
            {
                var point = tiles.First();
                var points = GetAllNeighbours(point);
                tiles = tiles.Except(points);
                result.Add(new Zone(points));
            }
            return result;
        }

        private IEnumerable<Tile> GetAllNeighbours(Tile point)
        {
            HashSet<Tile> passed = new HashSet<Tile>();
            Queue<Tile> noPassed = new Queue<Tile>();
            passed.Add(point);
            AddNeighbours(point);

            while (noPassed.Count > 0)
            {
                var p = noPassed.Dequeue();
                if (passed.Contains(p)) continue;
                if (IsCorrectTile(p) == false) continue;
                passed.Add(p);
                AddNeighbours(p);
            }
            
            return passed;

            void AddNeighbours(Tile tilePoint)
            {
                _filed.GetNeighbours(tilePoint).ForEach(x =>
                {
                    if(IsCorrectTile(x)) noPassed.Enqueue(x);
                });
            }
        }

        public bool IsCorrectTile(Tile t)
        {
            if (t.Content == null) return false;
            return IsWallkabelTile(t);
        }
        
        private bool IsWallkabelTile(Tile t) => t.Content.GetComponent<BorderMark>()==null;

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying == false || _zones == null) return;
            int i = 0;
            _zones.ForEach(x =>
            {
                Gizmos.color = GetColor(i);
                x.Tiles.ForEach(t =>
                {
                    Gizmos.DrawCube(t.transform.position, Vector3.one * 0.8f);
                });
                i++;
            });

            Color GetColor(int i)
            {
                var o = i % 3;
                if(o==0) return new Color(0f, 1f, 0f, 0.4f);
                else if(o==1) return new Color(1f, 0.96f, 0.97f, 0.4f);
                else return new Color(1f, 0.18f, 0.93f, 0.4f);
            }
        }

        [System.Serializable]
        public class Zone
        {
            [ShowInInspector] public int CountTiles => _countTile ??= Tiles.Count;
            private int? _countTile;
            
            [ShowInInspector, ReadOnly]public ReadOnlyCollection<Tile> Tiles => _tiles.Values.ToList().AsReadOnly();
            private Dictionary<string, Tile> _tiles;
            private Tile _minPoint;
            private Tile _maxPoint;

            public Zone(IEnumerable<Tile> t)
            {
                _tiles = new Dictionary<string, Tile>();
                var noDoubleT = t.Distinct();
                noDoubleT.ForEach(x => _tiles.Add(x.GUID, x));
                if(noDoubleT.Count()==0) return;
                var orderedElements = noDoubleT.OrderBy(x => x.Position.x + x.Position.y);
                _minPoint = orderedElements.First();
                _maxPoint = orderedElements.Last();
                
            }

            public bool HasTile(Tile t) => _tiles.ContainsKey(t.GUID);

            public bool HasTile(Vector2Int pos) => _tiles.ContainsKey(Tile.GetGuid(pos));

            public Tile GetTile(Vector2Int pos) => HasTile(pos) ? _tiles[Tile.GetGuid(pos)] : null;

            public IEnumerable<Collider> Overlap(LayerMask mask)
            {
                IEnumerable<Collider> result = new Collider[0];
                Tiles.ForEach(x => result = result.Union(x.Overlap(mask)));
                return result.Distinct();
            }
        }
    }
}