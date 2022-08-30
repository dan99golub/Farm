using System.Collections.Generic;
using System.Linq;
using Menu;
using ServiceScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Game.Plants
{
    public class LineDistibutePlant : PlantDistribute
    {
        [Min(1)]public int MinLine;
        [Min(1)]public int MaxLine;
        public bool IsHorizontal;

        public GameObject[] ListPlant;

        [ReadOnly, ShowInInspector] private List<Data> Datas = new List<Data>();

        private void Awake()
        {
            Datas.Add(new Data(0, UnityEngine.Random.Range(MinLine, MaxLine), ListPlant.GetRandom()));
            CalculateTo(50, Datas.Last());
        }

        public override GameObject GetPlant(Vector2Int posTile)
        {
            var first = Datas.FirstOrDefault(x => x.IsCorrect(GetPosition(posTile)));
            if (first == null)
            {
                CalculateTo(GetPosition(posTile), Datas.Last());
                return GetPlant(posTile);
            }
            return first.Plant;
        }

        private void CalculateTo(int getPosition, Data last)
        {
            int currentPos = last.EndLine + 1;
            while (currentPos<getPosition)
            {
                var realLast = Datas.Last();
                int nextPos = currentPos + UnityEngine.Random.Range(MinLine, MaxLine);
                Datas.Add(new Data(currentPos, nextPos, ListPlant.Except(new []{realLast.Plant}).GetRandom()));
                currentPos = nextPos + 1;
            }
        }

        private int GetPosition(Vector2Int vector)
        {
            if (IsHorizontal) return vector.y;
            else return vector.x;
        }

        public class Data
        {
            public int StartLine;
            public int EndLine;
            public GameObject Plant;

            public Data(int start, int end, GameObject plant)
            {
                StartLine = start;
                EndLine = end;
                Plant = plant;
            }
            
            public bool IsCorrect(int pos) => pos >= StartLine && pos <= EndLine;
        }

        private void OnValidate()
        {
            if (MaxLine <= MinLine) MaxLine = MinLine + 1;
        }
    }
}