using System;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public abstract class MoveMethodAnimal : MonoBehaviour
    {
        public abstract void Move(Cage cage, Action action);
    }
}