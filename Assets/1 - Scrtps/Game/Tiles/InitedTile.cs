using ServiceScript;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class InitedTile : MonoBehaviour
    {
        [ReadOnly, ShowInInspector]public Tile Tile { get; private set; }
        public ProgressMark.State StateTileForProgress;
        public UltEvent<Tile> Event;

        public void Init(Tile t)
        {
            Tile = t;
            t.Mark.Set(StateTileForProgress);
            CorutineGame.Instance.WaitFrame(1, () => Event.Invoke(Tile));
        }
    }
}