using System;
using UnityEditor;
using UnityEngine;

    public static class ExtHandles
    {
        public static Action SwapColor(Color newColor)
        {
            var baseColor = Handles.color;
            Handles.color = newColor;
            return () => Handles.color = baseColor;
        }
        
        public static void Button3D(Vector3 pos, Handles.CapFunction cap, Color color, Action callback)
        {
            var b = ExtHandles.SwapColor(color);
            if (Handles.Button(pos, Quaternion.identity, 1, 1, cap)) callback?.Invoke();
            b();
        }
        
        public static void Button3D(Vector3 pos, Handles.CapFunction cap, Color color, float size, Action callback)
        {
            var b = ExtHandles.SwapColor(color);
            if (Handles.Button(pos, Quaternion.identity, size, size, cap)) callback?.Invoke();
            b();
        }
    }
