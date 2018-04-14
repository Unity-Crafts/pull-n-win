using System.Collections;
using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit {
	[System.Serializable]
	public class ViewSwitcher : MonoBehaviour {

		public CanvasGroup[] views;
		public int viewIndex = 0;
		public float speed = 1f;

		private bool _fading = false;

		private void Awake() {
			foreach(var view in views) {
				view.alpha = 0;
				view.gameObject.SetActive(false);
				(view.transform as RectTransform).anchoredPosition = Vector2.zero;
			}
			//views[viewIndex].gameObject.SetActive(true);
			//views[viewIndex].alpha = 1;
		}

		public void SwitchView(int idx) {
			if(idx == viewIndex || _fading) return;

			StartCoroutine(CrossFadeEnumerator(viewIndex, idx));
		}

		private IEnumerator CrossFadeEnumerator(int fromIdx, int toIdx) {
			_fading = true;
			float p = 0;
			var fromView = views[fromIdx];
			var toView = views[toIdx];

			while(p < 1f) {
				fromView.alpha = Mathf.Lerp(1, 0, p);
				p += (Time.deltaTime * speed);
				yield return null;
			}

			fromView.alpha = 0;
			fromView.gameObject.SetActive(false);
			toView.gameObject.SetActive(true);

			p = 0f;
			while(p < 1f) {
				toView.alpha = Mathf.Lerp(0, 1, p);
				p += (Time.deltaTime * speed);
				yield return null;
			}
			toView.alpha = 1;

			viewIndex = toIdx;
			_fading = false;
		}
	}
}
