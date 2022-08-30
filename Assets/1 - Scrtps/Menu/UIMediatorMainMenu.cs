using System;
using DefaultNamespace;
using DTO;
using Lean.Gui;
using ServiceScript;
using SO;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class UIMediatorMainMenu : MonoBehaviour
    {
        public TextMeshProUGUI CurrentLevelLabel;
        public TextMeshProUGUI MoneyLabel;
        public UltEvent OnMoveCar;
        public UltEvent OnStopCar;
        public PanelUI Main;
        public PanelUI Shop;
        public LeanButton ArrorNextLevel;
        public LeanButton ArrorPrevLevel;
        [Min(0)]public float DelayActionOnSplash;
        private StopPointPointLevel _lastViewPointLevel;

        private LevelSelector Selector => Services<LevelSelector>.S.Get();
        private Progress ProgressData => Services<DB>.S.Get().Progress;
        private GroupLevel Levels => Services<GroupLevel>.S.Get();
        private C_MainMenu Controller => Services<C_MainMenu>.S.Get();
        private DB DB => Services<DB>.S.Get();
        private PanelUI FadeScreen => ServicesID<PanelUI>.S.Get(ConstGame.FadeScreen);
        private PointLevel PointLevel => Services<PointLevel>.S.Get();

        private void Awake()
        {
            Selector.NewSelected += NewSelectLevel;
            DB.Progress.Signals.Register<Updated>(OnProgressUpdated);
            Controller.SelectorMoving += () => OnMoveCar.Invoke();
            Controller.SelectorFinished += () => OnStopCar.Invoke();
            CorutineGame.Instance.WaitFrame(1, ()=>
            {
                NewSelectLevel(Selector.CurrentLevel);
                OnProgressUpdated(new Updated());
            });
        }

        public void OpenShopPanel()
        {
            Main.SetActive(false);
            _lastViewPointLevel = PointLevel.CurrentView;
            PointLevel.MoveToShop(() => Shop.SetActive(true));
        }

        public void CloseShopPanel()
        {
            Shop.SetActive(false);
            if (!PointLevel.MoveTo(_lastViewPointLevel, () => Main.SetActive(true)))
                Splash(() =>
                {    
                    PointLevel.InstanceMove(_lastViewPointLevel);
                    CorutineGame.Instance.Wait(0.25f, () => Main.SetActive(true));
                });
        }
        
        public void Splash(Action callback)
        {
            FadeScreen.SetActive(true);
            CorutineGame.Instance.Wait(DelayActionOnSplash, () =>
            {
                callback.Invoke();
                FadeScreen.SetActive(false);
            });
        }

        public void OnProgressUpdated(Updated e)
        {
            MoneyLabel.text = DB.Progress.Money.ToString();
        }
        
        private void NewSelectLevel(Level obj)
        {
            CurrentLevelLabel.text = Levels.Levels.Contains(obj) ? (Levels.Levels.IndexOf(obj)+1).ToString() : (-1).ToString();
            ArrorNextLevel.interactable = Selector.CanSelectNext();
            ArrorPrevLevel.interactable = Levels.GetPrev(obj) != null;
        }
    }
}