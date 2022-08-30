using System;
using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class Prop : MonoBehaviour
    {
        private Field Main => ServicesID<Field>.S.Get();

        private void Start()
        {
            if(Main!=null)
                CorutineGame.Instance.WaitFrame(1, () =>
                {
                    var content = Main.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position))).Content;
                    transform.SetParent(content ? content.transform : null);
                });
        }
    }
}