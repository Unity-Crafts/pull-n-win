using HungryCannibal.UnderTheSeaUIKit.ProgressBars;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HungryCannibal.UnderTheSeaUIKit.Editor {
	[System.Serializable]
	[CustomEditor(typeof(AwardProgressBar), true)]
	public class AwardProgressBarEditor : ProgressBarEditor<AwardProgressBar> {

		protected SerializedProperty _activeAwardSpriteProperty;
		protected SerializedProperty _inactiveAwardSpriteProperty;
		protected SerializedProperty _awardsProperty;

		protected override void ValuesChanged() {
			foreach(var award in _bar.awards) {
				award.ProgressBar_OnValueChanged(_bar);
			}
		}

		protected override void OnEnable() {
			base.OnEnable();

			_activeAwardSpriteProperty = serializedObject.FindProperty("activeAwardSprite");
			_inactiveAwardSpriteProperty = serializedObject.FindProperty("inactiveAwardSprite");
			_awardsProperty = serializedObject.FindProperty("awards");
		}

		protected override void DrawCustomFields() {
			//Awards
			int? deleteAwardIdx = null;
			string awardsFoldoutKey = string.Format("{0}_Awards", _barKey);

			EditorPrefs.SetBool(awardsFoldoutKey, EditorGUILayout.Foldout(EditorPrefs.GetBool(awardsFoldoutKey), "Awards"));
			if(EditorPrefs.GetBool(awardsFoldoutKey)) {
				EditorGUI.indentLevel++;

				//Sprites
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_activeAwardSpriteProperty, new GUIContent("Active Sprite"));
				EditorGUILayout.PropertyField(_inactiveAwardSpriteProperty, new GUIContent("Inactive Sprite"));
				if(EditorGUI.EndChangeCheck()) {
					serializedObject.ApplyModifiedProperties();
					ValuesChanged();
				}

				//List awards
				for(int i = 0; i < _awardsProperty.arraySize; i++) {
					EditorGUILayout.BeginHorizontal();
					string awardFoldoutKey = string.Format("{0}_Award_{1}", _barKey, i);
					EditorPrefs.SetBool(awardFoldoutKey, EditorGUILayout.Foldout(EditorPrefs.GetBool(awardFoldoutKey), string.Format("Award {0}", i + 1)));
					if(!_bar.hasFixedAwardCount) {
						if(GUILayout.Button("X", GUILayout.Width(25))) {
							deleteAwardIdx = i;
						}
					}
					EditorGUILayout.EndHorizontal();

					if(EditorPrefs.GetBool(awardFoldoutKey)) {
						EditorGUI.indentLevel++;
						EditorGUI.BeginChangeCheck();

						var awardSerializedObject = new SerializedObject(_awardsProperty.FindPropertyRelative(string.Format("Array.data[{0}]", i)).objectReferenceValue);
						var awardValueProperty = awardSerializedObject.FindProperty("_awardValue");
						EditorGUILayout.PropertyField(awardValueProperty);

						if(EditorGUI.EndChangeCheck()) {
							awardValueProperty.floatValue = Mathf.Clamp(awardValueProperty.floatValue, _bar.minValue, _bar.maxValue);
							awardSerializedObject.FindProperty("_awardPercent").floatValue = ProgressBar.CalculatePercent(_bar.minValue, _bar.maxValue, awardValueProperty.floatValue);
							awardSerializedObject.ApplyModifiedProperties();
							_bar.UpdateLayout();
						}

						EditorGUI.indentLevel--;
					}
				}

				if(!_bar.hasFixedAwardCount) {
					if(GUILayout.Button("Add Award")) {
						var go = new GameObject("Award");

						var award = go.AddComponent<ProgressAward>();
						if(award.image == null) award.image = award.GetComponent<Image>();
						award.image.preserveAspect = true;
						award.image.sprite = _bar.inactiveAwardSprite;
						award.transform.SetParent(_bar.transform.Find("Container"), false);
						award.transform.SetAsLastSibling();
						award.image.rectTransform.sizeDelta = new Vector2(award.image.rectTransform.rect.width, (_bar.transform as RectTransform).rect.height);

						_bar.progressLabel.transform.SetAsLastSibling();

						_awardsProperty.arraySize++;
						_awardsProperty.GetArrayElementAtIndex(_awardsProperty.arraySize - 1).objectReferenceValue = award;
					}

					if(deleteAwardIdx.HasValue) {
						var award = _awardsProperty.GetArrayElementAtIndex(deleteAwardIdx.Value);

						var awardObj = award.objectReferenceValue as ProgressAward;

						if(awardObj != null) {
							DestroyImmediate(awardObj.gameObject);
						}

						if(_awardsProperty.GetArrayElementAtIndex(deleteAwardIdx.Value).objectReferenceValue != null) {
							_awardsProperty.DeleteArrayElementAtIndex(deleteAwardIdx.Value);
						}
						_awardsProperty.MoveArrayElement(deleteAwardIdx.Value, _awardsProperty.arraySize - 1);
						_awardsProperty.arraySize--;
					}
				}

				EditorGUI.indentLevel--;
			}

			//Events
			EditorGUILayout.Space();
			var prop = serializedObject.FindProperty("OnAwardAwarded");
			EditorGUILayout.PropertyField(prop);
			serializedObject.ApplyModifiedProperties();
		}
	}
}