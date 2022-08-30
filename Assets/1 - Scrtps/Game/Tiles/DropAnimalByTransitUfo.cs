using System.Linq;
using DefaultNamespace.Game.Plants;
using DG.Tweening;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class DropAnimalByTransitUfo : MonoBehaviour
    {
        [Min(0)]public float H;
        public Ease MoveEase;
        [Min(0)]public float DurationFlyToPickUp;
        [Min(0)] public float DurationDrop;
        public UltEvent EndDrop;
        public Transform RootTransform;
        
        private CacheField CacheField => Services<Game.Plants.CacheField>.S.Get();
        public UfoTransiter Ufo;

        public void Drop(MoveAnimalByUfo target)
        {
            float progress = 0;
            var startPos = RootTransform.position;
            var targetCage = CacheField.CageManager.Cages.Where(x => x.IsBuild && x.TargetId == target.Mark.Id).GetRandom();
            DOTween.To(() => progress, x => progress = x, 1, DurationFlyToPickUp).OnUpdate(() =>
            {
                RootTransform.position = Vector3.Lerp(startPos, targetCage.PointAnimal.position + Vector3.up*H, progress);
            }).SetEase(MoveEase).OnComplete(() =>
            {
                target.Root.SetParent(CacheField.transform);
                target.Root.DOMove(targetCage.PointAnimal.position, DurationDrop).OnComplete(() =>
                {
                    target.Pick();
                    target.Drop();
                    Ufo.Drop(target);
                    EndDrop.Invoke();
                });
            });
        }
    }
}