#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using UltEvents;

namespace MaxyGames.Generated {
	public class ChikenAI : MaxyGames.RuntimeBehaviour {
		public DefaultNamespace.RandomPointOnField RandomPoint;
		private System.ValueTuple<Vector3, Vector2Int> _moveTo = new System.ValueTuple<Vector3, Vector2Int>() { Item1 = Vector3.zero, Item2 = new Vector2Int() };
		public UnityEngine.AI.NavMeshAgent Agent;
		[Min(0F)]
		public float DistanceReachPoint = 0.5F;
		public float WaitTime = 4F;
		public DefaultNamespace.SwitchEvent.EventStringSwitch Entered;
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

		public bool IsRich() {
			Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position);
			return (Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position) < DistanceReachPoint);
		}

		void Start() {
			coroutine1.Run();
		}

		System.Collections.IEnumerable _ExecuteCoroutineEvent(int uid) {
			switch(uid) {
				case -7812: {
					yield return new WaitForSeconds(0.25F);
					yield return coroutine11.Run();
				}
				break;
			}
			yield break;
		}

		void _ActivateTransition(string name) {
			switch(name) {
				case "HasPoint-7666": {
					if(coroutine2.IsRunning) {
						coroutine2.Stop(true);
						coroutine3.Run();
					}
				}
				break;
				case "NoPoint-7666": {
					if(coroutine2.IsRunning) {
						coroutine2.Stop(true);
						coroutine5.Run();
					}
				}
				break;
				case "PointReach-7764": {
					if(coroutine3.IsRunning) {
						coroutine3.Stop(true);
						coroutine4.Run();
					}
				}
				break;
				case "End-7818": {
					if(coroutine4.IsRunning) {
						coroutine4.Stop(true);
						coroutine2.Run();
					}
				}
				break;
			}
		}

		public override void OnAwake() {
			coroutine1.Setup(this, MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.Wait(0.25F), MaxyGames.Runtime.Routine.New(coroutine2)));
			coroutine2.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Search");
				RandomPoint.GetPoint();
				_moveTo = RandomPoint.GetPoint();
				return coroutine6.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine2.IsRunning;
			})));
			coroutine3.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine7.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine3.IsRunning;
			})));
			coroutine4.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine8.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine4.IsRunning;
			})));
			coroutine5.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Sleep");
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine5.IsRunning;
			})));
			coroutine6.Setup(this, new MaxyGames.Runtime.Conditional(() => (RandomPoint.CurrentPosition == _moveTo.Item2), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("NoPoint-7666");
			})), onFalse: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("HasPoint-7666");
			}))));
			coroutine7.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Entered.Invoke("Move");
					Agent.SetDestination(_moveTo.Item1);
					return coroutine9.Run();
			}));
			coroutine8.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Entered.Invoke("Wait");
					return new WaitForSeconds(WaitTime);
					_ActivateTransition("End-7818");
			}));
			coroutine9.Setup(this, new MaxyGames.Runtime.Conditional(() => IsRich(), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("PointReach-7764");
			})), onFalse: coroutine10));
			coroutine10.Setup(this, _ExecuteCoroutineEvent(-7812));
			coroutine11.Setup(this, new MaxyGames.Runtime.Conditional(() => IsRich(), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("PointReach-7764");
			})), onFalse: coroutine10));
		}
	}
}
