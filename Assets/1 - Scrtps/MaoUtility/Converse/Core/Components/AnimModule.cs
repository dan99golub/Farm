using System.Collections.Generic;
using System.Linq;
using Lean.Transition;
using MaoUtility.Converse.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MaoUtility.Converse.Core.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimModule : BaseConverseComponent
    {
        public override string PrefixAlias => "Anim";

        public List<Data> Anims;

        public UnityEvent<Data> Started;
        public UnityEvent<Data> Stoped;

        private void Awake()
        {
            Anims.ForEach(x =>
            {
                var xv = x;
                x.StartEvent.AddListener(() => Started.Invoke(xv));
                x.StopEvent.AddListener(() => Stoped.Invoke(xv));
            });
        }

        [Button]
        public void PlayByStr(string name)
        {
            var r = Anims.FirstOrDefault(x => x.Name == name);
            if (r != null) r.Start();
        }
        
        [Button]
        public void StopByStr(string name)
        {
            var r = Anims.FirstOrDefault(x => x.Name == name);
            if (r != null) r.Stop();
        }
        
        public void PlayByObj(Data d)
        {
            if(Anims.Contains(d)) d.Start();
        }

        public void StopByObj(Data d)
        {
            if(Anims.Contains(d)) d.Stop();
        }

        public Data GetAnim(string name) => Anims.FirstOrDefault(x => x.Name == name);

        [System.Serializable]
        public class Data
        {
            public string Name;

            public LeanManualAnimation LeanAnim;
            public UnityEvent StartEvent;
            public UnityEvent StopEvent;

            private bool _isPlay = false;

            public void Start()
            {
                _isPlay = true;
                StartEvent.Invoke();
                LeanAnim.BeginTransitions();
            }

            public void Stop()
            {
                if (_isPlay)
                {
                    _isPlay = false;
                    StopEvent.Invoke();
                    LeanAnim.StopTransitions();
                }
            }
        }
    }
}