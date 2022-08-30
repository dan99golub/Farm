using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game.Plants;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace DefaultNamespace.Game
{
    [RequireComponent(typeof(FieldEdt), typeof(AnimalManager), typeof(CacheField))][RequireComponent(typeof(BoxCollider), typeof(NavMeshSurface), typeof(CageManager))]
    public class Field : SerializedMonoBehaviour
    {
        public Transform PointTiles;
        [SerializeField] private Dictionary<string, Tile> tilesDict = new Dictionary<string, Tile>(); 
        public IReadOnlyCollection<Tile> Tiles => tilesDict.Values.ToList();

        public NavMeshSurface Surface => GetComponent<NavMeshSurface>();
        private Dictionary<string, Tile> TilesDict => tilesDict;
        private BoxCollider Collider => GetComponent<BoxCollider>();
        
        [SerializeField, ReadOnly]private Vector2Int _size;
        public Vector2Int Size => _size;

        [Button] public void Init()
        {
            if (!Application.isPlaying) return;
            tilesDict.Values.ForEach(x => x.Init());
        }

        [Button]public void UpdateSize()
        {
            Dictionary<Tile, int> summa = new Dictionary<Tile, int>();
            tilesDict.ForEach(x => summa.Add(x.Value, x.Value.Position.x + x.Value.Position.y));
            _size = summa.OrderByDescending(x => x.Value).First().Key.Position + Vector2Int.one;
        }

        public Tile GetTile(string guid)
        {
            TilesDict.TryGetValue(guid, out var r);
            return r;
        }

        public List<Tile> GetNeighbours(Tile tile)
        {
            List<Tile> result = new List<Tile>();
            Vector2Int[] guids = new[] 
                {tile.Position + new Vector2Int(0, 1), tile.Position + new Vector2Int(0, -1), tile.Position + new Vector2Int(1, 0), tile.Position + new Vector2Int(-1, 0)};
            guids.ForEach(x =>
            {
                var t = GetTile(Tile.GetGuid(x));
                if (t) result.Add(t);
            });
            return result;
        }

        public bool CanMakePath(Tile first, Tile second, Func<Tile, bool> canUseTile)
        {
            if (canUseTile(first) == false || canUseTile(second) == false) return false;
            
            HashSet<Tile> passedTile = new HashSet<Tile>();
            Queue<Tile> tiles = new Queue<Tile>();
            
            tiles.Enqueue(first);

            while (tiles.Count>0)
            {
                var tileCheck = tiles.Dequeue();
                passedTile.Add(tileCheck);

                if (tileCheck == second) return true;
                GetNeighbours(tileCheck).Except(passedTile).Where(x=>canUseTile(x)).ForEach(x => tiles.Enqueue(x));
            }
            return false;
        }

        public (TilePoint startPoint, bool resultFunc) MakePath(Tile first, Tile second, Func<Tile, bool> canMoveHere)
        {
            bool canMakePath = CanMakePath(first, second, canMoveHere);
            if (first == second || canMakePath==false) return (new TilePoint(null, first), true && canMakePath);
            
            var firstPoint = new TilePoint(null, first);
            Queue<TilePoint> unpassedTile = new Queue<TilePoint>();
            AddNewUnpassedTile(firstPoint, canMoveHere);

            while (unpassedTile.Count>0)
            {
                var nextPoint = unpassedTile.Dequeue();
                if (nextPoint.Content == second)
                {
                    nextPoint.InitPrevMe();
                    break;
                }

                AddNewUnpassedTile(nextPoint, canMoveHere);
            }

            return (firstPoint, true);

            void AddNewUnpassedTile(TilePoint prevPoint, Func<Tile, bool> canUseTile)
            {
                GetNeighbours(prevPoint.Content).ForEach(x =>
                {
                    if(canUseTile(x)) unpassedTile.Enqueue(new TilePoint(prevPoint, x));
                });
            }
        }

        private void OnDrawGizmosSelected()
        {
            TilesDict.Values.ForEach(x=>
            {
                var color = x.Position.x % 2 == 0 ? Color.white : Color.gray;
                if (x.Position.y % 2 == 0)
                {
                    if(color == Color.white) color = Color.gray;
                    else color = Color.white;
                }

                Gizmos.color = color;
                Gizmos.DrawWireSphere(x.transform.position, 0.2f);
            });
        }
        
        [Button] private void TestCanMakePath(Tile first, Tile second)
        {
            if(!first || !second) return;
            Debug.Log($"from {first.GUID} to {second.GUID} path can be place? Answer: {CanMakePath(first, second, x => true)}");
        }

        [Button] private void TestPath(Tile first, Tile second)
        {
            if(!first || !second) return;
            var path = MakePath(first, second, x => true);

            string pathStr = "";
            var point = path.startPoint;
            while (point!=null)
            {
                pathStr += $"{point.Content.GUID} = ";
                point = point.Next;
            }
            
            Debug.Log($"Start: {first.GUID}, Second: {second.GUID}, Path: {pathStr}");
        }

        [Button] public void InitField(Vector2Int size)
        {
            if (!PointTiles)
            {
                Debug.LogWarning("У уровня нет точки для тайлов");
                return;
            }
            GetComponentsInChildren<Tile>().ForEach(x => DestroyImmediate(x.gameObject));
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.y; z++)
                {
                    var pos = new Vector2Int(x,z);
                    var newTile = new GameObject(Tile.GetGuid(pos), new [] {typeof(Tile)});
                    newTile.transform.SetParent(PointTiles);
                    newTile.transform.localPosition = TilePosToLocalFieldUnityPos(new Vector2Int(x, z));
                    newTile.GetComponent<Tile>().InitEditor(pos);
                }
            }
            tilesDict=new Dictionary<string, Tile>();
            GetComponentsInChildren<Tile>().ForEach(x =>
            {
                tilesDict.Add(x.GUID, x);
            });
            InitCollider(size, Collider.size.y, Collider.center.y);
            UpdateSize();
        }

        [Button]
        public void InitCollider(Vector2Int size, float sizeY, float offsetY)
        {
            Collider.size  = new Vector3(size.x, sizeY, size.y);
            Collider.center = new Vector3(size.x/2, offsetY, size.y/2) - new Vector3(1,0,1)/2;
        } 

        [Button] private void FindContent()
        {
            Tiles.ForEach(x =>
            {
                var content = x.GetComponentInChildren<InitedTile>();
                if (content) x.ReplaceContent(content);
            });
        }

        public Vector3 TilePosToLocalFieldUnityPos(Vector2Int pos) => new Vector3(pos.x, 0, pos.y);
        
        public Vector3 TilePosToGlobalUnityPos(Vector2Int pos) => new Vector3(pos.x, 0, pos.y)+transform.position;

        [System.Serializable]
        public class TilePoint
        {
            public TilePoint Prev { get; private set; }
            public Tile Content { get; private set; }
            public TilePoint Next { get; private set; }

            public TilePoint(TilePoint prev, Tile content)
            {
                Content = content;
                Prev = prev;
            }

            public void InitPrevMe()
            {
                if (Prev == null) return;
                Prev.Next = this;
                Prev.InitPrevMe();
            }
        }
    }
}