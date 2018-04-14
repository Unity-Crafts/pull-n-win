using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	public class CounterBar : UIBehaviour {

		[System.Serializable]
		public class CounterBarPlusClick : UnityEvent { }

		/// <summary>
		/// The speed of the count animation
		/// </summary>
		public float countSpeed = 1;

		/// <summary>
		/// The current count value
		/// </summary>
		public float count {
			get { return _count; }
			set {
				_count = value;
				if(_counter != null) {
					_counter.text = Mathf.RoundToInt(_count).ToString("N0");
				}
			}
		}

		[SerializeField]
		private float _count;

		private Text _counter;
		private bool _animating = false;

		/// <summary>
		/// Event which is fired when the plus button is clicked
		/// </summary>
		public CounterBarPlusClick onPlusClick;

		/// <summary>
		/// Awake called by Unity
		/// </summary>
		protected override void Awake() {
			_counter = transform.Find("Count").GetComponent<Text>();

			var plusButton = transform.GetComponentInChildren<Button>();
			if(plusButton != null)
			{
				plusButton.onClick.AddListener(OnPlusButtonClick);
			}

			count = _count;

			base.Awake();
		}

		/// <summary>
		/// Event when the plus button is clicked
		/// </summary>
		private void OnPlusButtonClick() {
			onPlusClick.Invoke();
		}

		/// <summary>
		/// Increment the counter by an amount
		/// </summary>
		/// <param name="amount">The amount to increment, to decrement pass a negative value</param>
		/// <param name="animated">If true, the counter value will count up gradually.  Otherwise, the value will be set immediatly</param>
		public void IncrementCount(int amount, bool animated = true) {
			if(animated) {
				StartCoroutine(IncrementCountEnumerator(amount));
			} else {
				count += amount;
			}
		}

		/// <summary>
		/// Enumerator to animate the counter count
		/// </summary>
		/// <param name="amount">The amount to count up or down</param>
		/// <returns>An IEnumerator to yield</returns>
		private IEnumerator IncrementCountEnumerator(int amount) {
			//Calculate the total at the end of the animation
			float total = count + amount;
			_animating = true;

			//If we are incrementing
			if(amount > 0) {
				while(count < total && _animating) {
					count += amount * (Time.deltaTime * countSpeed);
					yield return null;
				}
			}
			//If we are decrementing
			else {
				while(count > total && _animating) {
					count += amount * (Time.deltaTime * countSpeed);
					yield return null;
				}
			}
			_animating = false;
		}
	}
}
