using System.Collections.Generic;
using UnityEngine;

namespace Menu.Shop_Component
{
    [CreateAssetMenu(menuName = "Game/Product/Container", order = 51)]
    public class ProductContainer : ScriptableObject
    {
        public IReadOnlyList<PlayerProduct> Players => _players.AsReadOnly();
        public IReadOnlyList<TraktorProduct> PointLevels => _pointLevels.AsReadOnly();
        public TraktorProduct DefaultPointLevel;
        
        [SerializeField] private List<PlayerProduct> _players;
        [SerializeField] private List<TraktorProduct> _pointLevels;
    }
}