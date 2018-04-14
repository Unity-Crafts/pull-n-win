using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	public class DragableProgressBar : ProgressBar {

		private DragableSlider _slider;

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();

			//This method can be called before OnEnable, so, we need to check for that!
			if(!IsActive()) return;

			//Update slider sizes/position
			_slider.rectTransform.sizeDelta = new Vector2((rectTransform.rect.height / _slider.image.sprite.rect.height) * _slider.image.sprite.rect.width, rectTransform.rect.height);
			_slider.rectTransform.anchoredPosition = new Vector2((-rectTransform.rect.width * rectTransform.pivot.x) + (rectTransform.rect.width * percent), _slider.rectTransform.anchoredPosition.y);

		}

		protected override void Awake() {
			_slider = transform.GetComponentInChildren<DragableSlider>();

#if UNITY_EDITOR
			OnValueChanged.AddListener(ValueChanged);
#endif
			base.Awake();
		}

#if UNITY_EDITOR
		private void ValueChanged(ProgressBar bar) {
			_slider.rectTransform.anchoredPosition = new Vector2((-rectTransform.rect.width * rectTransform.pivot.x) + (rectTransform.rect.width * percent), _slider.rectTransform.anchoredPosition.y);
		}
#endif
	}
}
