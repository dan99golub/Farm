#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using UltEvents;

namespace MaxyGames.Generated {
	public class BullAI : MaxyGames.RuntimeBehaviour {
		public DefaultNamespace.RandomPointOnField RandomPoint;
		private System.ValueTuple<Vector3, Vector2Int> _moveTo = new System.ValueTuple<Vector3, Vector2Int>() { Item1 = Vector3.zero, Item2 = new Vector2Int() };
		public UnityEngine.AI.NavMeshAgent Agent;
		[Min(0F)]
		public float DistanceReachPoint = 0.5F;
		public float WaitTime = 4F;
		public DefaultNamespace.SwitchEvent.EventStringSwitch Entered;
		public TriggerEvents3D TriggerSearch;
		private Vector3 _positionPlayerOnFound = Vector3.zero;
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
		private MaxyGames.Runtime.EventCoroutine coroutine12 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine13 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine14 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine15 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine16 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine17 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine18 = new MaxyGames.Runtime.EventCoroutine();
		private MaxyGames.Runtime.EventCoroutine coroutine19 = new MaxyGames.Runtime.EventCoroutine();

		public bool IsRich() {
			Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position);
			return (Vector3.Distance(_moveTo.Item1, Agent.gameObject.transform.position) < DistanceReachPoint);
		}

		void Start() {
			coroutine19.Run();
		}

		System.Collections.IEnumerable _ExecuteCoroutineEvent(int uid) {
			switch(uid) {
				case -688682: {
					yield return new WaitForSeconds(0.25F);
					yield return coroutine16.Run();
				}
				break;
			}
			yield break;
		}

		void _ActivateTransition(string name) {
			switch(name) {
				case "StartRun-192704": {
					if(coroutine1.IsRunning) {
						coroutine1.Stop(true);
						coroutine2.Run();
					}
				}
				break;
				case "Attack-194494": {
					if(coroutine2.IsRunning) {
						coroutine2.Stop(true);
						coroutine3.Run();
					}
				}
				break;
				case "ReapetAttack-195982": {
					if(coroutine3.IsRunning) {
						coroutine3.Stop(true);
						coroutine3.Run();
					}
				}
				break;
				case "CalmDown-195982": {
					if(coroutine3.IsRunning) {
						coroutine3.Stop(true);
						coroutine12.Run();
					}
				}
				break;
				case "HasPoint-189150": {
					if(coroutine4.IsRunning) {
						coroutine4.Stop(true);
						coroutine5.Run();
					}
				}
				break;
				case "NoPoint-189150": {
					if(coroutine4.IsRunning) {
						coroutine4.Stop(true);
						coroutine7.Run();
					}
				}
				break;
				case "PointReach-189248": {
					if(coroutine5.IsRunning) {
						coroutine5.Stop(true);
						coroutine6.Run();
					}
				}
				break;
				case "End-189314": {
					if(coroutine6.IsRunning) {
						coroutine6.Stop(true);
						coroutine4.Run();
					}
				}
				break;
			}
		}

		public override void OnAwake() {
			coroutine1.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine11.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine1.IsRunning;
			})));
			coroutine2.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine2.IsRunning;
			})));
			coroutine3.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine3.IsRunning;
			})));
			coroutine4.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Search");
				RandomPoint.GetPoint();
				_moveTo = RandomPoint.GetPoint();
				return coroutine13.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine4.IsRunning;
			})));
			coroutine5.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine14.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine5.IsRunning;
			})));
			coroutine6.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				coroutine15.Run();
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine6.IsRunning;
			})));
			coroutine7.Setup(this, 
			MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.New(() => {
				Entered.Invoke("Sleep");
			}), MaxyGames.Runtime.Routine.WaitWhile(() => {
				return coroutine7.IsRunning;
			})));
			coroutine8.Setup(this, MaxyGames.Runtime.Routine.New(MaxyGames.Runtime.Routine.Wait(0.25F), MaxyGames.Runtime.Routine.New(coroutine4)));
			coroutine9.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					_positionPlayerOnFound = parameters.transform.position;
					return coroutine10.Run();
			}));
			coroutine10.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					TriggerSearch.TriggerEnterEvent.DynamicCalls -= (Collider parameters) => {
						coroutine9.Run();
					};
					return coroutine1.Run();
			}));
			coroutine11.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Agent.transform.LookAt(_positionPlayerOnFound, Vector3.up);
					return coroutine18.Run();
			}));
			coroutine12.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					TriggerSearch.TriggerEnterEvent.DynamicCalls += (Collider parameters) => {
						coroutine9.Run();
					};
					return coroutine4.Run();
			}));
			coroutine13.Setup(this, new MaxyGames.Runtime.Conditional(() => (RandomPoint.CurrentPosition == _moveTo.Item2), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("NoPoint-189150");
			})), onFalse: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("HasPoint-189150");
			}))));
			coroutine14.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Entered.Invoke("Move");
					Agent.SetDestination(_moveTo.Item1);
					return coroutine16.Run();
			}));
			coroutine15.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					Entered.Invoke("Wait");
					return new WaitForSeconds(WaitTime);
					_ActivateTransition("End-189314");
			}));
			coroutine16.Setup(this, new MaxyGames.Runtime.Conditional(() => IsRich(), onTrue: MaxyGames.Runtime.EventCoroutine.Create(this, MaxyGames.Runtime.Routine.New(() => {
				_ActivateTransition("PointReach-189248");
			})), onFalse: coroutine17));
			coroutine17.Setup(this, _ExecuteCoroutineEvent(-688682));
			coroutine18.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					return new WaitForEndOfFrame();
					null.AddForce((base.transform.forward * -1F), ForceMode.Impulse);
			}));
			coroutine19.Setup(this, MaxyGames.Runtime.Routine.New(() => {
					TriggerSearch.TriggerEnterEvent.DynamicCalls += (Collider parameters) => {
						coroutine9.Run();
					};
					return coroutine8.Run();
			}));
		}
	}
}
