using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    [RequireComponent(typeof(Field))]
    public class CageManager : MonoBehaviour
    {
        public const string IdListCage = "CageList";
        public const string IdLinkTilesOfCage = "LinkTile";
        
        public event Action<Cage> SomeCageBuilded;
        
        [EditID(IdListCage), SerializeField] private List<Cage> _cages = new List<Cage>();

        public Field Field => GetComponent<Field>();

        public ReadOnlyCollection<Cage> Cages => _cages.AsReadOnly();

        private void Awake()
        {
            _cages.ForEach(x=>
            {
                x.ForceChangeState(Cage.State.Broke);
                x.OnBuild += () => SomeCageBuilded?.Invoke(x);
            });
        }

        public Cage GetCage(Func<Cage, bool> predict) => _cages.FirstOrDefault(predict);

        public void OnValidate() => _cages = _cages.Where(x => x != null).ToList();
    }
}