using DTO;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game
{
    public class AddMoneyForResultGame : MonoBehaviour
    {
        [Min(0)]public int Money;

        private C_GameScene Controller => Services<C_GameScene>.S.Get();
        
        [Button]
        public void Add() => Add(Money);

        [Button]
        public void Add(int count)
        {
            Controller.Result.NewMoney += count;
            Controller.Result.Events.Fire(new Updated());
        }
    }
}