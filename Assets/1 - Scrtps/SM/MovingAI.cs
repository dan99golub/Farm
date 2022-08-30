using System;
using Menu;
using ServiceScript;
using UltEvents;
using Unity.Collections;

namespace DefaultNamespace.SM
{
    public class MovingAI : BaseAI
    {
        public SearchPoint SearchPointAct;
        public MoveToPoint MoveAct;
        public IsRichPoint RichPoint;
        public float WaitTime;
        public float TimeCheckFinish;
        public float DistanceFinish;

        public State SearchPoint;
        public State Move;
        public State Wait;
        public State Sleep;

        public UltEvent PointIsRich;

        private void Start() => InitStates();

        private void OnEnable() => CorutineGame.Instance.WaitFrame(2, () => ChangeStaete(SearchPoint));
        
        private void InitStates()
        {
            SearchPoint.Entered.DynamicCalls += () =>
            {
                SearchPointAct.Act();
                MoveAct.PointForMove = RichPoint.Point = SearchPointAct.NewPoint.movePos;
                if(SearchPointAct.HasPoint) ChangeStaete(Move);
                else ChangeStaete(Sleep);
            };
            Move.Entered.DynamicCalls += () =>
            {
                MoveAct.Act();
                CheckMove(()=>_currentState==Move);
            };
            Wait.Entered.DynamicCalls += () => _corutines.Add(CorutineGame.Instance.Wait(WaitTime, () => ChangeStaete(SearchPoint)));
            Sleep.Entered.DynamicCalls += ()=> _corutines.Add(CorutineGame.Instance.Wait(WaitTime, () => ChangeStaete(SearchPoint)));
        }

        private void CheckMove(Func<bool> predictNextTry)
        {
            RichPoint.Act();
            if(predictNextTry()==false || enabled==false) return;
            if(RichPoint.IsRich) PointIsRich.Invoke();
            else _corutines.Add(CorutineGame.Instance.Wait(TimeCheckFinish, ()=>CheckMove(predictNextTry)));
        }
    }
}