using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HungryCannibal.UnderTheSeaUIKit.Dialogs {
	[System.Serializable]
	[RequireComponent(typeof(CanvasGroup))]
	public class DialogBehaviour : UIBehaviour {
		public float visibilitySpeed = 3;
		public Text contentMessage = null;
		protected DialogManager manager;

		public RectTransform rectTransform { get { return transform as RectTransform; } }

		private CanvasGroup _canvasGroup;

		protected override void Awake() {
			manager = GetComponentInParent<DialogManager>();
			rectTransform.anchoredPosition = Vector2.zero;
			_canvasGroup = GetComponent<CanvasGroup>();

			base.Awake();
		}

		protected override void Start() {
			gameObject.SetActive(false);
			base.Start();
		}

		public virtual void Show() {
			manager.PushDialog(this);

			StartCoroutine(CrossFadeEnumerator(0, 1));
			StartCoroutine(ShowEnumerator());
		}

		public virtual void Show(string content) {
			if(contentMessage != null)
			{
				contentMessage.text = content;
			}

			manager.PushDialog(this);

			StartCoroutine(CrossFadeEnumerator(0, 1));
			StartCoroutine(ShowEnumerator());
		}

		public virtual void Hide() {
			if (!gameObject.activeInHierarchy)
				return;
			
			StartCoroutine(CrossFadeEnumerator(1, 0, () => manager.PopDialog()));
		}

		public void OnDialogHideAnimationComplete() {
			gameObject.SetActive(false);
		}

		private IEnumerator CrossFadeEnumerator(float alphaStart, float alphaEnd, Action onComplete = null) {
			float p = 0;

			while(p < 1) {
				_canvasGroup.alpha = Mathf.Lerp(alphaStart, alphaEnd, p);
				p += Time.deltaTime * visibilitySpeed;
				yield return null;
			}

			_canvasGroup.alpha = alphaEnd;

			if(onComplete != null) onComplete();
		}

		private IEnumerator ShowEnumerator() {
			yield return ScaleEnumerator(1.2f, 0.8f, 4);
			yield return ScaleEnumerator(0.8f, 1.2f, 4);
			yield return ScaleEnumerator(1.1f, 0.9f, 4);
			yield return ScaleEnumerator(1f, 1f, 4);
		}

		private IEnumerator ScaleEnumerator(float xScale, float yScale, float enumerations) {
			var start = rectTransform.localScale;
			var end = new Vector3(xScale, yScale, 1);
			float p = 0;

			while(p < 1) {
				rectTransform.localScale = Vector3.Lerp(start, end, p);
				p += Time.deltaTime * (visibilitySpeed * enumerations);
				yield return null;
			}

			rectTransform.localScale = end;
		}
	}
}
