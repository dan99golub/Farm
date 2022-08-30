using DefaultNamespace.Game;
using UnityEngine;

namespace DefaultNamespace
{
    public class S_Player : MonoBehaviour
    {
        public void MakeGreenSquare(GreeningZone greeningZone, AbsFieldMover mover)
        {
            greeningZone.Set(mover.Direction);
        }
    }
}