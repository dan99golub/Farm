using System;
using System.Collections.Generic;
using DefaultNamespace.Game;
using Menu;
using ServiceScript;
using UltEvents;
using UnityEngine;

namespace DefaultNamespace
{
    public class ZoneManagerReactor : MonoBehaviour
    {
        public UltEvent NewZones;
        
        private ZoneManager Zones => Services<ZoneManager>.S.Get();

        private void Awake() => Zones.NewZones += OnNewZone;

        public void ReinitZones() => Zones.ReInit();

        private void OnNewZone(List<ZoneManager.Zone> obj) => CorutineGame.Instance.WaitFrame(1, () => NewZones.Invoke());
    }
}