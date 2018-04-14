using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	public abstract class ProgressBar : UIBehaviour {

		public const string TEXT_PERCENT = "{PERCENT}";
		public const string TEXT_VALUE = "{VALUE}";
		public const string TEXT_MINVALUE = "{MINVALUE}";
		public const string TEXT_MAXVALUE = "{MAXVALUE}";

		[System.Serializable]
		public class ValueChangedEvent : UnityEvent<ProgressBar> { }

		#region Properties
		public RectTransform rectTransform {
			get { return transform as RectTransform; }
		}

		public float minValue {
			get { return _minValue; }
			set {
				_minValue = Mathf.Clamp(value, int.MinValue, maxValue);
				percent = CalculatePercent(minValue, maxValue, _value);
				OnValueChanged.Invoke(this);
			}
		}

		[SerializeField]
		private float _minValue = 0;

		public float maxValue {
			get { return _maxValue; }
			set {
				_maxValue = Mathf.Clamp(value, minValue, int.MaxValue);
				percent = CalculatePercent(minValue, maxValue, _value);
				OnValueChanged.Invoke(this);
			}
		}

		[SerializeField]
		private float _maxValue = 100;

		public float value {
			get { return _value; }
			set {
				_value = Mathf.Clamp(value, minValue, maxValue);
				percent = CalculatePercent(minValue, maxValue, _value);
				OnValueChanged.Invoke(this);
			}
		}

		[SerializeField]
		private float _value = 0;

		public virtual float percent {
			get { return _percent; }
			set {
				_percent = Mathf.Clamp01(value);
				_value = CalculateValue(minValue, maxValue, _percent);
				OnValueChanged.Invoke(this);
			}
		}

		[SerializeField]
		private float _percent = 0;

		public ProgressTextType progressTextType {
			get { return _progressTextType; }
			set {
				_progressTextType = value;
				UpdateProgressText();
			}
		}

		[SerializeField]
		private ProgressTextType _progressTextType;

		public string progressText {
			get { return _progressText; }
			set {
				_progressText = value;
				UpdateProgressText();
			}
		}

		[SerializeField]
		private string _progressText = string.Empty;

		[SerializeField]
		private string _customProgressText = string.Empty;
		#endregion

		[HideInInspector]
		public Text progressLabel;

		[HideInInspector]
		public Image fillBar;

		private Coroutine _incrementValueCoroutine = null;
		private bool _animating = false;
		private float _currentIncrementAmount = 0;
		private float _currentIncrementAmountProgress = 0;

		public ValueChangedEvent OnValueChanged;

		/// <summary>
		/// Calculates a percentage between minValue/maxValue based on a value
		/// </summary>
		/// <param name="minValue">The minimum value (at 0%)</param>
		/// <param name="maxValue">The maximum value (at 100%)</param>
		/// <param name="value">A value between minValue and maxValue</param>
		/// <returns>A value between 0 and 1 representing the percent 'value' is between 'minValue' and 'maxValue'</returns>
		public static float CalculatePercent(float minValue, float maxValue, float value) {
			return Mathf.Abs(((100f / (maxValue - minValue)) * (minValue - value))) / 100f;
		}

		/// <summary>
		/// Calculates a value between minValue/maxValue based on a percentage
		/// </summary>
		/// <param name="minValue">The minimum value (at 0%)</param>
		/// <param name="maxValue">The maximum value (at 100%)</param>
		/// <param name="percent">The percent between 'minValue' and 'maxValue'</param>
		/// <returns>A value 'percent' between minValue and maxValue</returns>
		public static float CalculateValue(float minValue, float maxValue, float percent) {
			return minValue + ((maxValue - minValue) * percent);
		}

		/// <summary>
		/// Updates the layout of the progressbar when something has changed
		/// </summary>
		public virtual void UpdateLayout() { }

		/// <summary>
		/// Updates the progress bar text label if it has one
		/// </summary>
		public void UpdateProgressText() {
			//Do nothing if we couldnt find the label
			if(progressLabel == null) return;

			//If there is no progress text, then deactivate the label if its active
			if(_progressTextType == ProgressTextType.None) {
				if(progressLabel.gameObject.activeSelf) {
					progressLabel.gameObject.SetActive(false);
				}
				return;
			}

			//We need to show progress text, so activate the label if its not active
			if(!progressLabel.gameObject.activeSelf) {
				progressLabel.gameObject.SetActive(true);
			}

			//Set the text based on the required type
			switch(_progressTextType) {
				case ProgressTextType.Percent: {
					_progressText = string.Format("{0}%", Mathf.RoundToInt(_percent * 100f));
					break;
				}
				case ProgressTextType.Value: {
					_progressText = Mathf.RoundToInt(_value).ToString();
					break;
				}
				case ProgressTextType.ValueAndTotal: {
					_progressText = string.Format("{0}/{1}", Mathf.RoundToInt(_value), Mathf.RoundToInt(maxValue));
					break;
				}
				case ProgressTextType.Text: {
					_progressText = _customProgressText;
					break;
				}
			}

			progressLabel.text = _progressText.Replace(TEXT_PERCENT, Mathf.RoundToInt(_percent * 100f).ToString())
												.Replace(TEXT_VALUE, Mathf.RoundToInt(_value).ToString())
												.Replace(TEXT_MINVALUE, Mathf.RoundToInt(minValue).ToString())
												.Replace(TEXT_MAXVALUE, Mathf.RoundToInt(maxValue).ToString());
		}

		protected override void Awake() {
			//Hierarchy position of the progress label changes depending on progressTextPosition
			progressLabel = transform.Find("Container/Bar/Percent Label").GetComponent<Text>();
			fillBar = transform.Find("Container/Bar").GetComponent<Image>();

			OnValueChanged.AddListener(ProgressBar_OnValueChanged);
			value = _value;

			base.Awake();
		}

		protected virtual void ProgressBar_OnValueChanged(ProgressBar bar) {
			//Update the progress label
			UpdateProgressText();

			//Set the fill amount of the fill bar
			if(fillBar != null) {
				fillBar.fillAmount = _percent;
			}
		}

		/// <summary>
		/// Resets the progress bar back to 0
		/// </summary>
		/// <param name="animated">If true, the slider will slide to 0.  Otherwise, the slider will instantly jump to 0</param>
		public void Reset(bool animated = true) {
			//If we are animating, then we must stop first!
			StopAnimating();

			//Remember this increment amount and reset the progress of this animation
			_currentIncrementAmount = -CalculateValue(minValue, maxValue, percent);
			_currentIncrementAmountProgress = 0;

			//Reset progress to minValue, either using animation or just setting the value
			if(animated) {
				_incrementValueCoroutine = StartCoroutine(IncrementValueEnumerator(_currentIncrementAmount));
			} else {
				value = minValue;
			}
		}

		/// <summary>
		/// Increment the progress value
		/// </summary>
		/// <param name="amount">The amount to increment (can be negative to decrement)</param>
		/// <param name="animated">If true, the slider will slide to the new position.  Otherwise, the slider will instantly jump to the new position</param>
		public void IncrementValue(float amount, bool animated = true) {
			//If we are animating, we need to stop animating and add the difference added in the current animation to the passed amount
			//This allows this method to be called multiple times before the animation has completed
			if(_animating) {
				StopAnimating();
				amount += (_currentIncrementAmount - _currentIncrementAmountProgress);
			}

			//Remember this increment amount and reset the progress of this animation
			_currentIncrementAmount = amount;
			_currentIncrementAmountProgress = 0;

			//Update progress either using animation or just setting the value
			if(animated) {
				_incrementValueCoroutine = StartCoroutine(IncrementValueEnumerator(amount));
			} else {
				value = Mathf.Clamp(value + amount, minValue, maxValue);
			}
		}

		/// <summary>
		/// Co-routine to animate the progress slider
		/// </summary>
		/// <param name="amount">The amount to add to the value</param>
		/// <returns>An IEnumerator to yield</returns>
		protected IEnumerator IncrementValueEnumerator(float amount) {
			//Calculate the total at the end of the animation
			float total = Mathf.Clamp(value + amount, minValue, maxValue);
			_animating = true;

			//If we are incrementing
			if(amount > 0) {
				while(value < total && _animating) {
					_currentIncrementAmountProgress += (amount * Time.deltaTime);
					value = Mathf.Clamp(value + (amount * Time.deltaTime), minValue, total);
					yield return null;
				}
			}
			//If we are decrementing
			else {
				while(value > total && _animating) {
					_currentIncrementAmountProgress += (amount * Time.deltaTime);
					value = Mathf.Clamp(value + (amount * Time.deltaTime), total, maxValue);
					yield return null;
				}
			}
			_animating = false;
		}

		/// <summary>
		/// Stops all animation
		/// </summary>
		private void StopAnimating() {
			//Stop all animations/coroutines
			_animating = false;
			if(_incrementValueCoroutine != null) {
				StopCoroutine(_incrementValueCoroutine);
			}
		}
	}
}
