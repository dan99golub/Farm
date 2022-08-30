using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class FenceManager : MonoBehaviour
    {
        public event Action ViewUpdated;
        
        [ShowInInspector, ReadOnly] private HashSet<Fence> _fencer = new HashSet<Fence>();
        
        public void Add(Fence fence) => _fencer.Add(fence);

        public void Remove(Fence fence) => _fencer.Remove(fence);

        public void UpdateView()
        {
            _fencer.ForEach(x => x.UpdateFork());
            ViewUpdated?.Invoke();
        }
    }
}