using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class RotateByFieldMover : MonoBehaviour
    {
        public AbsFieldMover Mover;

        public UltEvent<Vector3> NewRotate;
        
        private void Awake()
        {
            Mover.NewDir += x =>
            {
                switch (x)
                {
                    case AbsFieldMover.DirectionType.Up: NewRotate.Invoke(new Vector3(0,0,0)); break;
                    case AbsFieldMover.DirectionType.Right: NewRotate.Invoke(new Vector3(0,90,0)); break;
                    case AbsFieldMover.DirectionType.Down: NewRotate.Invoke(new Vector3(0,180,0)); break;
                    case AbsFieldMover.DirectionType.Left: NewRotate.Invoke(new Vector3(0,270,0)); break;
                }
            };
        }
    }
}