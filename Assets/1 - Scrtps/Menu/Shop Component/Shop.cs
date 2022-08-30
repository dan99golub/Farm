using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using ServiceScript;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Menu.Shop_Component
{
    public class Shop : MonoBehaviour
    {
        public ViewPlayerContent PlayerViewPrefab;
        public ViewPointContent PointViewPrefab;
        public Transform PointContent;

        private ProductContainer Products => Services<ProductContainer>.S.Get();
        

        [Button]
        public void ShowPlayer()
        {
            Products.Players.ForEach(x => Instantiate(PlayerViewPrefab, PointContent).Init(x));
        }

        [Button]
        public void ShowPointLevel()
        {
            Products.PointLevels.ForEach(x => Instantiate(PointViewPrefab, PointContent).Init(x));
        }

        [Button]
        public void ClearView() => ClearChildren(PointContent);

        private void ClearChildren(Transform target)
        {
            target.GetComponentsInChildren<Transform>().Except(new[] {target}).Where(x => x.parent == target).ForEach(x => Destroy(x.gameObject));
        }
    }
}