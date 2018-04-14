using HungryCannibal.UnderTheSeaUIKit.OnOffSwitches;
using UnityEditor;
using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit.Editor {
	[System.Serializable]
	[CustomEditor(typeof(OnOffSwitch), true)]
	public class OnOffSwitchEditor : UnityEditor.Editor {

		private OnOffSwitch _switch;

		private SerializedProperty _stateProperty;
		private SerializedProperty _speedProperty;
		private SerializedProperty _styleProperty;

		protected virtual void OnEnable() {
			_switch = target as OnOffSwitch;

			_stateProperty = serializedObject.FindProperty("_state");
			_speedProperty = serializedObject.FindProperty("speed");
			_styleProperty = serializedObject.FindProperty("_style");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_stateProperty, new GUIContent("State"));
			if(EditorGUI.EndChangeCheck()) {
				_switch.SetState((OnOffSwitchState)_stateProperty.enumValueIndex, false);
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_styleProperty, new GUIContent("Style"));
			if(EditorGUI.EndChangeCheck()) {
				var containers = _switch.transform.GetComponentsInChildren<OnOffContainer>(true);
				foreach(var container in containers) {
					container.gameObject.SetActive(container.style == (OnOffSwitchStyle)_styleProperty.enumValueIndex);
				}
			}

			EditorGUILayout.PropertyField(_speedProperty, new GUIContent("Switch Speed"));

			EditorGUILayout.Space();

			//Events
			var prop = serializedObject.FindProperty("OnStateChanged");
			EditorGUILayout.PropertyField(prop);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
