using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	public class DragableSlider : UIBehaviour, IDragHandler {

		public RectTransform rectTransform { get { return transform as RectTransform; } }

		[HideInInspector]
		public Image image;

		private ProgressBar _progressBar;
		private Vector2 _min;
		private Vector2 _max;

		/// <summary>
		/// To handle dragging from IDragHandler
		/// </summary>
		/// <param name="eventData">The drag event data</param>
		public void OnDrag(PointerEventData eventData) {
			Vector3 globalMousePos;

			//If the drag is inside this rectangle
			if(RectTransformUtility.ScreenPointToWorldPointInRectangle(_progressBar.rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos)) {

				//Set the global position of the slider
				transform.position = new Vector3(globalMousePos.x, transform.position.y, transform.position.z);

				//If the slider is < min or > max, then reset its position
				if(rectTransform.anchoredPosition.x < _min.x) {
					rectTransform.anchoredPosition = _min;
				} else if(rectTransform.anchoredPosition.x > _max.x) {
					rectTransform.anchoredPosition = _max;
				}

				//Update progress percent
				_progressBar.percent = (rectTransform.anchoredPosition.x + (_progressBar.rectTransform.rect.width / 2f)) / _progressBar.rectTransform.rect.width;
			}
		}

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();

			if(!IsActive()) return;

			//Calculate min & max positions
			_min = new Vector2(-_progressBar.rectTransform.rect.width / 2f, rectTransform.anchoredPosition.y);
			_max = new Vector2(_progressBar.rectTransform.rect.width / 2f, rectTransform.anchoredPosition.y);
		}

		protected override void Awake() {
			//Get component references
			_progressBar = transform.GetComponentInParent<ProgressBar>();
			image = GetComponent<Image>();

			base.Awake();
		}
	}
}
