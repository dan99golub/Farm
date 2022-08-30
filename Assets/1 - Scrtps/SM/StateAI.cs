using UltEvents;

namespace DefaultNamespace.SM
{
    public class StateAI : BaseAI
    {
        public UltEvent OnEnabled;
        public UltEvent OnDisabled;

        private void OnEnable() => OnEnabled.Invoke();

        protected override void OnDisable()
        {
            base.OnDisable();
            OnDisabled.Invoke();
        }
    }
}