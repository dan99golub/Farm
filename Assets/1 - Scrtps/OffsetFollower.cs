using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class OffsetFollower : MonoBehaviour
    {
        public Transform Target;
        public Vector3 Offset;
        [Min(0)] public float Speed;
        [Min(0)] public float StopMoveOnDis;

        private void Update()
        {
            if(!Target) return;
            var targetPos = TargetPos();
            if(Vector3.Distance(targetPos, transform.position) < StopMoveOnDis) return;
            var dir = (transform.position - targetPos).normalized*-1;
            transform.position += dir * Time.deltaTime * Speed;
        }
        
        private Vector3 TargetPos() =>Target.position + Offset; 
    }
}