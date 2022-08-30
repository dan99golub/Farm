using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public abstract class AbsFieldMover : MonoBehaviour
    {
        public abstract event Action<(Tile from, Tile to)> Moving;
        public abstract event Action<Tile> ReachPoint;
        public abstract event Action<DirectionType> NewDir;
        
        public abstract DirectionType Direction { get; }

        [Button] public abstract void Init(Field field);

        [Button] public abstract void Move(DirectionType d);


        public enum DirectionType 
        {
            None, Right, Left, Up, Down
        }
    }
}