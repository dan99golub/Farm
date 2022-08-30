using System;
using System.Linq;
using DefaultNamespace.Game.Plants;
using DG.Tweening;
using DTO;
using Menu;
using Menu.Shop_Component;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SO;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.Game
{
    public class UIMediatorGame : MonoBehaviour
    {
        public Level CurrentLevel => Services<Level>.S.Get();
        public GroupLevel Levels => Services<GroupLevel>.S.Get();
        public ProgressLevel ProgressLevel => Services<ProgressLevel>.S.Get();
        public PlayerMark Player => Services<PlayerMark>.S.Get();
        public C_GameScene Controller => Services<C_GameScene>.S.Get();
        private SettingGame Setting => Services<DB>.S.Get() ? Services<DB>.S.Get().SettingGame : null;
        private PanelUI FadeScreen => ServicesID<PanelUI>.S.Get(ConstGame.FadeScreen);
        
        public UltEvent ButtonInputOn;
        public UltEvent SwipeInputOn;
        public UltEvent<bool> HasNextLevel;
        public float LoadDelayTurnOfFade = 0.6f;
        public float DelayShootDownScene = 1;

        [Header("Progress bar")]
        public Bar ProgressBarLevel;
        public Bar ProgressBarLevelWin;
        [Min(0)]public float DurationChange;

        [Header("Player HP")] 
        public IconBar BarHp;

        [Header("Other")] 
        public RectTransform CoinPanel;
        public int SiblingIndexCointPanel = 0;
        public RectTransform TargetSpawnCoinPanel;

        [Header("StateGame")]
        public TextMeshProUGUI LabelNumberLevel;
        public TextMeshProUGUI LabelResultGame;
        public TextMeshProUGUI LabelMoneyOnEnd;
        [ColorUsage(false)] public Color WinColor;
        [ColorUsage(false)] public Color LoseColor;
        [SerializeField, Min(0)] public float DurationAnimCoin;
        public UltEvent Win;
        public UltEvent Lose;
        
        private SettingGame.InputKey _inputType;
        private Progress ProgressGame => Services<DB>.S.Get().Progress;

        private void Awake()
        {
            _inputType = Setting.InputType;
            ProgressBarLevelWin.SetProgress(CurrentLevel.TargetProgress, 0);
            ProgressBarLevel.SetProgress(ProgressLevel.Progress, DurationChange);
            ProgressLevel.NewProgress += x => ProgressBarLevel.SetProgress(x, DurationChange);
            Controller.Win += () => OnFinishGame(true);
            Controller.Lose += () => OnFinishGame(false);
            LabelNumberLevel.text = Levels.NumberOrderLevel(CurrentLevel).ToString();
            Player.Health.Changed += () => UpdateHP();
            Services<CacheField>.S.Get().CageManager.Cages.
                ForEach(x => 
                    x.ApplayAnimalStart.DynamicCalls += () => Instantiate(CoinPanel, TargetSpawnCoinPanel).transform.SetSiblingIndex(SiblingIndexCointPanel));
            UpdateHP();
            
            Setting.Signals.Register((Updated e) =>
            {
                if (_inputType != Setting.InputType)
                {
                    _inputType = Setting.InputType;
                    InvokeEventInput(_inputType);
                }
            });
            InvokeEventInput(_inputType);
            HasNextLevel.Invoke(Controller.HasNextLevel);
            LabelMoneyOnEnd.text = Controller.Result.NewMoney.ToString();
            
            CorutineGame.Instance.Wait(LoadDelayTurnOfFade, ()=>FadeScreen.SetActive(false));
        }

        [Button]
        public Sprite GetSpriteTraktor()
        {
            var products = Services<ProductContainer>.S.Get();
            var r = products.PointLevels.FirstOrDefault(x => ProgressGame.GetSelectedGuidProduct<PointLevel>() == x.GUID);
            return r != null ? r.Icon : products.DefaultPointLevel.Icon;
        }
        
        public void ToMenu()
        {
            FadeScreen.SetActive(true);
            CorutineGame.Instance.Wait(DelayShootDownScene, () => Controller.LoadMenu());
        }

        public void Restart()
        {
            FadeScreen.SetActive(true);
            CorutineGame.Instance.Wait(DelayShootDownScene, () => Controller.RestartScene());
        }

        public void NextLevel()
        {
            FadeScreen.SetActive(true);
            CorutineGame.Instance.Wait(DelayShootDownScene, () => Controller.LoadNextLevel());
        }

        private Tween _animMoneyLabel;
        public void AnimFinishMoneyText(int count)
        {
            _animMoneyLabel?.Kill();
            float progress = 0;
            _animMoneyLabel = DOTween.To(() => progress, x => progress = x, 1, DurationAnimCoin).OnUpdate(() =>
            {
                LabelMoneyOnEnd.text = ((int) (count * progress)).ToString();
            });
        }
        
        private void OnFinishGame(bool isWin)
        {
            LabelResultGame.text = isWin ? "You\nWin" : "You\nLose";
            LabelResultGame.color = isWin ? WinColor : LoseColor;
            if(isWin) Win.Invoke(); else Lose.Invoke();
            CorutineGame.Instance.Wait(0.8f, () => AnimFinishMoneyText(Controller.Result.NewMoney));
        }
        
        private void InvokeEventInput(SettingGame.InputKey input)
        {
            if(input == SettingGame.InputKey.Button) ButtonInputOn.Invoke();
            else SwipeInputOn.Invoke();
        }
        
        private void UpdateHP() => BarHp.Set(Player.Health.Current, Player.Health.Max-Player.Health.Current);
    }
}