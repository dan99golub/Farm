using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Dirty
{
    public abstract class BaseGreenUpdateTex : MonoBehaviour
    {
        public InitedTile InitedTile;
        
        private GreenManager GreenManager => Services<GreenManager>.S.Get();
        
        private void Awake()
        {
            InitedTile.Event.DynamicCalls += x=>
            {
                GreenManager.Add(this);
                OnInit(x);
            };
        }

        protected abstract void OnInit(Tile t);
        
        [Button]
        public abstract void UpdateTex();
        
        private void OnDestroy() => GreenManager.Remove(this);
    }
}