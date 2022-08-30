using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "New level", menuName = "Game/Level", order = 51)]
    public class Level : ScriptableObject
    {
        public string GUID => _GUID;
        [SerializeField, ReadOnly]private string _GUID;
        public Field Field;
        public Vector2 StartPosition;
        public InitedTile Ground;
        public BorderMark BorderOnPathPlayer;
        public InitedTile Green;
        public InitedTile CageFoundament;
        [Range(0,1f)]public float TargetProgress;
        [Min(0)] public int Award;

        [Button] private void GenerateGuid() => _GUID = Guid.NewGuid().ToString();
        
        [ShowInInspector]
        public int MaxAward
        {
            get
            {
                if (!Field) return Award;

                var animals = Field.GetComponentsInChildren<AnimalMark>();
                var cages = Field.GetComponentsInChildren<Cage>();

                HashSet<AnimalID> ids = new HashSet<AnimalID>();
                cages.ForEach(x => ids.Add(x.TargetId));
                int addAward = 0;
                animals.ForEach(x =>
                {
                    if (ids.Contains(x.Id)) addAward += x.Award.Money;
                });
                return Award + addAward;
            }
        }
    }
}