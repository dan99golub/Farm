using System;
using GroupTweener;
using Sirenix.Utilities;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(LevelView))]
    public class ViewMenuGroupAnim : MonoBehaviour
    {
        public LevelView View;
        private GroupTween[] _groups;

        private void Awake()
        {
            _groups = GetComponentsInChildren<GroupTween>();
            View.PlayAnimOpened += Play;
        }

        private void Play() => _groups.ForEach(x => x.PrepareAndPlay());

        private void OnValidate()
        {
            if(View ==null)
                View = GetComponent<LevelView>();
        }
    }
}