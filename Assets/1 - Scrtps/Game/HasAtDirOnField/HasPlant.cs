using System;
using DefaultNamespace.Game.Plants;
using DG.Tweening;
using Menu;
using ServiceScript;

namespace DefaultNamespace.Game.HasAtDirOnField
{
    public class HasPlant : HasSomeAtField<Plant>
    {
        private FenceManager Fencer => Services<FenceManager>.S.Get();

        private string _corStart;
        private string _corUpdate;
        
        private void Start()
        {
            if (enabled)
            {
                Fencer.ViewUpdated += OnViewUpdate;
                _corStart = CorutineGame.Instance.WaitFrame(1, () => UpdateView());
            }
        }

        private void OnViewUpdate()
        {
            _corUpdate = CorutineGame.Instance.WaitFrame(4,()=>
            {
                if(gameObject)
                    UpdateView();
            });
        }

        private void OnDestroy()
        {
            CorutineGame.Instance.StopWait(_corStart);
            CorutineGame.Instance.StopWait(_corUpdate);
            Fencer.ViewUpdated -= OnViewUpdate;
        }
    }
}