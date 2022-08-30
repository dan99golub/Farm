using System;
using DefaultNamespace;
using DefaultNamespace.Game;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressBarIniter : MonoBehaviour
    {
        public RectTransform MartTargetProgress;
        public TextMeshProUGUI TextProcent;
        public TextMeshProUGUI TextCurrentProgress;
        public RectTransform LineWin;
        public Image LineCurrentProgress;
        public Image IconTractor;
        public Bar ProgressBar;
    
        private Level CurrentLevel => Services<Level>.S.Get();
        private C_GameScene Controller => Services<C_GameScene>.S.Get();
        private UIMediatorGame UiMediator => Services<UIMediatorGame>.S.Get();
        private ProgressLevel Progress => Services<ProgressLevel>.S.Get();


        private void Start()
        {
            InitMark();
            IconTractor.sprite = UiMediator.GetSpriteTraktor();
            CorutineGame.Instance.WaitFrame(1, () => SetTractorPosition());

            SetCurrentProgress(0);
            Progress.NewProgress += SetCurrentProgress;
            ProgressBar.StartChange.DynamicCalls += () => enabled = true;
            ProgressBar.FinishedChange.DynamicCalls += () => enabled = false;
            enabled = false;
        }

        private void SetCurrentProgress(float progress)
        {
            TextCurrentProgress.text = ((int)(100 * progress)).ToString();
        }

        private void Update() => SetTractorPosition();

        [Button]
        private void SetTractorPosition() => SetAnorhedPositionByFill(IconTractor.transform as RectTransform, true, LineCurrentProgress);

        [Button]
        private void InitMark()
        {
            CorutineGame.Instance.WaitFrame(1, () =>
            {
                SetAnorhedPositionByFill(MartTargetProgress.transform as RectTransform, true, LineWin, PositionByFillAmount(LineWin, true, CurrentLevel.TargetProgress));
                TextProcent.text = ((int) (100 * CurrentLevel.TargetProgress)).ToString() + "%";
            });
        }

        private float PositionByFillAmount(Image i, bool isX)
        {
            var rect = i.transform as RectTransform;
            var startOffset = isX ? rect.sizeDelta.x / 2 : rect.sizeDelta.y / 2;
            startOffset *= -1;
            return startOffset + (startOffset * -1 * 2 * i.fillAmount);
        }
        
        private float PositionByFillAmount(RectTransform i, bool isX, float fill)
        {
            var rect = i.transform as RectTransform;
            var startOffset = isX ? rect.sizeDelta.x / 2 : rect.sizeDelta.y / 2;
            startOffset *= -1;
            return startOffset + (startOffset * -1 * 2 * fill);
        }

        public void SetAnorhedPositionByFill(RectTransform target, bool isX, Image i) => SetAnorhedPositionByFill(target, isX, i.transform as RectTransform, PositionByFillAmount(i, isX));

        public void SetAnorhedPositionByFill(RectTransform target, bool isX, RectTransform newParent, float fill)
        {
            var prevParent = target.parent;
            target.SetParent(newParent.transform);
            var result = target.anchoredPosition;
            if (isX) result.x = fill;
            else result.y = fill;
            target.anchoredPosition = result;
            target.SetParent(prevParent);
        }
    }
}