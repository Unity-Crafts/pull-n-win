using HungryCannibal.UnderTheSeaUIKit.ProgressBars;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Editor {
	[System.Serializable]
	[CustomEditor(typeof(CounterBar))]
	public class CounterBarEditor : UnityEditor.Editor {

		private SerializedProperty _countProperty;
		private SerializedProperty _countSpeedProperty;
		private SerializedProperty _onPlusClickProperty;
		private SerializedObject _textObject;
		private SerializedProperty _textProperty;

		private void OnEnable() {
			_countProperty = serializedObject.FindProperty("_count");
			_countSpeedProperty = serializedObject.FindProperty("countSpeed");
			_onPlusClickProperty = serializedObject.FindProperty("onPlusClick");

			_textObject = new SerializedObject((target as CounterBar).transform.Find("Count").GetComponent<Text>());
			_textProperty = _textObject.FindProperty("m_Text");
		}

		public override void OnInspectorGUI() {
			//Update
			serializedObject.Update();
			_textObject.Update();

			//Editor for count
			EditorGUI.BeginChangeCheck();
			var amount = EditorGUILayout.IntField("Count", Mathf.RoundToInt(_countProperty.floatValue));
			if(EditorGUI.EndChangeCheck()) {
				_countProperty.floatValue = amount;
				_textProperty.stringValue = amount.ToString("N0");
			}

			//Speed of the count animation
			EditorGUILayout.PropertyField(_countSpeedProperty, new GUIContent("Count Animation Speed"));

			//Space
			EditorGUILayout.Space();

			//On value changed property at the bottom, to be the same as Unity!
			EditorGUILayout.PropertyField(_onPlusClickProperty);
			serializedObject.ApplyModifiedProperties();

			//Apply
			serializedObject.ApplyModifiedProperties();
			_textObject.ApplyModifiedProperties();
		}
	}
}
