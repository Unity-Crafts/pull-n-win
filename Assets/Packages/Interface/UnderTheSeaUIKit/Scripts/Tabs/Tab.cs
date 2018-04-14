using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Tabs {
	[System.Serializable]
	[RequireComponent(typeof(Button))]
	public class Tab : UIBehaviour {
		[HideInInspector]
		public Image image;
	}
}
