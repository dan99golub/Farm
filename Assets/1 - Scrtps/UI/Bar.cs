using DG.Tweening;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private Ease _ease;

        private Tween _tween;

        public UltEvent StartChange;
        public UltEvent FinishedChange;
        
        public void SetProgress(float p, float d)
        {
            _tween?.Kill();
            _tween = _bar.DOFillAmount(p, d).SetEase(_ease).OnComplete(()=>
            {
                _tween = null;
                FinishedChange.Invoke();
            }).OnStart(StartChange.Invoke);
        }
    }
}