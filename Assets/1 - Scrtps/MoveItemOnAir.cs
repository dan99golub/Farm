using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveItemOnAir : MonoBehaviour
    {
        public float AddH;
        public AnimationCurve Curve;

        public void Move(Vector3 startPoint, Vector3 endPoint, Ease ease, float duration, Transform target)
        {
            target.position = startPoint;
            float progress = 0;
            DOTween.To(() => progress, x => progress = x, 1, duration).OnUpdate(() =>
            {
                if(target) target.position = Vector3.Lerp(startPoint, endPoint, progress) + Vector3.up * Curve.Evaluate(progress)* AddH;
            });
        }
    }
}