using HungryCannibal.UnderTheSeaUIKit.ProgressBars;
using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit {
	[System.Serializable]
	public class ProgressBarControl : MonoBehaviour {

		public ProgressBar progressBar;
		public Button addButton;
		public Button subtractButton;
		public Button resetButton;
		public int amount = 10;

		private void Awake() {
			addButton.onClick.AddListener(OnAddClick);
			subtractButton.onClick.AddListener(OnSubtractClick);
			resetButton.onClick.AddListener(OnResetClick);
		}

		private void OnAddClick() {
			progressBar.IncrementValue(amount);
		}

		private void OnSubtractClick() {
			progressBar.IncrementValue(-amount);
		}

		private void OnResetClick() {
			progressBar.Reset();
		}
	}
}