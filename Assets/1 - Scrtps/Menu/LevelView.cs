using System;
using DefaultNamespace.Game;
using DTO;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using UltEvents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Menu
{
    public class LevelView : StopPointPointLevel
    {
        public float DelayBeforeAnim = 1f;
        [Min(0.1f)][SerializeField] private float _waitTime;
        [SerializeField] private Vector2 Size;
        [SerializeField] private bool ShowAllways;
        
        
        [ShowInInspector]
        private Level _data;

        public UltEvent Opened;
        public UltEvent PlayAnimOpened;
        public UltEvent Closed;
        private bool _lastOpenClose;

        private Progress ProgressData => Services<DB>.S.Get().Progress;
        private C_GameScene.ResultGame ResultLastGame => Services<C_GameScene.ResultGame>.S.Get();
        public float WaitTime => _waitTime;

        public void Init(Level data)
        {
            _data = data;
            _lastOpenClose = ProgressData.LevelIsPass(data);
            CorutineGame.Instance.WaitFrame(1, () => SetOpen(_lastOpenClose));
        }

        public void SetOpen(bool isOpen)
        {
            if (ResultLastGame != null)
            {
                if (ResultLastGame.PassLevels.Contains(_data.GUID))
                {
                    Closed.Invoke();
                    CorutineGame.Instance.Wait(DelayBeforeAnim, PlayAnimOpened.Invoke);
                    return;
                }
            }

            if (isOpen)
            {
                Opened.Invoke();
            }
            else
            {
                Closed.Invoke();
            }
        }

        [Button]private void InvokeOpened() => Opened.Invoke();
        [Button]private void InvokeAnim() => PlayAnimOpened.Invoke();
        [Button]private void InvokeClose() => Closed.Invoke();

        private void OnDrawGizmos()
        {
            if (!ShowAllways) return;
            
            Gizmos.color= Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(Size.x, 0.2f, Size.y));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color= Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(Size.x, 0.2f, Size.y));
        }
    }
}