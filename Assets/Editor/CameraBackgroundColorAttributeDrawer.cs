using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CameraBackgroundColorAttribute))]
public class CameraBackgroundColorAttributeDrawer : PropertyDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position, property, label);
		if (EditorGUI.EndChangeCheck()) Camera.main.backgroundColor = property.colorValue;
	}
}