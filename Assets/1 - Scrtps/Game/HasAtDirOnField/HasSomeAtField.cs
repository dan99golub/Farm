using System;
using Menu;
using ServiceScript;
using SO;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace.Game.HasAtDirOnField
{
    public abstract class HasSomeAtField<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Field Main => ServicesID<Field>.S.Get();

        public Vector3 Dir;
        public float Lenght=0.5f;
        public bool IsGlobal = false;

        public UltEvent Has;
        public UltEvent HasNot;

        public void UpdateView()
        {
            var tile = Main.GetTile(Tile.GetGuid(Tile.GetTilePos(GetPoint())));
            if (tile == null)
            {
                InvokeEvent(false);
                return;
            }
            if (tile.Content == null)
            {
                InvokeEvent(false);
                return;
            }
            InvokeEvent(tile.Content.GetComponent<T>()!=null);
        }

        private void InvokeEvent(bool v)
        {
            if(v) Has.Invoke();
            else HasNot.Invoke();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, GetPoint());
        }

        private Vector3 GetPoint()
        {
            if (IsGlobal) return transform.position + Dir*Lenght;
            else return transform.position+ (transform.right*Dir.x + transform.forward * Dir.z)*Lenght;
        }
    }
}