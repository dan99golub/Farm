using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class SwipeTrigger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Min(0)] public float _minDistance;
        
        public UltEvent Up; // Y
        public UltEvent Down; // Y
        public UltEvent Left; // x
        public UltEvent Right; // x
        private Vector2 _start;


        public void OnBeginDrag(PointerEventData eventData)
        {
            _start = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var end = eventData.position;
            if (!eventData.hovered.Contains(gameObject)) return;
            
            var xDelta = end.x - _start.x;
            var yDelta = end.y - _start.y;
            if (Mathf.Abs(xDelta) > Mathf.Abs(yDelta))
            {
                if (!(Mathf.Abs(xDelta) > _minDistance)) return;
                if(xDelta<0) Left.Invoke(); else Right.Invoke();
            }
            else
            {
                if (!(Mathf.Abs(yDelta) > _minDistance)) return;
                if(yDelta<0) Down.Invoke(); else Up.Invoke(); 
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Dont delete IDragHandler. Else other interface dont work. Wtf?!
        }
    }
}