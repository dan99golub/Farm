using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Menu;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Path))]
    public class PathEditor : UnityEditor.Editor
    {
        public VisualTreeAsset TreeAssetForInspector;
        
        private Path _target;
        private Path.Node _selectNode;
        private static bool ShowSceneGUI = true;
        private static bool ShowHandler = true;
        
        private VisualElement _sceneElement;
        private VisualElement _inspectorTreePoinTree;
        private VisualElement _rootOfRootPoint;

        private void OnEnable()
        {
            _target = (Path) target;
            _selectNode = _target.Points.Count>0 ?  _target.Points[0] : null;
        }

        public override VisualElement CreateInspectorGUI()
        {
            _rootOfRootPoint = new VisualElement();
            
            CreateViewPoint(_selectNode);
            
            var IMGUI = new IMGUIContainer();
            IMGUI.onGUIHandler = ()=>
            {
                if (GUILayout.Button("On\\Off Scene")) ShowSceneGUI = !ShowSceneGUI;
                if (GUILayout.Button("On\\Off Handler")) ShowHandler = !ShowHandler;
                if (GUILayout.Button("Add"))
                {
                    AddPoint();
                    EditorUtility.SetDirty(_target);
                }
                if (GUILayout.Button("Clear"))
                {
                    _target.Points=new List<Path.Node>();
                    EditorUtility.SetDirty(_target);
                }
                DrawDefaultInspector();
            };
            _rootOfRootPoint.Add(IMGUI);
            
            return _rootOfRootPoint;
        }

        private void CreateViewPoint()
        {
            _inspectorTreePoinTree.RemoveFromHierarchy();
            CreateViewPoint(_selectNode);
            _inspectorTreePoinTree.parent.hierarchy.Insert(0,_inspectorTreePoinTree);
        }

        private void CreateViewPoint(Path.Node selectNode)
        {
            _inspectorTreePoinTree = TreeAssetForInspector.Instantiate();
            bool isNull = selectNode == null;

            _inspectorTreePoinTree.Q<Label>("PointName").text = isNull ? "None" : selectNode.Name;
            _inspectorTreePoinTree.Q<Vector3Field>("Position").value = isNull ? Vector3.zero : selectNode.Position;
            if(!isNull) _inspectorTreePoinTree.Q<Vector3Field>("Position").RegisterCallback<ChangeEvent<Vector3>>(x=>
            {
                selectNode.Position = x.newValue;
                EditorUtility.SetDirty(_target);
            });
            _inspectorTreePoinTree.Q<Button>("AbsPosition").clickable.clicked += () =>
            {
                var pos = selectNode.Position;
                pos.x = Mathf.Floor(pos.x);
                pos.y = Mathf.Floor(pos.y);
                pos.z = Mathf.Floor(pos.z);
                CreateViewPoint();
            };
                        
            _rootOfRootPoint.Add(_inspectorTreePoinTree);
        }

        private void OnSceneGUI()
        {
            if(!ShowSceneGUI) return;
            
            int i = 0;
            _target.Points.ForEach(x =>
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Vector3.zero;
                if(ShowHandler)
                    newPos=Handles.PositionHandle(x.Position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    x.Position = newPos;
                    EditorUtility.SetDirty(_target);
                }
                
                var catchX = x;

                if(ShowHandler)
                    ExtHandles.Button3D(x.Position+Vector3.up*2, Handles.SphereHandleCap, new Color(0.54f, 0.98f, 1f, 0.36f), () =>
                    {
                        _selectNode = catchX;
                        CreateViewPoint();
                    });

                var b = ExtHandles.SwapColor(Color.red);
                Handles.Label(x.Position+Vector3.up, x.Name);
                b();
                
                if(x!=_selectNode && _selectNode!=null && ShowHandler)
                {
                    var isNeighbours = _selectNode.IsNeighbours(x);
                    var color = isNeighbours ? Color.green : Color.red;
                    ExtHandles.Button3D(x.Position+Vector3.up + Vector3.forward*-1*2, Handles.SphereHandleCap, color, () =>
                    {
                        if(isNeighbours) _selectNode.RemovePoint(x); else _selectNode.AddPoint(x);
                        EditorUtility.SetDirty(_target);    
                    });
                    
                }
                
                foreach (var neighbour in x.Neighbours)
                {
                    var t = GetOffsetBy(i);
                    var swapBack = ExtHandles.SwapColor(t.Item2);
                    
                    Handles.DrawLine(x.Position+t.Item1, neighbour.Position+t.Item1);
                    swapBack();
                }
                
                i++;
                
            });
        }

        private (Vector3, Color) GetOffsetBy(int i)
        {
            switch (i%3)
            {
                case 0:
                    return (Vector3.zero, Color.yellow);
                case 1:
                    return (Vector3.up*0.2f, new Color(1f, 0.56f, 0.05f));
                case 2:
                    return (Vector3.up*0.4f, new Color(1f, 0.19f, 0.11f));
                default:
                    return (Vector3.up*0.4f, new Color(1f, 0.19f, 0.11f)); 
            }
        }

        private void AddPoint()
        {
            var newPoint = new Path.Node(_target, _selectNode!=null ? _selectNode.Position+Vector3.forward : _target.transform.position+Vector3.forward);
            if(_selectNode!=null) newPoint.AddPoint(_selectNode);
            else if(_target.Points.Count>0) newPoint.AddPoint(_target.Points[_target.Points.Count-1]);
            _target.Points.Add(newPoint);
        }
    }
