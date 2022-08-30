using DG.Tweening;
using MaoUtility.Converse.Core.Components;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public class CurveAnim : MonoBehaviour
    {
        public RectTransform StartPoint;
        public RectTransform EndPoint;
        public float Duration;
        public AnimationCurve CurveX;
        public AnimationCurve CurveY;
        public UltEvent End;

        private Tween _tween;
        
        [Button]
        public void Play(RectTransform target)
        {
            _tween?.Kill();
            float progress = 0;
            _tween = DOTween.To(() => progress, x => progress = x, 1, Duration).OnUpdate(() =>
            {
                target.localPosition = new Vector3(
                    Calcaulte(StartPoint.localPosition.x, EndPoint.localPosition.x, progress, CurveX), 
                    Calcaulte(StartPoint.localPosition.y, EndPoint.localPosition.y, progress, CurveY));
            }).OnComplete(End.Invoke);
        }

        private float Calcaulte(float start, float end, float progress, AnimationCurve curve)
        {
            return (end - start) * curve.Evaluate(progress) + start;
        }
    }
}