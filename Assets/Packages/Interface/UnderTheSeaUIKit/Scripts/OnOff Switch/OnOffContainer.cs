using UnityEngine;
using UnityEngine.EventSystems;

namespace HungryCannibal.UnderTheSeaUIKit.OnOffSwitches {
	[System.Serializable]
	[ExecuteInEditMode]
	public class OnOffContainer : UIBehaviour {

		public OnOffSwitchStyle style = OnOffSwitchStyle.Text;

		private GameObject _on;
		private GameObject _off;

		private void OnStateChanged(OnOffSwitchState state) {
			//Deal with state change
			if(_on != null) {
				_on.gameObject.SetActive(state == OnOffSwitchState.On);
			}

			if(_off != null) {
				_off.gameObject.SetActive(state == OnOffSwitchState.Off);
			}
		}

		protected override void Awake() {
			//Find object references
			_on = transform.Find("On").gameObject;
			_off = transform.Find("Off").gameObject;

			//Try to get the switch controller from parents
			var onOffSwitch = transform.GetComponentInParent<OnOffSwitch>();
			if(onOffSwitch == null) {
				Debug.LogError("OnOffContainer: Cannnot find parent OnOffSwitchBehaviour");
				return;
			}

			//Subscribe to state change events from the switch controller
			onOffSwitch.OnStateChanged.AddListener(OnStateChanged);

			base.Awake();
		}
	}
}
