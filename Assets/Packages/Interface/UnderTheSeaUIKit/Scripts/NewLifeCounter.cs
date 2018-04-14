using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit {
	[System.Serializable]
	[RequireComponent(typeof(Text))]
	public class NewLifeCounter : MonoBehaviour {

		public int minutes = 15;

		private Text _text;
		private Coroutine _counter = null;
		private int _currentSeconds = 0;

		private void Awake() {
			_text = GetComponent<Text>();
		}

		private IEnumerator CountDownEnumerator() {
			_currentSeconds = minutes * 60;

			while(gameObject.activeSelf) {
				//Update timer
				int minute = Mathf.FloorToInt(_currentSeconds / 60f);
				int seconds = _currentSeconds - (minute * 60);
				_text.text = string.Format("{0:D2}:{1:D2}", minute, seconds);

				yield return new WaitForSeconds(1);
				_currentSeconds--;

				//For this demo, just reset at the end
				if(_currentSeconds <= 0) _currentSeconds = minute * 60;
			}
		}

		private void OnEnable() {
			if(_counter != null) StopCoroutine(_counter);
			_counter = StartCoroutine(CountDownEnumerator());
		}

		private void OnDisable() {
			if(_counter != null) StopCoroutine(_counter);
			_counter = null;
		}
	}
}
