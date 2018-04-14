using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.OnOffSwitches {
	[System.Serializable]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	public class OnOffSwitchSlider : UIBehaviour {

		public float aspect {
			get {
				if(!_aspect.HasValue) {
					var image = GetComponent<Image>();
					_aspect = image.sprite.rect.width / image.sprite.rect.height;
				}
				return _aspect.Value;
			}
		}
		private float? _aspect = null;

		public RectTransform rectTransform { get { return transform as RectTransform; } }
	}
}
