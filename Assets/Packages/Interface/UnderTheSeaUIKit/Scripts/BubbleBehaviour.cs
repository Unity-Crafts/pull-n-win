using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit {
	[System.Serializable]
	public class BubbleBehaviour : MonoBehaviour {
		public void OnAnimationComplete() {
			Destroy(gameObject);
		}
	}
}
