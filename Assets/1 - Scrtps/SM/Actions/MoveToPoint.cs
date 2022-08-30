using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.SM
{
    [System.Serializable]
    public class MoveToPoint : ActionAI
    {
        public NavMeshAgent Agent;

        [ReadOnly]  public Vector3 PointForMove;

        public override void Act()
        {
            Agent.SetDestination(PointForMove);
        }
    }
}