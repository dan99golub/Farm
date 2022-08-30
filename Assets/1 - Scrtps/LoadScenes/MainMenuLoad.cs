using System;
using System.Linq;
using DTO;
using Menu;
using Menu.Shop_Component;
using ServiceScript;
using SO;
using UnityEngine;
using UnityEngine.Audio;

namespace LoadScenes
{
    public class MainMenuLoad : RegisterServicesScene
    {
        [Header("На регестрацию")]
        public C_MainMenu MenuController;
        public LevelSelector Selector;
        public UIMediatorMainMenu UIMediator;
        

        [Header("Другое")] public Transform PointViewVirtualCamera;
        private PointLevel _cureentPoint;

        private Progress Progress => Services<DB>.S.Get().Progress;
        private ProductContainer Products => Services<ProductContainer>.S.Get();

        public override void Register()
        {
            Services<C_MainMenu>.S.Set(MenuController);
            Services<LevelSelector>.S.Set(Selector);
            Services<UIMediatorMainMenu>.S.Set(UIMediator);
            Services<MainMenuLoad>.S.Set(this);

            var selectedPointLevel = Progress.GetSelectedGuidProduct<PointLevel>();
            var point = GetPoint(selectedPointLevel);
            SwitchPointLevel(point);
        }

        public void SwitchPointLevel(PointLevel prefab)
        {
            var point = Instantiate(prefab);
            PointViewVirtualCamera.SetParent(point.transform);
            PointViewVirtualCamera.localPosition = Vector3.zero;
            Services<PointLevel>.S.Set(point);

            if (_cureentPoint)
            {
                point.InstanceMove(_cureentPoint.CurrentView);
                point.transform.rotation = _cureentPoint.transform.rotation;
                Destroy(_cureentPoint.gameObject);
            }
            _cureentPoint = point;
        }

        private PointLevel GetPoint(string guid)
        {
            var product = Products.PointLevels.FirstOrDefault(x => x.GUID == guid);
            if (product) return product.Content;
            else return Products.DefaultPointLevel.Content;
        }

        public override void Unregister()
        {
            
        }
    }
}