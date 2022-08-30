using System;
using LoadScenes;
using ServiceScript;
using SO;
using UnityEngine;

namespace Menu.Shop_Component
{
    public class ViewPointContent : ViewProduct<PointLevel>
    {
        private MainMenuLoad Loader => Services<MainMenuLoad>.S.Get();
        
        private void Start()
        {
            Selected.DynamicCalls += () => Loader.SwitchPointLevel(Content.Content);
        }
    }
}