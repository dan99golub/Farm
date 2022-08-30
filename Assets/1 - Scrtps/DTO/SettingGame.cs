using System;
using Sirenix.OdinInspector;

namespace DTO
{
    [System.Serializable]
    public class SettingGame
    {
        private SignalBus _signals;
        public SignalBus Signals => _signals??=new SignalBus();

        public InputKey InputType = InputKey.Swipe;
        public bool Music = true;
        public bool Sound = true;
        public bool Vibration = true;

        [Button] private void MakeUpdate() => Signals.Fire(new Updated());

        public enum InputKey
        {
            Swipe, Button
        }
    }
}