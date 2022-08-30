using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FloatValueContainer : ValueContainer<float>
    {
        protected override float DefaultValid(float current, float min, float max) => Mathf.Clamp(current, min, max);
    }
}