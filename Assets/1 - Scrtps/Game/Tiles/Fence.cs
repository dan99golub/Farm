using System;
using System.Collections.Generic;
using System.Linq;
using Menu;
using Microsoft.Win32.SafeHandles;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Game
{
    public class Fence : SerializedMonoBehaviour
    {
        public int Id;
        public bool IsSpawn;
        public Transform PointSpawn;
        public InitedTile TileMediator;

        public Dictionary<Cross, GameObject> Prefabs;
        
        [SerializeField] private (Cross typeObj, GameObject obj) LastSet;

        private FenceManager Manager => Services<FenceManager>.S.Get();

        public event Action<Cross, Direction> Updated;
        
        private void Start() => Manager.Add(this);

        public void UpdateFork()
        {
            if (!TileMediator.Tile) return;
            var neighr = TileMediator.Tile.Field.GetNeighbours(TileMediator.Tile).Where(x =>
            {
                if (!x.Content) return false;
                var fence = x.Content.GetComponent<Fence>();
                
                if(fence)
                    if (fence.Id == Id)
                        return true;
                return false;
            });
            
            ChangeRoadPrefab(neighr);
        }

        private void ChangeRoadPrefab(IEnumerable<Tile> neighr)
        {
            //Алгоритм по смене дорог 
            switch (neighr.Count())
            {
                case 0:
                    GetVectorRot(Direction.Forward, Cross.Zero);
                    TryChange(Cross.Zero); // нет соседей
                    break;
                // один сосед
                case 1:
                {
                    TryChange(Cross.Line, 
                        r => r.transform.eulerAngles = neighr.First().Position.x == TileMediator.Tile.Position.x ? GetVectorRot(Direction.Forward, Cross.Line) :  GetVectorRot(Direction.Right, Cross.Line));
                }
                    break;
                // Два соседа
                case 2:
                {
                    List<Vector2Int> pos = new List<Vector2Int>();
                    neighr.ForEach(x => pos.Add(x.Position));

                    var basePos = TileMediator.Tile.Position;
                    // прямая дорога
                    if (pos.Contains(basePos + Vector2Int.up) && pos.Contains(basePos + Vector2Int.down))
                    {
                        GetVectorRot(Direction.Forward, Cross.Line);
                        TryChange(Cross.Line);
                    }
                    else if (pos.Contains(basePos + Vector2Int.right) && pos.Contains(basePos + Vector2Int.left))
                    {
                        TryChange(Cross.Line, t => t.transform.eulerAngles = GetVectorRot(Direction.Right, Cross.Line));
                    }
                    else // угол
                    {
                        if (pos.Contains(basePos + Vector2Int.right))
                        {
                            if (pos.Contains(basePos + Vector2Int.up)) TryChange(Cross.Coner, t => t.transform.eulerAngles = GetVectorRot(Direction.Left, Cross.Coner));
                            else
                            {
                                GetVectorRot(Direction.Forward, Cross.Coner);
                                TryChange(Cross.Coner);
                            }
                        }
                        else // left
                        {
                            if (pos.Contains(basePos + Vector2Int.up)) TryChange(Cross.Coner, t => t.transform.eulerAngles = GetVectorRot(Direction.Down, Cross.Coner));
                            else TryChange(Cross.Coner, t => t.transform.eulerAngles =GetVectorRot(Direction.Right, Cross.Coner));
                        }
                    }
                }
                    break;
                case 3: // Т образный перекросток
                {
                    List<Vector2Int> pos = new List<Vector2Int>();
                    neighr.ForEach(x => pos.Add(x.Position));
                    if (!pos.Contains(TileMediator.Tile.Position + Vector2Int.up))
                    {
                        GetVectorRot(Direction.Forward, Cross.T);
                        TryChange(Cross.T);
                    }
                    else if (!pos.Contains(TileMediator.Tile.Position + Vector2Int.right)) TryChange(Cross.T, t => t.transform.eulerAngles = GetVectorRot(Direction.Right, Cross.T));
                    else if (!pos.Contains(TileMediator.Tile.Position + Vector2Int.down)) TryChange(Cross.T, t => t.transform.eulerAngles = GetVectorRot(Direction.Down, Cross.T));
                    else if (!pos.Contains(TileMediator.Tile.Position + Vector2Int.left)) TryChange(Cross.T, t => t.transform.eulerAngles = GetVectorRot(Direction.Left, Cross.T));
                }
                    break;
                case 4:
                    GetVectorRot(Direction.Forward, Cross.Cross);
                    TryChange(Cross.Cross); // 4 соседа
                    break;
            }
        }

        private void TryChange(Cross newT, Action<GameObject> addAction = null)
        {
            if(LastSet.obj!=null)
                if(newT == LastSet.typeObj) return;
            if(LastSet.obj) Destroy(LastSet.obj);
            if (IsSpawn)
            {
                var newObj = Instantiate(Prefabs[newT], PointSpawn);
                LastSet = (newT, newObj);
                addAction?.Invoke(newObj);
            }
        }
        
        [Button] private void CreateDict()
        {
            Prefabs=new Dictionary<Cross, GameObject>();
            foreach (var e in (Cross[])Enum.GetValues(typeof(Cross)))
            {
                Prefabs.Add(e, gameObject);
                Prefabs[e] = null;
            }
        }

        private Vector3 GetVectorRot(Direction dir, Cross type, bool evented = true)
        {
            Vector3 rot = Vector3.zero;
            switch (dir)
            {
                case Direction.Forward: rot = Vector3.zero;
                    break;
                case Direction.Right: rot = new Vector3(0,90,0);
                    break;
                case Direction.Down: rot = new Vector3(0,180,0);
                    break;
                case Direction.Left: rot = new Vector3(0,270,0);
                    break;
                case Direction.Any: rot = Vector3.zero;
                    break;
            }
            if(evented) Updated?.Invoke(type, dir);
            return rot;
        }
        
        private void OnDestroy()
        {
            Manager.Remove(this);
        }

        public enum Cross { Zero, Line, Coner, T, Cross }

        public enum Direction {Forward, Right, Down, Left, Any }
    }
}