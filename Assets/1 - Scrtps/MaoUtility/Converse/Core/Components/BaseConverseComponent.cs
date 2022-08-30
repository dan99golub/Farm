using System;
using DefaultNamespace;
using MaoUtility.Converse.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MaoUtility.Converse.Core.Components
{
    public abstract class BaseConverseComponent : SerializedMonoBehaviour, IConverseMarkType
    {
        [SerializeField] private string _alias;
        public string Alias => _alias;

        [ReadOnly] protected IManagerMarkType<IConverseMarkType> _manager;
        public IManagerMarkType<IConverseMarkType> Manager => _manager;

        public Canvas ParentCanvas => _parentCanvas != null ? _parentCanvas : _parentCanvas = (transform as RectTransform).GetParentCanvas();

        private Canvas _parentCanvas;
        
        public void Init(IManagerMarkType<IConverseMarkType> parent)
        {
            _manager = parent;
        }

        [ShowInInspector, ReadOnly, PropertyOrder(-10)]
        public string ID => PrefixAlias + "_" + Alias;
        
        public abstract string PrefixAlias { get; }
    }

    public static class ConverseExt
    {
        public static Canvas GetParentCanvas(this RectTransform trans)
        {
            var parent = trans.parent;
            Canvas result = null;
            while (result==null && parent!=null)
            {
                result = parent.GetComponent<Canvas>();
                parent = parent.parent;
            }
            return result;
        }
    } 
}