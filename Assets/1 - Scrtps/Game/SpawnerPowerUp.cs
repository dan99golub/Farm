using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game.Plants;
using DefaultNamespace.SM;
using DG.Tweening;
using Menu;
using PowerUps;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using LinqExtensions = Sirenix.Utilities.LinqExtensions;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Game
{
    public class SpawnerPowerUp : MonoBehaviour
    {
        [Header("Activators")]
        [SerializeField] private Timer _timer;
        [SerializeField] private AgresiveAnimalActivator _agresiveActivator=new AgresiveAnimalActivator();
        
        [Header("Setting Mono")]
        [SerializeField] private SettingPrefab[] _prefabs;
        [SerializeField] private RandomPointOnField _getRandomPoint;
        [SerializeField] private Transform _pointSpawnPrefabs;
        [SerializeField] private MoveItemOnAir _mover;
        [SerializeField] private Ease _Ease;
        [SerializeField] private float _durationFly;

        [Min(0)] public int MaxCountEvent; 
        
        private List<PowerUpShell> _spawnedPrefabs=new List<PowerUpShell>();
        [ShowInInspector]private PowerUpShell _ufoUp;

        private AnimalManager Animals => Services<CacheField>.S.Get().AnimalManager;

        private void Start()
        {
            _timer.GenerateNewFinish();
            _agresiveActivator.MakeSub(Animals);
            new List<BaseActivator>(){_timer}.ForEach(x=>
            {
                x.Init(_prefabs, () => _spawnedPrefabs.Count() < MaxCountEvent);
                x.Spawned += OnSpawned;
            });
            _agresiveActivator.Init(_prefabs, () => _ufoUp==null);
            _agresiveActivator.Spawned += x =>
            {
                _ufoUp = x;
                OnSpawned(x);
            };
        }



        private void OnSpawned(PowerUpShell obj)
        {
            _spawnedPrefabs.Add(obj);
            obj.Destroed += () =>
            {
                OnDestroyPrefab(obj);
            };
            var p = _getRandomPoint.GetPoint().movePos;
            p.y = _pointSpawnPrefabs.position.y;
            _mover.Move(_pointSpawnPrefabs.position, Tile.ConvertGlobalToTilePos(p, true), _Ease, _durationFly, obj.transform);
        }

        private void OnDestroyPrefab(PowerUpShell upShell)
        {
            _spawnedPrefabs.Remove(upShell);
        }

        private void Update()
        {
            _timer.Update();
        }


        [System.Serializable]
        public class SettingPrefab
        {
            public PowerUpShell Prefab;
            [SerializeReference]public ActivatedEvent ActiveMethod;
            
            [System.Serializable] public abstract class ActivatedEvent { }
        
            [System.Serializable] public class TimerActivated : ActivatedEvent { }
            
            [System.Serializable] public class AgresiveActivator : ActivatedEvent { }
        }
        
        [System.Serializable]
        public abstract class BaseActivator
        {
            public event Action<PowerUpShell> Spawned; 
            
            private List<SettingPrefab> _prefabs = new List<SettingPrefab>();
            private Func<bool> _canSpawn;

            public void Init(SettingPrefab[] settingPrefab, Func<bool> canSpawn)
            {
                foreach (var prefab in settingPrefab)
                {
                    if(IsCorrectMethod(prefab.ActiveMethod))
                        _prefabs.Add(prefab);
                }
                _canSpawn = canSpawn;
            }

            [Button]
            protected bool Spawn()
            {
                if (_canSpawn()==false) return false;
                var obj = Instantiate(_prefabs.GetRandom().Prefab, new Vector3(-10, 0, -10), Quaternion.identity);
                Spawned?.Invoke(obj);
                return true;
            }

            protected abstract bool IsCorrectMethod(SettingPrefab.ActivatedEvent settingPrefabActiveMethod);
        }
        
        [System.Serializable]
        public class Timer : BaseActivator
        {
            public float MinTime;
            public float MaxTime;

            [ShowInInspector, ReadOnly] private float _currentFinish;
            public float _currentValue;
            
            public void Update()
            {
                _currentValue += Time.deltaTime;
                if (_currentValue >= _currentFinish)
                {
                    if (Spawn())
                    {
                        _currentValue = 0;
                        GenerateNewFinish();
                    }
                }
            }

            public void GenerateNewFinish()
            {
                _currentFinish = Random.Range(MinTime, MaxTime);
            }

            protected override bool IsCorrectMethod(SettingPrefab.ActivatedEvent settingPrefabActiveMethod) => settingPrefabActiveMethod is SettingPrefab.TimerActivated;
        }
        
        [System.Serializable]
        public class AgresiveAnimalActivator : BaseActivator
        {
            public void MakeSub(AnimalManager manag)
            {
                manag.Animals.ForEach(x =>
                {
                    if (x.TryGetComponent<S_Agresive>(out var r))
                    {
                        r.OnEnab.DynamicCalls += () =>
                        {
                            Spawn();
                        };
                    }
                });
            }

            protected override bool IsCorrectMethod(SettingPrefab.ActivatedEvent settingPrefabActiveMethod) => settingPrefabActiveMethod is SettingPrefab.AgresiveActivator;
        }
    }
}