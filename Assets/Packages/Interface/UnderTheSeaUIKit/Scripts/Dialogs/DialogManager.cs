using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Dialogs {
	[System.Serializable]
	public class DialogManager : MonoBehaviour {
		public static DialogManager instance;

		public Image background;

		private Stack<DialogBehaviour> _dialogs;

		private void Awake() {
			instance = this;
			_dialogs = new Stack<DialogBehaviour>();

			background.rectTransform.anchoredPosition = Vector2.zero;
			background.gameObject.SetActive(false);
		}

		public void PushDialog(DialogBehaviour dialog) {
			background.gameObject.SetActive(true);
			background.transform.SetAsLastSibling();
			dialog.gameObject.SetActive(true);
			dialog.rectTransform.anchoredPosition = Vector2.zero;
			dialog.rectTransform.SetAsLastSibling();
			_dialogs.Push(dialog);
		}

		public DialogBehaviour PopDialog() {
			if(_dialogs.Count > 0) {
				var dialog = _dialogs.Pop();
				dialog.gameObject.SetActive(false);

				if(_dialogs.Count == 0) {
					background.gameObject.SetActive(false);
				} else {
					_dialogs.Peek().transform.SetAsLastSibling();
				}

				return dialog;
			}
			return null;
		}

		public void HideAll() {
			foreach(var dialog in _dialogs) {
				dialog.gameObject.SetActive(false);
			}
			background.gameObject.SetActive(false);
			_dialogs.Clear();
		}
	}
}
