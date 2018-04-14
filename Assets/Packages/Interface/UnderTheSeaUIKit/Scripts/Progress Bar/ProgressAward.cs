using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[RequireComponent(typeof(Image))]
	[ExecuteInEditMode]
	public class ProgressAward : MonoBehaviour {

		/// <summary>
		/// The current percent of the award
		/// </summary>
		public float awardPercent {
			get { return _awardPercent; }
			set {
				_awardPercent = value;
				_awardValue = ProgressBar.CalculateValue(_bar.minValue, _bar.maxValue, _awardPercent);
				ProgressBar_OnValueChanged(_bar);
			}
		}

		[SerializeField]
		private float _awardPercent = 0;

		/// <summary>
		/// The current value of the award
		/// </summary>
		public float awardValue {
			get { return _awardValue; }
			set {
				_awardValue = value;
				awardPercent = ProgressBar.CalculatePercent(_bar.minValue, _bar.maxValue, _awardValue);
			}
		}

		[SerializeField]
		private float _awardValue = 0;

		public Image image;
		public bool eventFired = false;
		public Sprite inactiveSprite;
		public Sprite activeSprite;

		private AwardProgressBar _bar;
		private Color _transparent;

		private void Awake() {
			image = GetComponent<Image>();
			_transparent = new Color(image.color.r, image.color.g, image.color.b, 0f);

			_bar = transform.GetComponentInParent<AwardProgressBar>();
			if(_bar != null) {
				_bar.OnValueChanged.AddListener(ProgressBar_OnValueChanged);
			}
		}

		/// <summary>
		/// Called when the value of the progress bar changes
		/// </summary>
		/// <param name="bar">The progress bar this award is associated with</param>
		public void ProgressBar_OnValueChanged(ProgressBar bar) {
			//Update the sprites when a value changes
			if(bar.percent >= _awardPercent) {
				image.color = Color.white;
				image.sprite = activeSprite == null ? _bar.activeAwardSprite : activeSprite;
			} else {
				if(inactiveSprite == null && _bar.inactiveAwardSprite == null) {
					image.color = _transparent;
				} else {
					image.color = Color.white;
					image.sprite = inactiveSprite == null ? _bar.inactiveAwardSprite : inactiveSprite;
				}
			}
		}
	}
}
