using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Tabs {
	[System.Serializable]
	public class TabController : UIBehaviour {

		public string initialTabName;
		public Transform tabsContainer = null;
		public Transform tabContentContainer = null;

		[System.Serializable]
		public class TabSprites {
			public Sprite active;
			public Sprite inactive;
		}

		public TabSprites tabSprites;

		[System.Serializable]
		private class TabContainerInternal {
			public Tab tab;
			public TabContainer container;

			private TabSprites _tabSprites;

			public TabContainerInternal(TabSprites tabSprites) {
				_tabSprites = tabSprites;
			}

			public bool active {
				get { return _active; }
				set {
					_active = value;
					container.gameObject.SetActive(_active);
					tab.image.sprite = _active ? _tabSprites.active : _tabSprites.inactive;
				}
			}

			private bool _active = false;
		}

		private Dictionary<string, TabContainerInternal> _tabs;
		private TabContainerInternal _currentTab = null;

		protected override void Awake() {

			//Get all tabs and containers
			var tabs = tabsContainer == null ? transform.GetComponentsInChildren<Tab>(true) : tabsContainer.GetComponentsInChildren<Tab>(true);
			var tabContainers = tabContentContainer == null ? transform.GetComponentsInChildren<TabContainer>(true) : tabContentContainer.GetComponentsInChildren<TabContainer>(true);

			//Initialise dictionary
			if(_tabs == null) _tabs = new Dictionary<string, TabContainerInternal>();

			//Add tabs to dictionary
			for(int i = 0; i < tabs.Length; i++) {
				var tab = tabs[i];
				var name = tab.gameObject.name;

				//Try to find a container with the same name
				var container = tabContainers.FirstOrDefault(c => c.gameObject.name == name);
				if(container == null) {
					Debug.LogErrorFormat("Tab {0} does not have a corrisponding container with the same name", name);
					continue;
				}

				//We can not have 2 tabs with the same name!
				if(_tabs.ContainsKey(tab.gameObject.name)) {
					Debug.LogErrorFormat("There are 2 or more tabs with the same name: {0}", name);
					continue;
				}

				//Get component references
				tab.image = tab.GetComponent<Image>();

				//Add on click handler
				var button = tab.GetComponent<Button>();
				button.onClick.AddListener(() => OnTabClick(name));

				//All ok!  Add the tab to the dictionary
				_tabs.Add(name, new TabContainerInternal(tabSprites) {
					tab = tab,
					container = container
				});

				//Initially hide all tabs
				container.gameObject.SetActive(false);
			}

			//If we dont have an initial name, then just use the first tab in the list
			if(string.IsNullOrEmpty(initialTabName) && _tabs.Count() > 0) {
				initialTabName = _tabs.Keys.First();
			}

			//Fire click event to show the initial tab
			OnTabClick(initialTabName);

			base.Awake();
		}

		private void OnTabClick(string name) {
			//Do nothing if we dont have a tab by this name
			if(!_tabs.ContainsKey(name)) return;

			//Hide the current tab if we have one
			if(_currentTab != null) {
				_currentTab.active = false;
			}

			//Set this tab as the new current tab
			_currentTab = _tabs[name];
			_currentTab.active = true;
		}
	}
}
