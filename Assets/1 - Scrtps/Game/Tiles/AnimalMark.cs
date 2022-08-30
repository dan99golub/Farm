using SO;
using UltEvents;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Game
{
    public class AnimalMark : MonoBehaviour
    {
        public AnimalID Id => id;
        public bool CanCagedByZone = true;
        public MoveMethodAnimal MoveMethod;
        public AddMoneyForResultGame Award;

        [SerializeField] private AnimalID id;

        [SerializeField] private UltEvent PreMoveToCage;
        [SerializeField] private UltEvent AnimalInCage;

        public void EventAnimalInCage() => AnimalInCage.Invoke();

        public void EventPreAnimalInCage() => PreMoveToCage.Invoke();

        public Vector2Int GetPositionOnMap() => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

        public void Stop(NavMeshAgent agent) => agent.SetDestination(transform.position);
    }
}