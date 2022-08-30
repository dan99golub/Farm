using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using SO;
using UnityEngine;

namespace DefaultNamespace.SM
{
    public class TrialGrass : MonoBehaviour
    {
        public float Delayed;

        private float _passDelay;

        private Level Level => Services<Level>.S.Get();
        private Field Field => ServicesID<Field>.S.Get();
        private ProgressLevel Progress => Services<ProgressLevel>.S.Get();

        private void OnEnable() => _passDelay = Delayed + 1;

        private void Update()
        {
            _passDelay += Time.deltaTime;
            if (_passDelay > Delayed)
            {
                var tile = GetTile();
                if (CanSpawn(tile))
                {
                    SetGreen(tile);
                    _passDelay = 0;
                }
            }
        }

        private void SetGreen(Tile tile)
        {
            tile.ReplaceContent(Instantiate(Level.Green));
            Progress.CalculateProgress();
        }

        private Tile GetTile() => Field.GetTile(Tile.GetGuid(Tile.GetTilePos(transform.position)));
        

        private bool CanSpawn(Tile t)
        {
            if (t.Content == null) return false;
            if (t.Content.GetComponent<BorderMark>()) return false;
            return true;
        }
    }
}