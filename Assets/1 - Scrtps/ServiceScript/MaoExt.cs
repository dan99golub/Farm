using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ServiceScript
{
    public static class MaoExt
    {
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            var l = collection.ToList();
            return l[Random.Range(0, l.Count)];
        }
    }
}