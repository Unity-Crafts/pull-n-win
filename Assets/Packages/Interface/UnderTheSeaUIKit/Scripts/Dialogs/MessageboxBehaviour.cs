using System;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Dialogs {
	[System.Serializable]
	public class MessageboxBehaviour : DialogBehaviour {

		private Text _title;
		private Text _message;
		private Action _onOKClick;

		protected override void Awake() {
			_title = transform.Find("Header/Title").GetComponent<Text>();
			_message = transform.Find("Container/Background/Message").GetComponent<Text>();
			base.Awake();
		}

		public void Show(string title, string message, Action onOKClick) {
			_title.text = title;
			_message.text = message;
			_onOKClick = onOKClick;
			base.Show();
		}

		public void OnOKClick() {
			if(_onOKClick != null) {
				_onOKClick();
			}
		}
	}
}
