using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GroupTweener
{
    public class GroupTween : MonoBehaviour
    {
        public List<GameObject> Targets;
        public GroupTweenAnimation Animation;
        public float DelayBetweenTarget;
        private Coroutine _corutine;

        [ShowInInspector] public float AllDuration => DelayBetweenTarget * Targets.Count();
        [ShowInInspector] public float DurationByElement => Animation ? Targets.Count() * Animation.Duration : 0;

        public void PrepareAndPlay()
        {
            Prepare();
            Play();
        }

        [Button]
        public void Play()
        {
            if (_corutine == null)
                _corutine = StartCoroutine(PlayCorutine());
        }

        [Button]
        public void Prepare() => Targets.ForEach(x => Animation.Prepare(x));

        [Button] private void GetTarget(GameObject first, bool createNew)
        {
            var newChilds = first.GetComponentsInChildren<Transform>().Except(new[] {first.transform});
            if (createNew)
            {
                Targets=new List<GameObject>();
            }

            newChilds.ForEach(x =>
            {
                if (Targets.Contains(x.gameObject) == false) Targets.Add(x.gameObject);
            });
        }

        private void OnDestroy()
        {
            if(_corutine!=null) StopCoroutine(_corutine);
        }

        private IEnumerator PlayCorutine()
        {
            foreach (var target in Targets)
            {
                Animation.Play(target);
                yield return new WaitForSeconds(DelayBetweenTarget);
            }

            _corutine = null;
        }
    }
}