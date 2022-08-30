using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerCheckMover : MonoBehaviour
    {
        public BoxCollider Trigger => _trigger ??= GetComponent<BoxCollider>();
        [SerializeField, ReadOnly]private BoxCollider _trigger;
        public event Action<GameObject> Damaged;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<DamagePathMark>(out var r)) Damaged?.Invoke(r.gameObject);
        }

        [Button] private void ManualValidate()
        {
            if(Application.isPlaying) return;
            Trigger.isTrigger = true;
            Trigger.size = new Vector3(1,4,1);
            Trigger.center = Vector3.zero;
        }
    }
}