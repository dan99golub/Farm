using System;
using System.Collections.Generic;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SO;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class Cage : MonoBehaviour
    {
        public UltEvent OnBroke;
        public UltEvent OnBuild;
        public UltEvent ApplayAnimalStart;

        [ShowInInspector] public bool IsBuild => _state == State.Build;
        [ShowInInspector] public State CurrentState => _state;
        public event Action<State> NewState;
        public AnimalID TargetId => TargetID;

        public Transform PointAnimal => _pointAnimal;
        
        [SerializeField] private AnimalID TargetID;
        [SerializeField] private Transform _pointAnimal;
        [EditID(CageManager.IdLinkTilesOfCage)][SerializeField] private List<Tile> Tiles;
        [SerializeField, ReadOnly]private List<AnimalMark> _animals = new List<AnimalMark>();

        private State _state;

        public void Broke() => ChangeState(State.Broke);
        
        public void Build() => ChangeState(State.Build);

        public bool CanTakeAnimalForZone(AnimalMark mark) => TargetID == mark.Id && mark.CanCagedByZone;

        public bool TryTakeAnimal(AnimalMark mark)
        {
            if (TargetID != mark.Id || _state == State.Broke) return false;
            if (mark.MoveMethod)
            {
                ApplayAnimalStart.Invoke();
                mark.EventPreAnimalInCage();
                mark.MoveMethod.Move(this, () =>
                {
                    _animals.Add(mark);
                    mark.EventAnimalInCage();
                });
            }
            else
            {
                ApplayAnimalStart.Invoke();
                mark.EventPreAnimalInCage();
                mark.gameObject.SetActive(false);
                mark.transform.position = _pointAnimal.transform.position;
                _animals.Add(mark);
                CorutineGame.Instance.WaitFrame(1, () =>
                {
                    mark.gameObject.SetActive(true);
                    mark.EventAnimalInCage();
                });
            }

            return true;
        }

        public bool HasAnimal(AnimalMark mark) => _animals.Contains(mark);

        private HashSet<Tile> _tilesSet;
        public bool HasTile(Tile t)
        {
            if(Application.isPlaying==false)
                return Tiles.Contains(t);
            if (_tilesSet == null) _tilesSet = Tiles.ToHashSet();
            return _tilesSet.Contains(t);
        }

        public bool InZone(ZoneManager.Zone zone)
        {
            foreach (var t in Tiles)
            {
                if (zone.HasTile(t) == false)
                    return false;
            }

            return true;
        }

        public void ForceChangeState(State s) => ChangeState(s, true);
        
        public void ChangeState(State newState, bool forceInvoke = false)
        {
            if(newState==_state && forceInvoke==false) return;
            _state = newState;
            
            if(_state==State.Broke) OnBroke.Invoke(); else OnBuild.Invoke();
        }
        
        public enum State
        {
            Broke, Build
        }
    }
}