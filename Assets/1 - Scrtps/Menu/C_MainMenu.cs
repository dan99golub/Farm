using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DefaultNamespace;
using DefaultNamespace.Game;
using DTO;
using ServiceScript;
using SO;
using UltEvents;
using UnityEngine;

namespace Menu
{
    public class C_MainMenu : MonoBehaviour
    {
        public string NameGameScene;
        public DeskSwitcher DeskSwitcher;
        public float LoadDelay = 1.1f;

        public UltEvent SelectorMoving;
        public UltEvent SelectorFinished;

        private LevelSelector Selector => Services<LevelSelector>.S.Get();
        private PointLevel PointerLevel => Services<PointLevel>.S.Get();
        private Progress ProgressData => Services<DB>.S.Get().Progress;
        private PanelUI FadeScreen => ServicesID<PanelUI>.S.Get(ConstGame.FadeScreen);
        private UIMediatorMainMenu UIMediator => Services<UIMediatorMainMenu>.S.Get();
        private GroupLevel Levels => Services<GroupLevel>.S.Get();
        private DB Db => Services<DTO.DB>.S.Get();
        private C_GameScene.ResultGame ResultGame => Services<C_GameScene.ResultGame>.S.Get();

        private void Start()
        {
            CorutineGame.Instance.WaitFrame(1, () =>
            {
                if (Services<Level>.S.Get() != null)
                {
                    Selector.Select(Services<Level>.S.Get());
                    PointerLevel.InstanceMove(DeskSwitcher.GetViewByLevel(Services<Level>.S.Get()));
                    SelectorFinished.Invoke();
                }
                else
                {
                    Selector.Select(FirstCloseLevel());
                    PointerLevel.InstanceMove(DeskSwitcher.GetViewByLevel(Selector.CurrentLevel));
                    SelectorFinished.Invoke();
                }

                bool canAutoMoveNext = ProgressData.LevelIsPass(Services<Level>.S.Get())==false &&
                                       ProgressData.LevelIsPass(Levels.GetPrev(Services<Level>.S.Get()));
                
                if (ResultGame != null && ResultGame.MakeNextLevel && canAutoMoveNext)
                {
                    var l = Levels.GetPrev(FirstCloseLevel());
                    Selector.Select(l);
                    UIMediator.Main.gameObject.SetActive(false);
                    var viewLevel = DeskSwitcher.GetViewByLevel(l);
                    PointerLevel.InstanceMove(viewLevel);
                    CorutineGame.Instance.Wait(viewLevel.WaitTime, () =>
                    {
                        //var r = Selector.Next();
                        //if (!r)
                            //PointerLevel.EndMove.DynamicCalls += () => { LoadGame(); };
                        //else
                        UIMediator.Main.gameObject.SetActive(true);
                    });
                }
                else if(ResultGame!=null && canAutoMoveNext)
                {
                    if (Selector.CanSelectNext())
                    {
                        UIMediator.Main.SetActive(false);
                        CorutineGame.Instance.Wait(DeskSwitcher.GetViewByLevel(Selector.CurrentLevel).WaitTime, () =>
                        {
                            //Selector.Next();
                            UIMediator.Main.SetActive(true);
                        });
                    }
                }
                Selector.NewSelected += OnNewSelectLevel;    
            });
            CorutineGame.Instance.WaitFrame(2, () => FadeScreen.SetActive(false));
        }

        private Level FirstCloseLevel()
        {
            var closeLevel = Levels.Levels.FirstOrDefault(l => Db.Progress.LevelIsPass(l) == false);
            return closeLevel != null ? closeLevel : Levels.Levels.Last();
        }

        public void LoadGame()
        {
            Services<Level>.S.Set(Selector.CurrentLevel);
            Services<C_GameScene.ResultGame>.S.Set(null);
            FadeScreen.SetActive(true);
            CorutineGame.Instance.Wait(LoadDelay, () => SceneLoader.Load(NameGameScene));
        }

        public bool LevelNumberIsPass(int index)
        {
            if (index < 0 || index >= Levels.Levels.Count())
            {
                Debug.LogError("Запрос индекс уровня за пределеом массива");
                return false;
            }
            return Db.Progress.LevelIsPass(Levels.Levels[index]);
        }
        
        public void ClearProgress()
        {
            Db.ClearAll();
            SceneLoader.Restart();
        }
        
        private void OnNewSelectLevel(Level obj)
        {
            var targetMove = DeskSwitcher.GetViewByLevel(obj);
            if(!targetMove) return;
            SelectorMoving?.Invoke();
            if (PointerLevel.MoveTo(targetMove, () => SelectorFinished?.Invoke()) == false)
            {
                UIMediator.Splash(() =>
                {
                    DeskSwitcher.SwapDesk(DeskSwitcher.Desks.FirstOrDefault(d => d.Datas.FirstOrDefault(s => s.LevelSo == obj) != null));
                    PointerLevel.InstanceMove(targetMove);
                    SelectorFinished?.Invoke();    
                });
            }
        }
    }
}