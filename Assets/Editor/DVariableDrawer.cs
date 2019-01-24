using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DInt))]
public class DVariableDrawer : PropertyDrawer {

	public const float FIELD_HEIGHT = 16f;
	public const float PADDING = 3f;
	public const float LINE_HEIGHT = FIELD_HEIGHT + PADDING;
	public const float CHARACTER_WIDTH = 7f;
	public const float SPACE = 5f;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = "" + label.text + " : " + property.FindPropertyRelative("_value").intValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DInt dInt = property.ToObject<DInt>();
		EditorGUI.BeginChangeCheck();
		int result = EditorGUI.DelayedIntField(pos, "[Real Value]", property.FindPropertyRelative("_realValue").intValue);
		if (EditorGUI.EndChangeCheck()) dInt.Value = result;
		pos.xMin -= 3 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			for (int i = 0, l = dInt.DecoratorCount; i < l; i++) {
				IntDecorator decorator = dInt.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6.5f * CHARACTER_WIDTH };
				EditorGUI.LabelField(rect, "Priority");
				rect.x += rect.width + SPACE;
				rect.width = 3 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.DelayedIntField(rect, decorator.priority);
				if (EditorGUI.EndChangeCheck()) {
					decorator.priority = result;
					dInt.Reorder();
					dInt.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 4 * CHARACTER_WIDTH;
				EditorGUI.LabelField(rect, decorator.type);
				rect.x += rect.width + SPACE;
				rect.width = 7 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.DelayedIntField(rect, decorator.value);
				if (EditorGUI.EndChangeCheck()) {
					decorator.value = result;
					dInt.Refresh();
				}
				
				rect.x += rect.width + 2 * SPACE;
				rect.width = pos.xMax - rect.x;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dInt.RemoveDecorator(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				IntDecorator decorator = EditorExtensions.IntDecoratorPopup(pos);
				if (decorator != null) dInt.AddDecorator(decorator);
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		float height = LINE_HEIGHT;
		if (!property.isExpanded) return height;
		height += LINE_HEIGHT;
		DInt dInt = property.ToObject<DInt>();
		height += LINE_HEIGHT * dInt.DecoratorCount;
		return height;
	}
}