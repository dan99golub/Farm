using UnityEngine;

namespace DefaultNamespace.Game
{
    public class IntValueContainer : ValueContainer<int>
    {
        protected override int DefaultValid(int current, int min, int max) => Mathf.Clamp(current, min, max);
    }
}