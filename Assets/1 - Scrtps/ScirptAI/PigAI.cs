using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.ScirptAI
{
    public class PigAI : MonoBehaviour
    {
        public RandomPointOnField randomPointOnField;
        public NavMeshAgent Agent;
        [Min(0)] public float WaitTime;
        [Min(0)] public float DistanceForFinishMove;
    }
}