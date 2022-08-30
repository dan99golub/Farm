using System;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FenceReactor : MonoBehaviour
    {
        [SerializeField] private Fence _fence;
        [SerializeField] private List<React> _reacts;

        private void Awake() => _fence.Updated += OnUpdated;

        private void OnUpdated(Fence.Cross cross, Fence.Direction dir) => _reacts.ForEach(x=>x.TryInvoke(cross, dir));

        [System.Serializable]
        public class React
        {
            public Fence.Cross TargetCross;
            public Fence.Direction TargetDir;
            public UltEvent Event;

            public void TryInvoke(Fence.Cross cr, Fence.Direction dir)
            {
                if (TargetCross == cr)
                {
                    if (CheckDir(dir))
                    {
                        Event.Invoke();
                    }
                }
            }

            private bool CheckDir(Fence.Direction dir)
            {
                if (dir == Fence.Direction.Forward)
                {
                    return dir == Fence.Direction.Forward || dir == Fence.Direction.Any;
                }

                return dir == TargetDir;
            }
        }
    }
}