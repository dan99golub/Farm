using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpContainer : MonoBehaviour
    {
        public event Action<PowerUp.Data> Added;
        public event Action<PowerUp.Data> Removed;

        [ShowInInspector] private Dictionary<PowerUp, PowerUp.Data> _powerUps = new Dictionary<PowerUp, PowerUp.Data>();

        [Button] 
        public void TryAdd(PowerUp.Data data)
        {
            if(_powerUps.ContainsKey(data.TypePowerUp)) return;
            _powerUps.Add(data.TypePowerUp, data);
            Added?.Invoke(data);
        }

        
        private List<PowerUp> _toDelete = new List<PowerUp>();
        private void Update()
        {
            _powerUps.Values.ForEach(x =>
            {
                x.Update();
                if (x.LifePowerUp.IsEnd()) 
                    _toDelete.Add(x.TypePowerUp);
            });
            if (_toDelete.Count > 0)
            {
                _toDelete.ForEach(x => Remove(x));
                _toDelete.Clear();
            }
        }

        public PowerUp.Data GetData(PowerUp up)
        {
            _powerUps.TryGetValue(up, out var r);
            return r;
        }

        public bool HasPowerUp(PowerUp powerUp) => _powerUps.ContainsKey(powerUp);
        
        [Button] 
        public void Remove(PowerUp powerUp)
        {
            if (_powerUps.TryGetValue(powerUp, out var data))
            {
                _powerUps.Remove(powerUp);
                Removed?.Invoke(data);
            }
        }

        public void Replace(PowerUp.Data data)
        {
            Remove(data.TypePowerUp);
            TryAdd(data);
        }
    }
}