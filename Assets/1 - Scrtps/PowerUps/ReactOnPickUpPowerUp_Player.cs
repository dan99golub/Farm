using DefaultNamespace.Game;
using Menu;
using ServiceScript;

namespace PowerUps
{
    public class ReactOnPickUpPowerUp_Player : ReactOnPickUpPowerUp
    {
        protected override void Awake()
        {
            _container = Services<PlayerMark>.S.Get().ActivePowerUp;
            base.Awake();
        }
    }
}