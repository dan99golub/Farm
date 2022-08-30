using System.Linq;
using DefaultNamespace.Game;
using DefaultNamespace.Game.Plants;
using DefaultNamespace.Game.UFOs;
using DG.Tweening;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class S_UfoAI : MonoBehaviour
    {
        public UltEvent TargetFound;
        public UltEvent TargetNotFound;
        
        private Field MainField => ServicesID<Field>.S.Get();
        private CacheField CacheField => Services<Game.Plants.CacheField>.S.Get();
        public UFOMark TargetMark;
        public Cage TargetCage;
        public Transform LastParentTarget;

        [Button]
        public void TryFindTarget()
        {
            TargetCage = null;
            TargetMark = null;
            
            TargetMark = UFOMark.Marks.FirstOrDefault(x => x.CanMove);
            if (!TargetMark)
            {
                TargetNotFound.Invoke();
                return;
            }
            TargetCage = CacheField.CageManager.Cages.FirstOrDefault(x => x.IsBuild && x.TargetId == TargetMark.Mark.Id);
            if (!TargetCage)
            {
                TargetNotFound.Invoke();
                return;
            }

            if ((TargetCage.TargetId == TargetMark.Mark.Id) == false)
            {
                TargetNotFound.Invoke();
                return;
            }

            LastParentTarget = TargetMark.MoveTransform.parent;
            TargetFound.Invoke();
        }
    }
}