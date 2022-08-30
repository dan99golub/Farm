using System;
using System.Linq;
using System.Reflection;
using Menu;
using ServiceScript;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

    [CustomEditor(typeof(LevelDeskEdit))]
    public class LevelDeskEditor : Editor
    {
        private LevelDesk _target;
        private LevelDesk.Data _selectData;

        private static string IdFieldPointId => "Point";

        private void OnEnable()
        {
            _target = ((LevelDeskEdit) target).Desk;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var result = new VisualElement();
            return result;
        }

        public void OnSceneGUI()
        {
            int i = 0;
            _target.Datas.ForEach(x =>
            {
                Handles.Label(x.View.transform.position+Vector3.up*2, $"Order - {i+1}");
                if (x != _selectData) ExtHandles.Button3D(x.View.transform.position + Vector3.up, Handles.SphereHandleCap, new Color(0f, 1f, 0f, 0.46f), () => _selectData = x);
                var point = _target.Path.GetPoint(p => p.GUID == x.PointGUID);
                if (point != null)
                {
                    var b = ExtHandles.SwapColor(Color.yellow);
                    Handles.DrawLine(x.View.transform.position + Vector3.up, point.Position);
                    b();
                }
                i++;
            });
            if(_selectData!=null)
            {
                _target.Path.Points.ForEach(x =>
                {
                    ExtHandles.Button3D(x.Position, Handles.DotHandleCap, new Color(0.28f, 0.9f, 1f, 0.6f), 0.2f, () =>
                    {
                        var field = _selectData.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(f =>
                        {
                            if (f.GetCustomAttribute<EditID>() != null)
                                if (f.GetCustomAttribute<EditID>().ID == IdFieldPointId)
                                    return true;
                            return false;
                        });

                        var prevPoint = _target.Path.GetPoint(x => x.GUID == _selectData.PointGUID);
                        prevPoint?.RemoveContent(_selectData.View);
                        field.SetValue(_selectData, x.GUID);
                        x.AddContent(_selectData.View);
                        SetDirtyTarget();
                    });
                });
            }
        }

        private void SetDirtyTarget() => EditorUtility.SetDirty(_target);
    }


