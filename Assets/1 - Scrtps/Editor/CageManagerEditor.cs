using System;
using System.Collections.Generic;
using ServiceScript;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DefaultNamespace.Game
{
    [CustomEditor(typeof(CageManager))]    
    public class CageManagerEditor : Editor
    {
        private Cage SelectedCage;
        private CageManager _target;
        private Field _field;
        private Action UpdateLabel;

        private void OnEnable()
        {
            _target = (CageManager) target;
            _target.OnValidate();
            _field = _target.Field;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var IMGUI = new IMGUIContainer();
            IMGUI.onGUIHandler = () => DrawDefaultInspector();
            root.Add(IMGUI);

            var labelSelectCage = new Label("selectedCage");
            root.Add(labelSelectCage);
            UpdateLabel = () => labelSelectCage.text = SelectedCage != null ? SelectedCage.gameObject.name : "none"; 
                
            root.Add(CreateButton("Zero cage", () =>
            {
                SelectedCage = null;
                UpdateLabel();
            }));
            root.Add(CreateButton("Remove link Tile", () =>
            {
                if(SelectedCage) (EditID.GetValue(SelectedCage, CageManager.IdLinkTilesOfCage, EditID.Data.Field) as List<Tile>).Clear();
                SetDirty();
            }));
            root.Add(CreateButton("Find All cages", () =>
            {
                var list = (EditID.GetValue(_target, CageManager.IdListCage, EditID.Data.Field) as List<Cage>);
                list.Clear();
                _target.GetComponentsInChildren<Cage>().ForEach(x => list.Add(x));
                SetDirty();
            } ));

            UpdateLabel();
            return root;
        }

        private void OnSceneGUI()
        {
            if (SelectedCage == null)
            {
                _target.Cages.ForEach(c =>
                {
                    Handles.Label(c.transform.position+Vector3.up*4.5f, $"Cage: {c.gameObject.name}");
                    ExtHandles.Button3D(PositionViewCage(c), Handles.SphereHandleCap, new Color(1f, 0f, 0f, 0.5f), 1, ()=>
                    {
                        SelectedCage = c;
                        UpdateLabel();
                    });
                });
            }
            else
            {
                _field.Tiles.ForEach(t =>
                {
                    if (Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, t.transform.position) > 18) return;
                    var b = ExtHandles.SwapColor(new Color(0f, 1f, 0f, 0.85f));
                    if(SelectedCage.HasTile(t)) Handles.DrawLine(t.transform.position, PositionViewCage(SelectedCage));
                    b();
                    ExtHandles.Button3D(t.transform.position, Handles.SphereHandleCap, new Color(0.23f, 0.98f, 1f, 0.4f), 0.2f, () =>
                    {
                        
                        var list = (EditID.GetValue(SelectedCage, CageManager.IdLinkTilesOfCage, EditID.Data.Field) as List<Tile>);
                        if (SelectedCage.HasTile(t)) list.Remove(t);
                        else list.Add(t);
                        SetDirty();
                    });

                });
                ExtHandles.Button3D(PositionViewCage(SelectedCage), Handles.SphereHandleCap, new Color(1f, 0f, 0f, 1f), 1, () =>
                {
                    SelectedCage = null;
                    UpdateLabel();
                });
            }

            Vector3 PositionViewCage(Cage cage)
            {
                return cage.transform.position + Vector3.up * 3;
            }
        }

        private void SetDirty() => EditorUtility.SetDirty(_target);

        private Button CreateButton(string name, Action callback)
        {
            var b = new Button();
            b.text = name;
            b.clicked += callback;
            return b;
        }
    }
}