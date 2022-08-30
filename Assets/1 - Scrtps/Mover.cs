using System;
using ServiceScript;
using UltEvents;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class Mover : MonoBehaviour
    {
        public NavMeshAgent Agent;
        [Min(0)] public float DistanceRich;
        [Min(0)] public float TimeBetweenCheck;
        public Color Color;

        public UltEvent Riched;
        private Vector3 _prevPositionOnCheck;

        private string _id;

        public void Move(Vector3 target)
        {
            StopCheck();
            Agent.SetDestination(target);
            _prevPositionOnCheck = transform.position;
            _id = CorutineGame.Instance.Wait(TimeBetweenCheck, () => Check(target));
        }

        public void StopCheck()
        {
            if(_id!=null) CorutineGame.Instance.StopWait(_id);
        }

        private void Check(Vector3 target)
        {
            var dis = Vector3.Distance(target, Agent.transform.position);
            if (dis < DistanceRich) Riched.Invoke();
            else if(Vector3.Distance(_prevPositionOnCheck, transform.position)<0.4f) Riched.Invoke();
            else
            {
                _prevPositionOnCheck = transform.position;
                _id = CorutineGame.Instance.Wait(TimeBetweenCheck, () => Check(target));
            }
        }

        private void OnDrawGizmosSelected()
        {
            if(!Agent) return;
            Gizmos.color = Color;
            Gizmos.DrawWireSphere(Agent.transform.position, DistanceRich);
        }
    }
}