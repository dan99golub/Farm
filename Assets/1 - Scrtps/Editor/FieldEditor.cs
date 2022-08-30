using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DefaultNamespace.Game
{
    [CustomEditor(typeof(FieldEdt))]
    public class FieldEditor : Editor
    {
        private Field _target;

        private static int WKisty;
        private static int HKisty;
        private static ActionWithTile Action;
        private VisualElement ActionTree;
        private static InitedTile TileBrush;

        private void OnEnable()
        {
            _target = ((FieldEdt) target).Field;
        }

        private void OnSceneGUI()
        {
            _target.Tiles.ForEach(x =>
            {
                var cathcX = x;
                if (Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, x.transform.position) > 18) return;
                var b = ExtHandles.SwapColor(new Color(0.26f, 1f, 0.99f, 0.13f));
                if (Handles.Button(cathcX.transform.position+Vector3.up/2, Quaternion.identity, 0.3f, 0.5f, Handles.SphereHandleCap)) 
                    Action.Activated(cathcX, WKisty, HKisty, _target);
                b();
            });
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var sliderX = new SliderInt(0, 15);
            var labelX = new Label();
            labelX.text = "X";
            var sliderY = new SliderInt(0, 15);
            var labelY = new Label();
            labelY.text = "Y";
            sliderX.RegisterCallback<ChangeEvent<int>>(x=>
            {
                WKisty = x.newValue;
                labelX.text = $"X-{x.newValue.ToString()}";
            });
            sliderY.RegisterCallback<ChangeEvent<int>>(x=>
            {
                HKisty = x.newValue;
                labelY.text = $"Y-{x.newValue.ToString()}";
            });
            root.Add(labelX);
            root.Add(sliderX);
            root.Add(labelY);
            root.Add(sliderY);
            root.Add(CreateButton("Null action", () =>
            {
                Action = null;
                UpdateViewAction();
            }));
            root.Add(CreateButton("Clear tile action", ()=>CreatAction("Zero tile", t =>
            {
                t.GetComponentsInChildren<Transform>().Except(new[] {t.transform}).Where(x => x.parent == t.transform).ForEach(x =>
                {
                    if(PrefabUtility.IsPartOfAnyPrefab(x.gameObject))
                        PrefabUtility.UnpackPrefabInstance(x.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                    DestroyImmediate(x.gameObject);
                });
            })));
            root.Add(CreateButton("Aplay to all tiles", () =>
            {
                if (Action != null) _target.Tiles.ForEach(x => Action.Activated(x,0,0, _target));
            }));
            
            ActionTree = CreateViewAction();
            root.Add(ActionTree);
            
            root.Add(CreateBorder());
            
            var brushField = new ObjectField();
            brushField.objectType = typeof(InitedTile);
            brushField.value = TileBrush;
            brushField.RegisterCallback<ChangeEvent<Object>>(x=>TileBrush=x.newValue as InitedTile);
            
            root.Add(brushField);
            root.Add(CreateButton("Draw brush", () =>
            {
                CreatAction("Draw", t =>
                {
                    if (t.Content)
                    {
                        if(PrefabUtility.IsPartOfAnyPrefab(t.Content.gameObject))
                            PrefabUtility.UnpackPrefabInstance(t.Content.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                        DestroyImmediate(t.Content.gameObject); 
                    }
                    if(TileBrush==null) return;
                    var newContent = PrefabUtility.InstantiatePrefab(TileBrush.gameObject, t.transform);
                    t.ReplaceContent((newContent as GameObject).GetComponent<InitedTile>());
                });
            }));
            
            UpdateViewAction();
            return root;
        }
        
        private VisualElement CreateButton(string name, Action callback)
        {
            var r = new Button();
            r.text = name;
            r.clickable.clicked += callback;
            return r;
        }
        
        private void CreatAction(string name, Action<Tile> act)
        {
            Action = new ActionWithTile(name, act);
            Debug.Log(Action);
            UpdateViewAction();
        }

        private VisualElement CreateBorder()
        {
            var e = new VisualElement();
            e.style.height = 8;
            e.style.backgroundColor = new StyleColor(new Color(0.18f, 0.18f, 0.18f));
            return e;
        }

        private void UpdateViewAction() => (ActionTree as Label).text = Action != null ? Action.Name : "None";
        
        private VisualElement CreateViewAction()
        {
            var r = new Label();
            r.style.fontSize = 15;
            r.text = "test text";
            r.style.color = new StyleColor(Color.white);
            return r;
        }

        [System.Serializable]
        public class ActionWithTile
        {
            public string Name { get; private set; }
            public Action<Tile> Action { get; private set; }

            public ActionWithTile(string name, Action<Tile> act)
            {
                Name = name;
                Action = act;
            }

            public void Activated(Tile t, int x, int z, Field f)
            {
                for (int w = t.Position.x-x; w <= t.Position.x+x; w++)
                {
                    for (int h = t.Position.y-z; h <= t.Position.y+z; h++)
                    {
                        var tile = f.GetTile(Tile.GetGuid(new Vector2Int(w, h)));
                        if(tile)
                            Action.Invoke(tile);
                    }
                }
            }
        }
    }
}