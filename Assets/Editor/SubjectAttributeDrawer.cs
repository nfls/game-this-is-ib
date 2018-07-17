using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SubjectAttribute))]
public class SubjectAttributeDrawer : PropertyDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		string[] subjects = Subject.Names;
		int index = 0;
		for(int i = 0, l = subjects.Length; i < l; i ++) {
			if (property.stringValue == subjects[i]) {
				index = i;
				break;
			}
		}

		property.stringValue = subjects[EditorGUI.Popup(position, label.text, index, subjects)];
	}
}