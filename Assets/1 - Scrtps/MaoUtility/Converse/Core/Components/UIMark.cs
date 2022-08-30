using MaoUtility.Converse.Interfaces;
using UnityEngine;

namespace MaoUtility.Converse.Core.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class UIMark : BaseConverseComponent
    {
        public override string PrefixAlias => "UI";
    }
}