using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.SM
{
    [System.Serializable]
    public abstract class ActionAI
    {
        public abstract void Act();
    }

    [System.Serializable]
    public class IsRichPoint : ActionAI
    {
        public NavMeshAgent Agent;
        public float DistanceStop;
        
        [ReadOnly] public Vector3 Point;
        
        public bool IsRich { get; private set; }
        
        public override void Act()
        {
            IsRich = Vector3.Distance(Agent.transform.position, Point) < DistanceStop;
        }
    }
}