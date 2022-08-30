using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Menu
{
    public class ViewMenuBuild : MonoBehaviour
    {
        public LevelView View;
        private MenuHouse[] Houses;

        private void Awake()
        {
            Houses = GetComponentsInChildren<MenuHouse>();
            View.Closed += Close;
            View.Opened += Open;
            View.PlayAnimOpened += PlayAnim;
            Close();
        }

        [Button]
        private void Close() => Houses.ForEach(x => x.Hide());
        
        [Button]
        private void Open() => Houses.ForEach(x => x.InstanceBuild());
        
        [Button]
        private void PlayAnim() => Houses.ForEach(x => x.Build());
    }
}