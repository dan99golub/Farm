using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class ChangeMatFence : MonoBehaviour
    {
        [SerializeField] private Fence _fence;
        [SerializeField] private List<MeshRenderer> _rendererss;

        public Material Point;
        
        public Material Line_0;
        public Material Line_90;

        public Material Corner_0;
        public Material Corner_90;
        public Material Corner_180;
        public Material Corner_270;

        public Material T_0;
        public Material T_90;
        public Material T_180;
        public Material T_270;
        
        public Material X;

        private void Awake()
        {
            _fence.Updated += OnUpdated;
        }

        private void OnUpdated(Fence.Cross cross, Fence.Direction dir)
        {
            _rendererss.ForEach(x=>x.material = GetMaterial(cross, dir));
        }

        private Material GetMaterial(Fence.Cross cross, Fence.Direction dir)
        {
            if (cross == Fence.Cross.Zero)
            {
                return Point;
            }
            else if (cross == Fence.Cross.Line)
            {
                return dir == Fence.Direction.Forward ? Line_0 : Line_90;
            }
            else if (cross == Fence.Cross.Coner)
            {
                switch (dir)
                {
                    case Fence.Direction.Forward: return Corner_0;
                    case Fence.Direction.Right: return Corner_90;
                    case Fence.Direction.Down: return Corner_180;
                    case Fence.Direction.Left:return Corner_270;
                    default: return Corner_0;
                }
            }
            else if (cross == Fence.Cross.T)
            {
                switch (dir)
                {
                    case Fence.Direction.Forward: return T_0;
                    case Fence.Direction.Right: return T_90;
                    case Fence.Direction.Down: return T_180;
                    case Fence.Direction.Left:return T_270;
                    default: return T_0;
                }
            }
            else // X
            {
                return X;
            }
        }
    }
}