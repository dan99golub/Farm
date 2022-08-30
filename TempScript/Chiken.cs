#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

namespace MaxyGames.Generated {
	public class ChikenAI : MaxyGames.RuntimeBehaviour {
		public DefaultNamespace.RandomPointOnField RandomPoint = null;
		private System.ValueTuple<Vector3, Vector2Int> _moveTo = new System.ValueTuple<Vector3, Vector2Int>() { Item1 = new Vector3(10f, 0f, 6f), Item2 = new Vector2Int() };
		public UnityEngine.AI.NavMeshAgent Agent = null;
		[Min(null)]
		public float DistanceReachPoint = 0.2F;
		public float WaitTime = 4F;
		public DefaultNamespace.SwitchEvent.EventStringSwitch Entered = null;
		private System.Action variable6;
		private MaxyGames.Runtime.EventCoroutine coroutine1 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine2 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine3 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine4 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine5 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine6 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine7 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine8 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine9 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine10 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine11 = new MaxyGames.Runtime.EventCoroutine();

		void Start() {
			coroutine1.Run();
		}

		void _ActivateTransition(string name) {
			switch(name) {
				case "HasPoint-1279246": {
					if(coroutine2.IsRunning) {
						coroutine2.Stop(true);
						coroutine3.Run();
					}
				}
				break;
				case "NoPoint-1279246": {
					if(coroutine2.IsRunning) {
						coroutine2.Stop(true);
						coroutine6.Run();
					}
				}
				break;
				case "PointReach-1279344": {
					if(coroutine3.IsRunning) {
						coroutine3.Stop(true);
						coroutine4.Run();
					}
				}
				break;
				case "End-1279422": {
					if(coroutine4.IsRunning) {
						coroutine4.Stop(true);
						coroutine2.Run();
					}
				}
				break;
			}
		}

		public override void OnAwake() {
			coroutine1.Setup(this, MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.Wait(0.1F), MaxyGames.Runtime.Routine.New(coroutine2)));
			coroutine2.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Search");
				RandomPoint.GetPoint();
				_moveTo = RandomPoint.GetPoint();
				return coroutine7.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine2.IsRunning;
			})));
			coroutine3.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine8.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine3.IsRunning;
			})));
			coroutine4.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Wait");
				CorutineGame.Instance.Wait(WaitTime, () => {
					_ActivateTransition("End-1279422");
				});
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine4.IsRunning;
			})));
			coroutine5.Setup(this, MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.Wait(0.25F), MaxyGames.Runtime.Routine.New(coroutine10)));
			coroutine6.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Sleep");
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine6.IsRunning;
			})));
			coroutine7.Setup(this, new MaxyGames.Runtime.Conditional(() => (RandomPoint.CurrentPosition == _moveTo.Item2), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("NoPoint-1279246");
			})), onFalse: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("HasPoint-1279246");
			}))));
			coroutine8.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Entered.Invoke("Move");
					return coroutine9.Run();
			}));
			coroutine9.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Agent.SetDestination(_moveTo.Item1);
					return coroutine10.Run();
			}));
			coroutine10.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position);
					return coroutine11.Run();
			}));
			coroutine11.Setup(this, new MaxyGames.Runtime.Conditional(() => (Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position) < DistanceReachPoint), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("PointReach-1279344");
			})), onFalse: coroutine5));
		}
	}
}
