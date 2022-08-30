using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class ProgressLevel : MonoBehaviour
    {
        public event Action<float> NewProgress;

        public float Progress => _progress;
        [ShowInInspector, ReadOnly]private float _progress;
        private Field _field;

        public void Init(Field field)
        {
            _field = field;
            CalculateProgress();
        }

        public void CalculateProgress()
        {
            int complete = 0;
            int incomplete = 0;
            _field.Tiles.ForEach(x =>
            {
                var state = x.Mark.CurrentState;
                if (state == ProgressMark.State.Complete) complete++;
                else if (state == ProgressMark.State.Incomplete) incomplete++;
            });
            _progress = (float)complete / (complete + incomplete);
            NewProgress?.Invoke(_progress);
        }
    }
}