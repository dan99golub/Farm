using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.AnimScripts;
using DefaultNamespace.Game;
using DefaultNamespace.Game.Plants;
using DTO;
using Game.Dirty;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using SO;
using UltEvents;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.SM
{
    public class S_Agresive : MonoBehaviour
    {
        public UltEvent PlayerFounded;
        public UltEvent OnEnab;
        public UltEvent ClampDown;
        public Rigidbody RB;
        
        public DataMove MoveDataBack;

        private Dir _lastDir;

        private bool CanShow => Application.isPlaying;

        private Field MainField => ServicesID<Field>.S.Get();
        private Level Level => Services<Level>.S.Get();
        private CacheField CacheOfField => Services<CacheField>.S.Get();

        private void OnEnable()
        {
            OnEnab.Invoke();
        }

        [System.Serializable]
        public class DataMove
        {
            public Dir Direction;

            public Vector3 VectorD;

            public void Init(Dir d, Vector3 vd)
            { 
                Direction = d;
                VectorD = vd;
            }
        }

        [Button]
        public void GetVectorMoveBack()
        {
            var d= GetDir();
            MoveDataBack.Init(d, GetNext(Vector3.zero, d));
        }

        public void GetRandomVectorMoveBack()
        {
            var d =new List<Dir>() {Dir.Left, Dir.Down, Dir.Right, Dir.Up}.Except(new[] {MoveDataBack.Direction}).GetRandom();
            MoveDataBack.Init(d, GetNext(Vector3.zero, d));
        }

        public IEnumerator StopMoveByVelocity(float timeCheck, float triggerVelocity, Action callback)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeCheck);
                if (RB.velocity.magnitude < triggerVelocity)
                {
                    callback();
                    yield break;
                }
            }
        }

        public IEnumerator StopBackMove(float timeCheck, float distance, Action callback)
        {
            Vector3 lastPos = RB.transform.position;
            yield return new WaitForSeconds(timeCheck);
            while (true)
            {
                yield return new WaitForSeconds(timeCheck);
                if (Vector3.Distance(lastPos, RB.transform.position) < distance)
                {
                    callback();
                    yield break;
                }
                lastPos = RB.transform.position;
            }
        }
        
        [Button]
        public void RotateToDirAttack()
        {
            transform.LookAt(transform.position+MoveDataBack.VectorD*-1);
        }

        private Dir GetDir()
        {
            Vector3 direction = transform.position+transform.forward - transform.position;
            Debug.Log($"Info get dir: Dir {direction}, abs x {Mathf.Abs(direction.x)} > abs z  {Mathf.Abs(direction.z)} = {Mathf.Abs(direction.x) > Mathf.Abs(direction.z)}");
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if (direction.x < 0) return Dir.Right;
                else return Dir.Left;
            }
            else
            {
                if (direction.z < 0) return Dir.Up;
                else return Dir.Down;
            }
        }
        
        public IEnumerator TryReplaceTile(float waitTime)
        {
            while (true)
            {
                var t = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position)));
                TryDestroyFence(t);
                yield return new WaitForSeconds(waitTime);
            }
        }
        
        public IEnumerator SetVelocity(Vector3 velocity, Func<bool> predictStop)
        {
            while (predictStop()==false && enabled)
            {
                yield return null;
                RB.velocity = velocity;    
            }
        }

        public Tile GetTileBehindYou()
        {
            _lastDir = GetDir();
            return TakeTileForMoveBack(_lastDir);
        }

        public Tile TakeTileForMoveBack(Dir d)
        {
            Vector3 startPos = Tile.ConvertGlobalToTilePos(transform.position, false);
            Vector3 nextPos = GetNext(startPos, d);
            Tile prevTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(startPos)));
            Tile nextTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(nextPos)));
            while (IsBlockMark(nextTile)==false)
            {
                if (IsTargetTile(nextTile)) return nextTile;
                prevTile = nextTile;
                nextTile = MainField.GetTile(Tile.GetGuid(Tile.GetTilePos(GetNext(nextTile.transform.position, d))));
            }

            return prevTile;
        }

        public void TryDestroyFence(Tile t)
        {
            if (t.Content != null)
            {
                if (t.Content.GetComponent<TileForReplaceBullMark>())
                {
                    ReplaceToDirty(t.Content);
                }
            }   
        }

        public void TryDestroyFence(Collider collider)
        {
            var inited = collider.GetComponent<InitedTile>();
            if (inited)
            {
                if (inited.GetComponent<TileForReplaceBullMark>())
                {
                    ReplaceToDirty(inited);
                }
                /*
                if (inited.Tile.GetComponent<BorderMark>() && inited.Tile.GetComponent<RoadMark>()==null)
                {
                    inited.Tile.ReplaceContent(Instantiate(Level.Ground));
                    TileFounded.Invoke(inited.Tile);
                }
                */
            }
        }

        private GreenManager Geens => Services<GreenManager>.S.Get();
        private void ReplaceToDirty(InitedTile inited)
        {
            var g = Instantiate(Level.Ground);
            inited.Tile.ReplaceContent(g);
            Geens.UpdateNeirBus(g);
        }

        public void TryFindPlayer(Collider col, AnimalMark me)
        {
            col.TryGetComponent<PlayerMark>(out var p);
            if(p && CacheOfField.CageManager.GetCage(x => x.HasAnimal(me)) == null)
                PlayerFounded.Invoke();
        }

        [Button]
        private void ManualFindPlayer() => PlayerFounded.Invoke();

        public void TurnOnClampDown() => ClampDown.Invoke();

        private bool IsBlockMark(Tile t)
        {
            if (t == null) return true;
            if (t.Content == null) return true;
            return t.Content.GetComponent<RoadMark>()!=null;
        }

        private bool IsTargetTile(Tile t) => t.Content.GetComponent<BorderMark>();

        private Vector3 GetNext(Vector3 start, Dir d)
        {
            switch (d)
            {
                case Dir.Down: return start + Vector3.forward * -1;
                case Dir.Up: return start + Vector3.forward;
                case Dir.Right: return start + Vector3.right;
                case Dir.Left: return start + Vector3.right * -1;
                default: return start;
            }
        }

        private Dir InvertDir(Dir d)
        {
            if (d == Dir.Up) return Dir.Down;
            else if (d == Dir.Down) return Dir.Up;
            else if (d == Dir.Right) return Dir.Left;
            else return Dir.Right;
        }

        public enum Dir { Up, Down, Right, Left }
        
    }
}