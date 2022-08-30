using UnityEngine;

namespace DefaultNamespace.SM
{
    public static class TweenExt
    {
        public static float CalculateDuration(this Vector3 start, Vector3 endPoint, float speed) => Vector3.Distance(start, endPoint) / speed;
    }
}