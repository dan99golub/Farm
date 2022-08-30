using System;
using DG.Tweening;
using ServiceScript;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.SM
{
    public class HelpAI : MonoBehaviour
    {
        public void ThrowDirty(GameObject prefab, MoveItemOnAir move, RandomPointOnField point, Ease e, float duration)
        {
            var d = Instantiate(prefab);
            var p = point.GetPoint();
            move.Move(transform.position, p.movePos, e, duration, d.transform);
        }

        public void StopNavMeshAgent(NavMeshAgent agent) => agent.SetDestination(transform.position);

        public void ChangeSizeCollider(BoxCollider box, Vector3 newSize)
        {
            var oldSize = box.size;
            box.size = newSize;
            CorutineGame.Instance.WaitFrame(2,()=> box.size = oldSize);
        }
    }
}