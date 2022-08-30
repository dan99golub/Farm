#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

namespace MaxyGames.Generated {
	public class ValidValueContainer : MaxyGames.RuntimeBehaviour {
		public DefaultNamespace.Game.ValueContainer SomeContainer = null;

		public int ValidFunc(System.ValueTuple<int, int, int, int> MinMaxOldNew) {
			return Mathf.Clamp(MinMaxOldNew.Item4, MinMaxOldNew.Item1, MinMaxOldNew.Item2);
		}

		void Start() {
			SomeContainer.SetMax(() => {
				return 25;
			});
			SomeContainer.SetMin(() => {
				return -10;
			});
			SomeContainer.SetValid(ValidFunc);
			SomeContainer.Current = SomeContainer.Current;
		}
	}
}
