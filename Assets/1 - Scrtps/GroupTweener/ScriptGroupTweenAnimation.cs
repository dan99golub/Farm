using System;
using UnityEngine;

namespace GroupTweener
{
    public class ScriptGroupTweenAnimation : GroupTweenAnimation
    {
        private Action<GameObject> _play;
        private Action<GameObject> _prepare;
        private float _duration;

        public void Init(float duration, Action<GameObject> prepareAct, Action<GameObject> play)
        {
            _duration = duration;
            _prepare = prepareAct;
            _play = play;
        }

        public override float Duration => _duration;
        public override void Prepare(GameObject target) => _prepare.Invoke(target);

        public override void Play(GameObject target) => _play.Invoke(target);
    }
}