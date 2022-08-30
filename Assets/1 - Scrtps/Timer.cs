using System;
using ServiceScript;
using UltEvents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Timer : MonoBehaviour
    {
        [SerializeField, SerializeReference] private TargetTime Time;

        public UltEvent Ended;

        private string _idCor;

        public void StartTime()
        {
            if(_idCor!=null) CorutineGame.Instance.StopWait(_idCor);
            var t = Time.GetTime();
            _idCor = CorutineGame.Instance.Wait(t, ()=>Ended.Invoke());
        }

        private void OnValidate() => Time.Validate();


        public abstract class TargetTime
        {
            public abstract float GetTime();
            public virtual void Validate() { }
        }
        
        public class Single : TargetTime
        {
            [Min(0)]public float Time;
            
            public override float GetTime() => Time;
        }
        
        public class RandomTime : TargetTime
        {
            [Min(0)] public float Min;
            [Min(0)] public float Max;

            public override float GetTime() => Random.Range(Min, Max);

            public override void Validate()
            {
                if (Max < Min) Max = Min;
            }
        }
    }
}