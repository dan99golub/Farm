using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DTO;
using Game.Dirty;
using LoadScenes;
using Menu;
using Newtonsoft.Json;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SO;
using UltEvents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace DefaultNamespace.Game
{
    public class C_GameScene : MonoBehaviour
    {
        public LayerMask Mask;
        public ZoneManager ZoneManager;
        public string NameSceneMainMenu;
        public float LoadDelay = 1.1f;

        public UltEvent Win;
        public UltEvent Lose;
        public UltEvent NewGreenZone;
        
        public PlayerMark Player => Services<PlayerMark>.S.Get();
        public Level CurrentLevel => Services<Level>.S.Get();
        public FenceManager ManagerFance => Services<FenceManager>.S.Get();
        private ProgressLevel LevelProgress => Services<ProgressLevel>.S.Get();
        private DB DB => Services<DB>.S.Get();
        public GroupLevel Levels => Services<GroupLevel>.S.Get();
        private GreenManager GreenManag => Services<GreenManager>.S.Get();
        
        private CageManager _cagesOfField;
        private AnimalManager _animalManager;
        private Field _field => ServicesID<Field>.S.Get();
        private List<InitedTile> _greenTiles = new List<InitedTile>();
        private GameObject _parentGreenTiles;

        [ShowInInspector, EnableIf("CanShowInInspector")]private ResultGame _resultGame;
        public ResultGame Result => _resultGame ??= GetResult();

        public bool HasNextLevel => Levels.NextLevelOrNull(CurrentLevel) != null;

        private bool CanShowInInspector => Application.isPlaying;

        private void Start()
        {
            //init
            _field.Init();
            _cagesOfField = _field.GetComponent<CageManager>();
            _animalManager = _field.GetComponent<AnimalManager>();
            
            LevelProgress.Init(_field);
            ZoneManager.Init(_field);
            Player.transform.position = new Vector3(CurrentLevel.StartPosition.x, 0, CurrentLevel.StartPosition.y);
            Player.FieldMover.Init(_field);

            //Subscribes
            Player.CheckMove.Finish += OnFinishMove;
            ZoneManager.NewZones += OnNewZone;
            LevelProgress.NewProgress += OnNewProgress;
            Player.Health.Changed += OnChangeHP;
            Player.CheckMove.Broken += OnBroke;
            
            // need call this
            CorutineGame.Instance.WaitFrame(1, ()=>
            {
                ManagerFance.UpdateView();
                //GreenManag.UpdateTex();
            });
            LevelProgress.CalculateProgress();
            _field.Surface.BuildNavMesh();
            _parentGreenTiles = new GameObject("Parent Green");
            SpawnGreen(20);
        }

        private ResultGame GetResult()
        {
            if (Services<ResultGame>.S.Get() != null)
            { 
                var clone = Services<ResultGame>.S.Get().Clone();
                Services<ResultGame>.S.Set(clone);
                return clone;
            }
            else
            {
                var r = new ResultGame();
                Services<ResultGame>.S.Set(r);
                return r;
            }
        }

        private void OnBroke(GameObject obj)
        {
            Player.Health.Current--;
        }

        private void OnChangeHP()
        {
            if (Player.Health.Current == 0)
            {
                Lose.Invoke();
                Player.FieldMover.enabled = false;
            }
        }

        public bool CurrentLevelIsTarget(Level target) => CurrentLevel == target && DB.Progress.LevelIsPass(target)==false;

        private void OnNewProgress(float progress)
        {
            if (progress > CurrentLevel.TargetProgress)
            {
                if(Result.PassLevels.Contains(CurrentLevel.GUID)==false && DB.Progress.LevelIsPass(CurrentLevel)==false)
                    Result.NewMoney += CurrentLevel.Award;
                Player.FieldMover.enabled = false;

                if (!DB.Progress.LevelIsPass(CurrentLevel))
                {
                    Result.AddPassLevel(CurrentLevel);
                    Result.Events.Fire(new Updated());
                }
                DB.Progress.AddPassLevel(CurrentLevel);
                Win.Invoke();
            }
        }

        [Button]
        public void FinishGame() => OnNewProgress(1);

        private void OnFinishMove((Field.TilePoint startPoint, Field.TilePoint finishPoint) eventData) => SetFenceByPath(eventData);

        public void SetFenceByPath((Field.TilePoint startPoint, Field.TilePoint finishPoint) path)
        {
            var p = path.startPoint;
            while (p!=null)
            {
                if(p.Content.Content!=null && p.Content.Mark.CurrentState == ProgressMark.State.Incomplete)
                    p.Content.ReplaceContent(Instantiate(CurrentLevel.BorderOnPathPlayer).InitedTile);
                p = p.Next;
            }

            CorutineGame.Instance.WaitFrame(1,()=>
            {
                ManagerFance.UpdateView();
                GreenManag.UpdateTex();
            });
            CorutineGame.Instance.WaitFrame(2, ()=> MakeUpdateZone());
            CorutineGame.Instance.WaitFrame(3,()=> LevelProgress.CalculateProgress());

            void MakeUpdateZone()
            {
                var r = false;
                var currentPoint = path.startPoint;
                do
                {
                    r = ZoneManager.UpdateByTile(currentPoint.Content);
                    currentPoint = currentPoint.Next;
                } while (r == false && currentPoint!=null);    
            }
        }

        private void OnNewZone(List<ZoneManager.Zone> obj)
        {
            obj.ForEach(x => CheckZone(x));
            _field.Surface.BuildNavMesh();
        }

        private void CheckZone(ZoneManager.Zone zone)
        {
            var listAnimal = _animalManager.GetAnimalsInZone(zone);
            var firstAnimal = listAnimal.FirstOrDefault();

            var cageCollection = CheckZoneAtCage(zone);

            if (listAnimal.Count() > 0)
            {
                if (listAnimal.Count() == 1)
                {
                    if (cageCollection.Count() == 1)
                    {
                        if(!firstAnimal.CanCagedByZone) return;
                        var c = BuildFirstCage(c => {});
                        if(c!=null) TryMoveAnimalAndMakeGreen(c, firstAnimal, zone);
                    }
                    else if (cageCollection.Count() == 0)
                    {
                        var cage = _cagesOfField.GetCage(x => x.CanTakeAnimalForZone(firstAnimal));
                        if (cage && firstAnimal.CanCagedByZone) CorutineGame.Instance.WaitFrame(1, () => TryMoveAnimalAndMakeGreen(cage, firstAnimal, zone));
                    }
                    else { /*dirty*/ } // Cages > 1 
                }
            }
            else // NO ANIMAL
            {
                if (cageCollection.Count() == 1) // build, set green and fondument
                    BuildFirstCage(c=>ReplaceContent(zone, GetGreen));
                else if (cageCollection.Count() == 0) // MakeGreen
                {
                    ReplaceContent(zone, GetGreen);
                    NewGreenZone.Invoke();
                }
            }

            bool TryMoveAnimalAndMakeGreen(Cage cage, AnimalMark animal, ZoneManager.Zone zone)
            {
                var result = cage.TryTakeAnimal(animal);
                if (result)
                {
                    if(cage.InZone(zone)) ReplaceContent(zone, GetGreen);
                    else ReplaceContent(zone, GetGreen);
                    NewGreenZone.Invoke();
                }
                return result;
            }

            Cage BuildFirstCage(Action<Cage> actionIfCageNotInZone)
            {
                var cage = cageCollection.ToList()[0];
                if (cage.InZone(zone))
                {
                    cage.Build();
                    ReplaceContent(zone, () => Instantiate(CurrentLevel.CageFoundament));
                    //ReplaceContent(zone, () => Instantiate(CurrentLevel.CageFoundament), t => cage.HasTile(t));
                    //ReplaceContent(zone, () => Instantiate(CurrentLevel.Green), t => !cage.HasTile(t));
                    NewGreenZone.Invoke();
                    return cage;
                }
                else
                {
                    actionIfCageNotInZone?.Invoke(cage);
                    cage.gameObject.SetActive(false);
                    return null;
                }
            }
        }

        private void SpawnGreen(int countInGroup)
        {
            for (int i = 0; i < countInGroup; i++)
            {
                var newGreen = Instantiate(CurrentLevel.Green);
                newGreen.gameObject.SetActive(false);
                newGreen.transform.SetParent(_parentGreenTiles.transform);
                _greenTiles.Add(newGreen);
            }

            var size = ServicesID<Field>.S.Get().Size;
            if (_greenTiles.Count() < size.x * size.y)
            {
                CorutineGame.Instance.WaitFrame(1, ()=>SpawnGreen(countInGroup));
            }
        }
        
        private InitedTile GetGreen()
        {
            if (_greenTiles.Count() > 0)
            {
                var r = _greenTiles[0];
                _greenTiles.RemoveAt(0);
                r.gameObject.SetActive(true);
                return r;
            }
            return Instantiate(CurrentLevel.Green);
        }

        private IEnumerable<Cage> CheckZoneAtCage(ZoneManager.Zone zone)
        {
            HashSet<Cage> cages =new HashSet<Cage>();
            if (_cagesOfField == null) return cages;
            zone.Tiles.ForEach(t =>
            {
                var cage = _cagesOfField.GetCage(c => c.HasTile(t));
                if(cage) cages.Add(cage);
            });
            return cages;
        }

        private void ReplaceContent(ZoneManager.Zone zone, Func<InitedTile> Content, Func<Tile, bool> SortTile) 
            => zone.Tiles.Where(SortTile).ForEach(x => x.ReplaceContent(Content()));

        private void ReplaceContent(ZoneManager.Zone zone, Func<InitedTile> Content) 
            => zone.Tiles.ForEach(x => x.ReplaceContent(Content()));

        public void LoadMenu(bool isLoadNextLevel = false)
        {
            DB.Progress.AddMoney(Result.NewMoney);
            Result.MakeNextLevel = isLoadNextLevel;
            CorutineGame.Instance.Wait(LoadDelay, () => SceneLoader.Load(NameSceneMainMenu));
        }

        public void RestartScene()
        {
            DB.Progress.AddMoney(Result.NewMoney);
            CorutineGame.Instance.Wait(LoadDelay, () => SceneLoader.Restart());
        }

        public void LoadNextLevel()
        {
            Services<Level>.S.Set(Levels.NextLevelOrNull(CurrentLevel));
            LoadMenu(true);
        }
        
        [System.Serializable]
        public class ResultGame
        {
            private SignalBus _events;
            private bool _makeNextLevel;
            public bool MakeNextLevel
            {
                get => _makeNextLevel;
                set => _makeNextLevel = value;
            }

            public SignalBus Events => _events??=new SignalBus();

            [SerializeField] private int _newMoney = 0;
            [SerializeField] private List<string> _passLevels = new List<string>();

            public ReadOnlyCollection<string> PassLevels => _passLevels.AsReadOnly();

            [Button] public void AddPassLevel(Level level)
            {
                if(!_passLevels.Contains(level.GUID))
                    _passLevels.Add(level.GUID);
            }

            public int NewMoney
            {
                get => _newMoney;
                set
                {
                    if (value < 0) _newMoney = 0;
                    else _newMoney = value;
                }
            }

            [Button] public void UpdateView()
            {
                _events.Fire(new Updated());
            }

            public ResultGame Clone()
            {
                var r = new ResultGame();
                r._newMoney = 0;
                r._passLevels = _passLevels;
                return r;
            }
        }
    }
}