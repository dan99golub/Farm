using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class IconBar : MonoBehaviour
    {
        [SerializeField] private Image _empty;
        [SerializeField] private Image _full;
        [SerializeField] private Transform _contentPoint;

        public void Set(int fullCount, int emptyCount)
        {
            DeleteAllChilds(_contentPoint);
            for (int i = 0; i < fullCount; i++) Instantiate(_full, _contentPoint);
            for (int i = 0; i < emptyCount; i++) Instantiate(_empty, _contentPoint);
        }
        
        private void DeleteAllChilds(Transform t)
        {
            t.GetComponentsInChildren<Transform>().Except(new[] {t}).ForEach(x => Destroy(x.gameObject));
        }
    }
}