using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.OnOffSwitches {
	[System.Serializable]
	[ExecuteInEditMode]
	public class OnOffSwitch : UIBehaviour, IPointerClickHandler {

		[System.Serializable]
		public class StateChanged : UnityEvent<OnOffSwitchState> { }

		/// <summary>
		/// Sets the state of the switch (using animation).  Use SetState() with animated==false to set the state instantly.
		/// </summary>
		public OnOffSwitchState state {
			get { return _state; }
			set { SetState(value); }
		}

		[SerializeField]
		private OnOffSwitchState _state = OnOffSwitchState.Off;

		/// <summary>
		/// The style of the switch
		/// </summary>
		public OnOffSwitchStyle style {
			get { return _style; }
			set {
				_style = value;

				//Enable/Disable style containers based on style
				foreach(var container in _onOffContainers) {
					container.gameObject.SetActive(container.style == _style);
				}
			}
		}

		[SerializeField]
		private OnOffSwitchStyle _style = OnOffSwitchStyle.Text;

		/// <summary>
		/// The speed the slider will animate to its on/off position
		/// </summary>
		public float speed = 12;

		/// <summary>
		/// The RectTransform of this UI object
		/// </summary>
		public RectTransform rectTransform { get { return transform as RectTransform; } }

		private Image _fill = null;
		private bool _animating = false;
		private OnOffSwitchSlider _slider = null;
		private Vector2 _offAnchoredPosition;
		private Vector2 _onAnchoredPosition;
		private OnOffContainer[] _onOffContainers;

		/// <summary>
		/// Event which is fired each time the state of the switch changes
		/// </summary>
		public StateChanged OnStateChanged;

		/// <summary>
		/// Toggles the on/off state
		/// </summary>
		/// <param name="animated">If true, the slider will slide over time.  Otherwise, the slider will instantly jump to its on/off position.</param>
		public void Toggle(bool animated = true) {
			SetState(state == OnOffSwitchState.Off ? OnOffSwitchState.On : OnOffSwitchState.Off, animated);
		}

		/// <summary>
		/// Triggered when the switch is clicked or tapped on
		/// </summary>
		/// <param name="eventData">Pointer event data from Unity Event System</param>
		public void OnPointerClick(PointerEventData eventData) {
			Toggle();

		}

		/// <summary>
		/// Sets the state of the switch, optionally using animation
		/// </summary>
		/// <param name="state">The new state of the switch</param>
		/// <param name="animated">If true, the slider will slide into position.  Otherwise, the slider will instantly move to position.</param>
		public void SetState(OnOffSwitchState state, bool animated = true) {
			//Dont do anything if the passed state is the same as the current state or if we are animating
			if(state == _state || _animating) return;

			//Store the change
			_state = state;

			//Move the slider
			if(animated) {
				StartCoroutine(SliderEnumerator());
			} else {
				_slider.rectTransform.anchoredPosition = state == OnOffSwitchState.On ? _onAnchoredPosition : _offAnchoredPosition;
				_fill.fillAmount = state == OnOffSwitchState.On ? 1 : 0;
			}

			//Invoke change event
			OnStateChanged.Invoke(state);
		}

		/// <summary>
		/// Coroutine to animate the slider position
		/// </summary>
		/// <returns>An IEnumerator to yield</returns>
		private IEnumerator SliderEnumerator() {
			_animating = true;

			//Set initial states
			float p = 0;
			var start = state == OnOffSwitchState.On ? _offAnchoredPosition : _onAnchoredPosition;
			var end = state == OnOffSwitchState.On ? _onAnchoredPosition : _offAnchoredPosition;

			//Loop until we are 100% complete
			while(p < 1f) {
				//Change position of slider and fill amount image
				_slider.rectTransform.anchoredPosition = Vector2.Lerp(start, end, p);
				_fill.fillAmount = state == OnOffSwitchState.On ? p : 1f - p;

				//Increment percentage complete
				p += (Time.deltaTime * speed);

				//Wait for next frame
				yield return null;
			}

			//Ensure we never under/overshoot the end position
			_slider.rectTransform.anchoredPosition = end;
			_fill.fillAmount = _state == OnOffSwitchState.On ? 1 : 0;
			_animating = false;
		}

		/// <summary>
		/// Fired by Unity
		/// </summary>
		protected override void Awake() {
			_animating = false;

			//Get component references
			_fill = transform.Find("Container/Fill").GetComponent<Image>();
			_slider = transform.GetComponentInChildren<OnOffSwitchSlider>();
			_onOffContainers = transform.GetComponentsInChildren<OnOffContainer>();

			//Reset state to serialized state
			SetState(_state, false);
			OnRectTransformDimensionsChange();
			base.Awake();
		}

		/// <summary>
		/// Fired by Unity whenever the RectTransform changes
		/// </summary>
		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();

			if(!IsActive()) return;

			//Recalculate slider positions when the size of the UI container changes
			_onAnchoredPosition = new Vector2((rectTransform.rect.width / 2f) - (_slider.rectTransform.rect.width / 2f), _slider.rectTransform.anchoredPosition.y);
			_offAnchoredPosition = new Vector2((-rectTransform.rect.width / 2f) + (_slider.rectTransform.rect.width / 2f), _slider.rectTransform.anchoredPosition.y);

			_slider.rectTransform.anchoredPosition = _state == OnOffSwitchState.On ? _onAnchoredPosition : _offAnchoredPosition;

			//var width = _slider.rectTransform.sizeDelta.x 

			_slider.rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * _slider.aspect, rectTransform.sizeDelta.y);
			_fill.fillAmount = _state == OnOffSwitchState.On ? 1 : 0;
		}
	}
}