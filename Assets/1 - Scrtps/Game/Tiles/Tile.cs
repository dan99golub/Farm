using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    [RequireComponent(typeof(ProgressMark))]
    public class Tile : MonoBehaviour
    {
        public Vector2Int Position => _position;
        public string GUID => GetGuid(Position);
        
        [SerializeField] private Vector2Int _position;
        [SerializeField, ReadOnly]private InitedTile _content;
        
        public InitedTile Content => _content;

        private ProgressMark _mark;

        public ProgressMark Mark
        {
            get
            {
                if (!_mark) _mark = GetComponent<ProgressMark>();
                if (!_mark) _mark = gameObject.AddComponent<ProgressMark>();
                return _mark;
            }
        }
        private Field _filed;
        public Field Field => _filed ??= GetComponentInParent<Field>();
        
        public void Init()
        {
            Mark.Set(ProgressMark.State.Ignore);
            Content?.Init(this);
        }


        public static string GetGuid(Vector2Int pos) => $"X{pos.x}Y{pos.y}";
        
        public static Vector2Int GetTilePos(Vector3 globalPos) => new Vector2Int(Mathf.RoundToInt(globalPos.x), Mathf.RoundToInt(globalPos.z));

        public Collider[] Overlap(LayerMask mask) => Physics.OverlapBox(transform.position, new Vector3(1,6,1) / 2, Quaternion.identity, mask);

        public void InitEditor(Vector2Int pos)
        {
            _position = pos;
        }

        public void ReplaceContent(InitedTile instantiate)
        {
            if(_content==instantiate) return;
            
            instantiate.transform.position = transform.position;
            instantiate.transform.SetParent(transform);
            if (Content)
            {
                if(Application.isPlaying) Destroy(Content.gameObject);
                else DestroyImmediate(Content.gameObject);
            }
            _content = instantiate;
            if(Application.isPlaying) Init();
        }

        public static Vector3 ConvertGlobalToTilePos(Vector3 global, bool saveY)
        {
            var v2 = GetTilePos(global);
            return new Vector3(v2.x, saveY? global.y : 0, v2.y);
        }
    }
}