using HungryCannibal.UnderTheSeaUIKit.ProgressBars;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit.Editor {
	[System.Serializable]
	public abstract class ProgressBarEditor<T> : UnityEditor.Editor where T : ProgressBar {

		protected T _bar;
		protected string _barKey = string.Empty;
		protected SerializedProperty _onValueChangedProperty;

		protected SerializedProperty _minValueProperty;
		protected SerializedProperty _maxValueProperty;
		protected SerializedProperty _initialValueProperty;
		protected SerializedProperty _initialPercentProperty;

		protected SerializedProperty _progressTextTypeProperty;
		protected SerializedProperty _progressTextProperty;
		protected SerializedProperty _customProgressTextProperty;
		protected string _helpBoxContent = string.Empty;

		protected virtual void OnEnable() {
			_bar = target as T;
			_barKey = string.Format("ProgressBar_{0}", _bar.GetInstanceID());

			_onValueChangedProperty = serializedObject.FindProperty("OnValueChanged");

			_minValueProperty = serializedObject.FindProperty("_minValue");
			_maxValueProperty = serializedObject.FindProperty("_maxValue");
			_initialValueProperty = serializedObject.FindProperty("_value");
			_initialPercentProperty = serializedObject.FindProperty("_percent");

			_progressTextTypeProperty = serializedObject.FindProperty("_progressTextType");
			_progressTextProperty = serializedObject.FindProperty("_progressText");
			_customProgressTextProperty = serializedObject.FindProperty("_customProgressText");

			//Help text for custom bar text
			var sb = new StringBuilder();
			sb.AppendLine("Usage:");
			sb.AppendLine();
			sb.AppendFormat("{0}: The current percent value\n", ProgressBar.TEXT_PERCENT);
			sb.AppendFormat("{0}: The current value\n", ProgressBar.TEXT_VALUE);
			sb.AppendFormat("{0}: The minimum value\n", ProgressBar.TEXT_MINVALUE);
			sb.AppendFormat("{0}: The maximum value\n", ProgressBar.TEXT_MAXVALUE);
			sb.AppendLine();
			sb.AppendFormat("Example: Loading {0} of {1}", ProgressBar.TEXT_VALUE, ProgressBar.TEXT_MAXVALUE);
			_helpBoxContent = sb.ToString();
		}

		protected virtual void DrawCustomFields() { }
		protected virtual void ValuesChanged() { }

		public override void OnInspectorGUI() {
			//Style
			string styleFoldoutKey = string.Format("{0}_Style", _barKey);
			EditorPrefs.SetBool(styleFoldoutKey, EditorGUILayout.Foldout(EditorPrefs.GetBool(styleFoldoutKey), "Style"));
			if(EditorPrefs.GetBool(styleFoldoutKey)) {
				EditorGUI.indentLevel++;

				//Change progress text type
				EditorGUI.BeginChangeCheck();
				var progressType = (ProgressTextType)_progressTextTypeProperty.enumValueIndex;
				EditorGUILayout.PropertyField(_progressTextTypeProperty, new GUIContent("Progress Text"));
				if(EditorGUI.EndChangeCheck()) {
					progressType = (ProgressTextType)_progressTextTypeProperty.enumValueIndex;
					serializedObject.ApplyModifiedProperties();
					_bar.OnValueChanged.Invoke(_bar);
				}

				//If the progress text type is custom text, then show a text box
				if(progressType == ProgressTextType.Text) {
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(_customProgressTextProperty, new GUIContent("Text"));
					EditorGUILayout.HelpBox(_helpBoxContent, MessageType.Info);
					if(EditorGUI.EndChangeCheck()) {
						_progressTextProperty.stringValue = _customProgressTextProperty.stringValue;
						serializedObject.ApplyModifiedProperties();
						_bar.OnValueChanged.Invoke(_bar);
					}
				}
				EditorGUI.indentLevel--;
			}

			//Values
			string valuesFoldoutKey = string.Format("{0}_Values", _barKey);
			EditorPrefs.SetBool(valuesFoldoutKey, EditorGUILayout.Foldout(EditorPrefs.GetBool(valuesFoldoutKey), "Values"));
			if(EditorPrefs.GetBool(valuesFoldoutKey)) {
				EditorGUI.indentLevel++;
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.PropertyField(_minValueProperty);
				EditorGUILayout.PropertyField(_maxValueProperty);
				EditorGUILayout.PropertyField(_initialValueProperty);

				if(EditorGUI.EndChangeCheck()) {
					_initialValueProperty.floatValue = Mathf.Clamp(_initialValueProperty.floatValue, _minValueProperty.floatValue, _maxValueProperty.floatValue);
					_initialPercentProperty.floatValue = ProgressBar.CalculatePercent(_minValueProperty.floatValue, _maxValueProperty.floatValue, _initialValueProperty.floatValue);

					serializedObject.ApplyModifiedProperties();
					_bar.OnValueChanged.Invoke(_bar);
					ValuesChanged();
				}

				EditorGUI.indentLevel--;
			}

			//Draw the custom fields for this progress bar type
			DrawCustomFields();

			//Space
			EditorGUILayout.Space();

			//Events at the bottom, to be the same as Unity!
			EditorGUILayout.PropertyField(_onValueChangedProperty);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
