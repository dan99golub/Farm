using System;
using PowerUps;
using UnityEngine;

namespace DefaultNamespace.Game
{
    [RequireComponent(typeof(CheckMoveNotOnBorder), typeof(AbsFieldMover))]
    public class PlayerMark : MonoBehaviour
    {
        public CheckMoveNotOnBorder CheckMove => GetComponent<CheckMoveNotOnBorder>();
        public AbsFieldMover FieldMover => GetComponent<AbsFieldMover>();
        public IntValueContainer Health => _health;
        public PowerUpContainer ActivePowerUp;
        [SerializeField] private IntValueContainer _health;
    }
}