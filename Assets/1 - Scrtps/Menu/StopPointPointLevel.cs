using UnityEngine;

namespace Menu
{
    public class StopPointPointLevel : MonoBehaviour
    {
        public LevelDesk ParentDesk => Desk;
        
        protected LevelDesk Desk;

        public void SetDesk(LevelDesk d)
        {
            Desk = d;
        }
    }
}