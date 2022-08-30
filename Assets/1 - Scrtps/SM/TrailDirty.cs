using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Game;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class TrailDirty : MonoBehaviour
    {
        public GameObject Prefab;
        public float Delayed;
        public int Lenght;

        private List<Data> _dirtyPoint=new List<Data>();
        private float _passDelay;

        private void OnEnable()
        {
            _passDelay = Delayed+1;
        }

        private void Update()
        {
            _passDelay += Time.deltaTime;
            if (_passDelay > Delayed)
            {
                if (CanSpawn())
                {
                    SpawnDirty();
                    _passDelay = 0;
                }
            }
        }

        public void SpawnDirty()
        {
            TryDeleteLastDirty();
            var instacne = Instantiate(Prefab);
            instacne.transform.position = transform.position;
            _dirtyPoint.Add(new Data(instacne, Tile.GetTilePos(transform.position)));
        }

        private bool CanSpawn()
        {
            TryDeleteLastDirty();
            if (_dirtyPoint.Where(x => x.Pos == Tile.GetTilePos(transform.position)).Count() > 0)
                return false;
            return true;
        }

        public void TryDeleteLastDirty()
        {
            if (_dirtyPoint.Count() >= Lenght)
            {
                var obj = _dirtyPoint[0];
                _dirtyPoint.RemoveAt(0);
                if(obj.InstanceDirty!=null) Destroy(obj.InstanceDirty.gameObject);
            }
        }

        [System.Serializable]
        public class Data
        {
            public GameObject InstanceDirty;
            public Vector2Int Pos;

            public Data(GameObject instance, Vector2Int pos)
            {
                InstanceDirty = instance;
                instance.transform.position = new Vector3(pos.x, 0, pos.y);
                Pos = pos;
            }
        }
    }
}