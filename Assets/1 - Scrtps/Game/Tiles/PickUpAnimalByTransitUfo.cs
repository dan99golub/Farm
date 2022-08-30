using DG.Tweening;
using Menu;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class PickUpAnimalByTransitUfo : MonoBehaviour
    {
        [Min(0)]public float H;
        public Ease MoveEase;
        [Min(0)]public float DurationFlyToPickUp;
        [Min(0)] public float DurationPick;
        public Transform PickUpPoint;
        public UltEvent EndPickUp;
        public UltEvent StartPick;
        
        public Transform RootTransform;
        
        public UfoTransiter Ufo;

        public void Pick(MoveAnimalByUfo target)
        {
            float progress = 0;
            var startPos = RootTransform.position;
            DOTween.To(() => progress, x => progress = x, 1, DurationFlyToPickUp).OnUpdate(() =>
            {
                RootTransform.position = Vector3.Lerp(startPos, target.Root.position + Vector3.up*H, progress);
            }).
                SetEase(MoveEase).
                OnComplete(() =>
            {
                target.Pick();
                target.Root.SetParent(PickUpPoint);
                Ufo.PickUpByMark(target);
                target.Root.DOMove(PickUpPoint.transform.position, DurationPick).OnComplete(() => EndPickUp.Invoke()).OnStart(StartPick.Invoke);
            });
        }
    }
}