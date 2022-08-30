using System;
using System.Collections.Generic;
using DefaultNamespace.Game;
using Menu;
using Menu.Shop_Component;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;

namespace DTO
{
    [System.Serializable]
    public class Progress
    {
        private SignalBus _signals;
        public SignalBus Signals => _signals??=new SignalBus();

        public int Money => _money;
        
        [ReadOnly, SerializeField, JsonProperty] private List<string> _passLevel = new List<string>();
        [SerializeField, JsonProperty] private int _money;

        [SerializeField, JsonProperty] private string SelectedPointLevel;
        [SerializeField, JsonProperty] private ProductSave<PointLevel> _pointLevel = new ProductSave<PointLevel>();
        [SerializeField, JsonProperty] private string SelectedPlayer;
        [SerializeField, JsonProperty] private ProductSave<PlayerMark> _playerView = new ProductSave<PlayerMark>();

        [Button] public void AddPassLevel(Level l)
        {
            if(!_passLevel.Contains(l.GUID)) _passLevel.Add(l.GUID);
        }

        [Button] public void AddMoney(int count)
        {
            if (count < 0) return;
            _money += count;
        }

        public ProductSave<T> GetSaveProduct<T>() where T : MonoBehaviour
        {
            if (typeof(T) == typeof(PointLevel)) return _pointLevel as ProductSave<T>;
            if (typeof(T) == typeof(PlayerMark)) return _playerView as ProductSave<T>;
            throw new Exception("Запросили неизвестный тип продукта у сохранения");
        }

        public void SetSelectedGuidProduct<T>(Product<T> content) where T : MonoBehaviour
        {
            if (typeof(T) == typeof(PointLevel)) SelectedPointLevel = content.GUID;
            if (typeof(T) == typeof(PlayerMark)) SelectedPlayer = content.GUID;
        }
        
        public string GetSelectedGuidProduct<T>() where T : MonoBehaviour
        {
            if (typeof(T) == typeof(PointLevel)) return SelectedPointLevel;
            if (typeof(T) == typeof(PlayerMark)) return SelectedPlayer;
            throw new Exception("Запросили неизвестный тип продукта у сохранения");
        }
        
        private string GetSelectedPointLevelGUID() => SelectedPointLevel;

        [Button] public bool CanSpentMoney(int count) => count <= _money;

        [Button] public bool SpentMoney(int count)
        {
            var r = CanSpentMoney(count);
            if (r) _money -= count;
            return r;
        }

        [Button] public void RemoveLevel(Level l) => _passLevel.Remove(l.GUID);

        public bool LevelIsPass(Level l)
        {
            if (l) return _passLevel.Contains(l.GUID);else return false;
        }

        [Button] private void MakeUpdate() => Signals.Fire(new Updated());
    }
}