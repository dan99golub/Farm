using System;
using UltEvents;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpShell : MonoBehaviour
    {
        public PowerUp.Data DataPowerUp;
        public UltEvent OnPick;

        public event Action Destroed;

        public void Pick() => OnPick.Invoke();

        private void OnDestroy() => Destroed?.Invoke();
    }
}