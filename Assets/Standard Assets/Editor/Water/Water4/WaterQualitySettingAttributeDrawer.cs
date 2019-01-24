using UnityEditor;
using UnityEngine;

namespace UnityStandardAssets.Water {
	[CustomPropertyDrawer(typeof(WaterQualitySettingAttribute))]
	public class WaterQualityAttributeDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property, label);
			if (EditorGUI.EndChangeCheck()) foreach (var waterBase in property.serializedObject.targetObjects) ((WaterBase) waterBase).UpdateShader();
		}
	}
}