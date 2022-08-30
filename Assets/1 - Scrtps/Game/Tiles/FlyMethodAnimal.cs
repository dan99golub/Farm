using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FlyMethodAnimal : MoveMethodAnimal
    {
        [Min(0)]public float Duration;
        public float H;
        public AnimationCurve CurveFly;
        public Transform RootMove;
        public UltEvent Start;
        public UltEvent Finish;
        private Tween _tweem;

        public override void Move(Cage cage, Action action)
        {
            Start.Invoke();
            float progress = 0;
            var startPos = RootMove.position;
            var endPos = cage.PointAnimal.position;
            _tweem = DOTween.To(() => progress, x => progress = x, 1, Duration).OnUpdate(() =>
            {
                RootMove.position = Vector3.Lerp(startPos, endPos, progress) + Vector3.up*H*CurveFly.Evaluate(progress); 
            }).OnComplete(() =>
            {
                action();
                Finish.Invoke();
            });
        }

        private void OnDestroy()
        {
            _tweem.Kill(false);
        }
    }
}