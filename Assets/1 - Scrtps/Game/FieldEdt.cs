using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FieldEdt : MonoBehaviour
    {
        private Field _field;
        public Field Field => _field ??= GetComponent<Field>();
    }
}