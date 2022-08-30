using System;
using DG.Tweening;
using MaoUtility.Converse.Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MaoUtility.Converse.Core.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class TextView : BaseConverseComponent
    {
        public override string PrefixAlias => "Text";

        public TextMeshProUGUI Label;
        public float Duration;
        public string TargetText;
        public string CurrentText => Label.text;

        public UnityEvent StartSet;
        public UnityEvent EndSet;

        private Tween _tween;
        
        [Button]
        public void StopSet(bool isEvent) => _tween?.Kill(isEvent);

        [Button]
        public void SetText(string targetText)
        {
            TargetText = targetText;
            StopSet(true);
            if (Duration > 0)
            {
                float progress = 0;
                StartSet.Invoke();
                _tween = DOTween.To(() => progress, x => progress = x, 1, Duration).OnUpdate(() =>
                {
                    var l = (int) (TargetText.Length * progress);
                    var r =TargetText.Substring(0, l);
                    Label.text = @r;
                }).OnComplete(() =>
                {
                    Label.text = targetText;
                    EndSet.Invoke();
                }).SetEase(Ease.Linear);
            }
            else
            {
                StartSet.Invoke();
                Label.text = targetText;
                EndSet.Invoke();
            }
        }
    }
}