using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceScript;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Menu
{
    public class Path : MonoBehaviour
    {
        public List<Node> Points=new List<Node>();

        public List<Node> GetPoints(Func<Node, bool> predictr) => Points.Where(x => predictr(x)).ToList();

        public Node GetPoint(Func<Node, bool> predictr) => Points.FirstOrDefault(x=>predictr(x));

        public Node GetPoint(string guid) => GetPoint(x => x.GUID == guid);

        [ContextMenu("Test")]
        public void Test()
        {
            var first = Points[Random.Range(0, Points.Count)];
            var Second = Points[Random.Range(0, Points.Count)];
            
            var path = MakePath(first, Second);
            var point = path.startPoint;
            string resultPath = "";
            while (point!=null)
            {
                resultPath += point.target.Name + " - ";
                point = point.Next;
            }
            
            Debug.Log($"Start: {first.Name}; End: {Second.Name}; Path: {resultPath}");
        }
        
        public (PathPoint startPoint, bool canBeBuilded) MakePath(Node Start, Node Finish)
        {
            bool isOneBoard = Points.Contains(Start) && Points.Contains(Finish);
            
            if(Start==Finish || !isOneBoard) return (new PathPoint(Start), true && isOneBoard);

            var firstPoint = new PathPoint(Start);
            
            Queue<PathPoint> unpassed = new Queue<PathPoint>();
            firstPoint.GetNeighbours().ForEach(x=>
            {
                var newPoint = new PathPoint(x);
                newPoint.SetPrev(firstPoint);
                unpassed.Enqueue(newPoint);
            });

            while (unpassed.Count>0)
            {
                var nextPoint = unpassed.Dequeue();
                
                nextPoint.GetNeighbours().ForEach(x=>
                {
                    var newPoint = new PathPoint(x);
                    newPoint.SetPrev(nextPoint);
                    unpassed.Enqueue(newPoint);
                });

                if (nextPoint.target == Finish)
                {
                    var lastPoint = nextPoint;
                    while (lastPoint.Prev!=null)
                    {
                        lastPoint.Prev.SetNext(lastPoint);
                        lastPoint = lastPoint.Prev;
                    }
                    firstPoint = lastPoint;
                    break;
                }
            }
            
            return (firstPoint, true);
        }
        
        public class PathPoint
        {
            public PathPoint Prev;
            public Node target;
            public PathPoint Next;

            public PathPoint(Node node) => target = node;

            public List<Node> GetNeighbours()
            {
                var result = target.Neighbours.ToList();
                if (Prev != null) result.Remove(Prev.target);
                return result;
            }
            
            public void SetNext(PathPoint point)
            {
                Next = point;
            }

            public void SetPrev(PathPoint point)
            {
                Prev = point;
            }
        }
        
        [System.Serializable]
        public class Node
        {
            public string GUID => _GUID;
            public string Name;
            
            public Vector3 Position
            {
                get => _path.transform.position + _pos;
                set => _pos = value-_path.transform.position;
            }

            [SerializeField] private Vector3 _pos;
            [SerializeField, HideInInspector] private string _GUID;
            [SerializeField, HideInInspector] private Path _path;
            [SerializeField, EditID("Content")] private List<Object> _content = new List<Object>();
            [SerializeField, HideInInspector] private List<string> _neighbours = new List<string>();

            public ReadOnlyCollection<Object> Content => _content.AsReadOnly();
            public ReadOnlyCollection<Node> Neighbours => _path.GetPoints(x => _neighbours.Contains(x.GUID)).AsReadOnly();

            public Node(Path path, Vector3 position)
            {
                _path = path;
                Position = position;
                _GUID = Guid.NewGuid().ToString();
                Name = $"{Random.Range(0, 10).ToString()}{Random.Range(0, 10).ToString()}{Random.Range(0, 10).ToString()}{Random.Range(0, 10).ToString()}{Random.Range(0, 10).ToString()}{Random.Range(0, 10).ToString()}";
            }

            public void AddContent(Object obj)
            {
                if(!_content.Contains(obj)) _content.Add(obj);
            }

            public void RemoveContent(Object obj) => _content.Remove(obj);

            public bool IsNeighbours(Node otherNode) => _neighbours.Contains(otherNode.GUID);
            
            public void AddPoint(Node obj)
            {
                if (!_neighbours.Contains(obj.GUID)) _neighbours.Add(obj.GUID);
            }

            public void RemovePoint(Node obj) => _neighbours.Remove(obj.GUID);

            public void RemoveNull()
            {
                _content = _content.Where(x => x != null).ToList();
                _neighbours = _neighbours.Where(x => x != null).ToList();
            }
        }
    }
}