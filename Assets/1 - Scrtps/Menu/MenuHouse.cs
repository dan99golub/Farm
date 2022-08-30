using System;
using DefaultNamespace;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace Menu
{
    public class MenuHouse : MonoBehaviour
    {
        public string BuildIdAction;
        public string InstanceBuildIdAction;
        public string HideIdAction;

        public PanelUI House;

        public AnimationClip[] Clips;
        public UltEvent<AnimationClip> SelectedClip;
        private AnimationClip _selectClip;
        

        private void Awake()
        {
            SelectedClip.Invoke(_selectClip = Clips.GetRandom());
        }

        public void Build() => House.TryCustomAction(BuildIdAction);
        
        public void InstanceBuild() => House.TryCustomAction(InstanceBuildIdAction);

        public void Hide() => House.TryCustomAction(HideIdAction);
    }
}