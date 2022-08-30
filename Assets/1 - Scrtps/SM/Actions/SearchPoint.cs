using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.SM
{
    [System.Serializable]
    public class SearchPoint : ActionAI
    {
        public RandomPointOnField RandomPoint;

        [ShowInInspector] [ReadOnly]  public (Vector3 movePos, Vector2Int posTile) NewPoint { get; private set; }
        [ShowInInspector] [ReadOnly] public bool HasPoint { get; private set; }
        
        public override void Act()
        {
            NewPoint = RandomPoint.GetPoint();
            HasPoint = NewPoint.posTile != RandomPoint.CurrentPosition;
        }
    }
}