using System;
using System.Collections.Generic;
using DG.Tweening;
using ServiceScript;
using UnityEngine;

namespace GroupTweener
{
    public class UpDownFenceAnimation : GroupTweenAnimation
    {
        public float StartH;

        [UnityEngine.Header("Scale")] 
        public float DurationScale;
        public Ease ScaleEase;
        public float Delay;

        [UnityEngine.Header("MoveDown")] 
        public float DurationMove;
        public Ease MoveEase;

        private Dictionary<GameObject, Data> _datas;


        private void Awake() => _datas = new Dictionary<GameObject, Data>();

        public override float Duration => DurationMove + DurationScale + Delay;
        public override void Prepare(GameObject target)
        {
            if(_datas.ContainsKey(target)==false) _datas.Add(target, new Data(target));
            
            var transformPosition = target.transform.position;
            transformPosition.y += StartH;
            target.transform.position = transformPosition;
            target.transform.localScale = Vector3.zero;
        }

        public override void Play(GameObject target)
        {
            target.transform.DOScale(_datas[target].TargetScale, DurationScale).SetEase(ScaleEase).OnComplete(() =>
            {
                CorutineGame.Instance.Wait(Delay, () =>
                {
                    target.transform.DOMove(_datas[target].TargetPos, DurationMove).SetEase(MoveEase).OnComplete(()=>_datas.Remove(target));
                });
            });
        }
        
        private class Data
        {
            public Vector3 TargetPos;
            public Vector3 TargetScale;

            public Data(GameObject target)
            {
                TargetPos = target.transform.position;
                TargetScale = target.transform.localScale;
            }
        }
    }
}