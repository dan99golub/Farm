using System.Linq;
using Sirenix.Utilities;
using UltEvents;
using UnityEngine;

namespace PowerUps
{
    public class PickerPowerUp : MonoBehaviour
    {
        public UltEvent OnPick;
        public PowerUpContainer Container;

        public void TryPick(Collider collider)
        {
            var shells = collider.GetComponents<PowerUpShell>();
            if(shells.Count()==0) return;
            shells.ForEach(x => TryPick((PowerUpShell) x));
        }

        private void TryPick(PowerUpShell shell)
        {
            if(!Container) return;
            if (shell.DataPowerUp.ReaplcaeMethod.Replace(shell.DataPowerUp, Container))
            {
                OnPick.Invoke();
                shell.Pick();
            }
        }
    }
}