using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DInt))]
public abstract class DVariableDrawer : PropertyDrawer {

	public const float FIELD_HEIGHT = 16f;
	public const float PADDING = 3f;
	public const float LINE_HEIGHT = FIELD_HEIGHT + PADDING;
	public const float CHARACTER_WIDTH = 7f;
	public const float SPACE = 5f;
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		float height = LINE_HEIGHT;
		if (!property.isExpanded) return height;
		height += LINE_HEIGHT;
		DInt dInt = property.ToObject<DInt>();
		height += LINE_HEIGHT * dInt.DecoratorCount;
		return height;
	}
}

public class DIntDrawer : DVariableDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = "" + label.text + " = " + property.FindPropertyRelative("_value").intValue;
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
		pos.xMin -= 6 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(property.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dInt.DecoratorCount; i < l; i++) {
				IntDecorator decorator = dInt.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
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
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dInt.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
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
				dInt.AddDecorator(new IntDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}

	public static DInt OnGUI(Rect position, DInt dInt, GUIContent label) {
		Rect pos = position;
		dInt = dInt ?? new DInt();
		string text = "" + label.text + " = " + dInt.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		dInt.isExpanded = EditorGUI.Foldout(pos, dInt.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		int result = EditorGUI.DelayedIntField(pos, "[Real Value]", dInt.realValue);
		if (EditorGUI.EndChangeCheck()) dInt.Value = result;
		pos.xMin -= 6 * SPACE;
		if (dInt.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(dInt.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, dInt.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dInt.DecoratorCount; i < l; i++) {
				IntDecorator decorator = dInt.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
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
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dInt.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
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

			if (delete != -1) dInt.RemoveDecorator(delete);
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dInt.AddDecorator(new IntDecorator());
			}
		}
		
		return dInt;
	}
	
	public static float GetPropertyHeight(DInt dInt) {
		dInt = dInt ?? new DInt();
		float height = LINE_HEIGHT;
		if (!dInt.isExpanded) return height;
		height += LINE_HEIGHT;
		height += LINE_HEIGHT * dInt.DecoratorCount;
		return height;
	}
}

public class DLongDrawer : DVariableDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = "" + label.text + " = " + property.FindPropertyRelative("_value").longValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DLong dLong = property.ToObject<DLong>();
		EditorGUI.BeginChangeCheck();
		long result = EditorGUI.LongField(pos, "[Real Value]", property.FindPropertyRelative("_realValue").longValue);
		if (EditorGUI.EndChangeCheck()) dLong.Value = result;
		pos.xMin -= 6 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(property.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dLong.DecoratorCount; i < l; i++) {
				LongDecorator decorator = dLong.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
				EditorGUI.LabelField(rect, "Priority");
				rect.x += rect.width + SPACE;
				rect.width = 3 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				int r = EditorGUI.DelayedIntField(rect, decorator.priority);
				if (EditorGUI.EndChangeCheck()) {
					decorator.priority = r;
					dLong.Reorder();
					dLong.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dLong.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.LongField(rect, decorator.value);
				if (EditorGUI.EndChangeCheck()) {
					decorator.value = result;
					dLong.Refresh();
				}
				
				rect.x += rect.width + 2 * SPACE;
				rect.width = pos.xMax - rect.x;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dLong.RemoveDecorator(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dLong.AddDecorator(new LongDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}
	
	public static DLong OnGUI(Rect position, DLong dLong, GUIContent label) {
		Rect pos = position;
		dLong = dLong ?? new DLong();
		string text = "" + label.text + " = " + dLong.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		dLong.isExpanded = EditorGUI.Foldout(pos, dLong.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		long result = EditorGUI.LongField(pos, "[Real Value]", dLong.realValue);
		if (EditorGUI.EndChangeCheck()) dLong.Value = result;
		pos.xMin -= 6 * SPACE;
		if (dLong.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(dInt.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, dInt.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dLong.DecoratorCount; i < l; i++) {
				LongDecorator decorator = dLong.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
				EditorGUI.LabelField(rect, "Priority");
				rect.x += rect.width + SPACE;
				rect.width = 3 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				int r = EditorGUI.DelayedIntField(rect, decorator.priority);
				if (EditorGUI.EndChangeCheck()) {
					decorator.priority = r;
					dLong.Reorder();
					dLong.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dLong.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.LongField(rect, decorator.value);
				if (EditorGUI.EndChangeCheck()) {
					decorator.value = result;
					dLong.Refresh();
				}
				
				rect.x += rect.width + 2 * SPACE;
				rect.width = pos.xMax - rect.x;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) dLong.RemoveDecorator(delete);
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dLong.AddDecorator(new LongDecorator());
			}
		}
		
		return dLong;
	}
	
	public static float GetPropertyHeight(DLong dLong) {
		dLong = dLong ?? new DLong();
		float height = LINE_HEIGHT;
		if (!dLong.isExpanded) return height;
		height += LINE_HEIGHT;
		height += LINE_HEIGHT * dLong.DecoratorCount;
		return height;
	}
}

public class DFloatDrawer : DVariableDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = "" + label.text + " = " + property.FindPropertyRelative("_value").floatValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DFloat dFloat = property.ToObject<DFloat>();
		EditorGUI.BeginChangeCheck();
		float result = EditorGUI.DelayedFloatField(pos, "[Real Value]", property.FindPropertyRelative("_realValue").floatValue);
		if (EditorGUI.EndChangeCheck()) dFloat.Value = result;
		pos.xMin -= 6 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(property.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dFloat.DecoratorCount; i < l; i++) {
				FloatDecorator decorator = dFloat.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
				EditorGUI.LabelField(rect, "Priority");
				rect.x += rect.width + SPACE;
				rect.width = 3 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				int r = EditorGUI.DelayedIntField(rect, decorator.priority);
				if (EditorGUI.EndChangeCheck()) {
					decorator.priority = r;
					dFloat.Reorder();
					dFloat.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dFloat.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.DelayedFloatField(rect, decorator.value);
				if (EditorGUI.EndChangeCheck()) {
					decorator.value = result;
					dFloat.Refresh();
				}
				
				rect.x += rect.width + 2 * SPACE;
				rect.width = pos.xMax - rect.x;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dFloat.RemoveDecorator(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dFloat.AddDecorator(new FloatDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}
	
	public static DFloat OnGUI(Rect position, DFloat dFloat, GUIContent label) {
		Rect pos = position;
		dFloat = dFloat ?? new DFloat();
		string text = "" + label.text + " = " + dFloat.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		dFloat.isExpanded = EditorGUI.Foldout(pos, dFloat.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		float result = EditorGUI.DelayedFloatField(pos, "[Real Value]", dFloat.realValue);
		if (EditorGUI.EndChangeCheck()) dFloat.Value = result;
		pos.xMin -= 6 * SPACE;
		if (dFloat.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;
			/*
			pos.y += LINE_HEIGHT;
			pos.height = position.yMax - pos.y;
			Debug.Log(dInt.FindPropertyRelative("variableDecorators") == null);
			EditorGUI.PropertyField(pos, dInt.FindPropertyRelative("variableDecorators"));
			*/
			for (int i = 0, l = dFloat.DecoratorCount; i < l; i++) {
				FloatDecorator decorator = dFloat.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { width = 6f * CHARACTER_WIDTH };
				EditorGUI.LabelField(rect, "Priority");
				rect.x += rect.width + SPACE;
				rect.width = 3 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				int r  = EditorGUI.DelayedIntField(rect, decorator.priority);
				if (EditorGUI.EndChangeCheck()) {
					decorator.priority = r;
					dFloat.Reorder();
					dFloat.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				VariableDecoratorType type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);
				if (EditorGUI.EndChangeCheck()) {
					decorator.type = type;
					dFloat.Refresh();
				}
				
				rect.x += rect.width + SPACE;
				rect.width = 5 * CHARACTER_WIDTH;
				EditorGUI.BeginChangeCheck();
				result = EditorGUI.DelayedFloatField(rect, decorator.value);
				if (EditorGUI.EndChangeCheck()) {
					decorator.value = result;
					dFloat.Refresh();
				}
				
				rect.x += rect.width + 2 * SPACE;
				rect.width = pos.xMax - rect.x;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) dFloat.RemoveDecorator(delete);
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dFloat.AddDecorator(new FloatDecorator());
			}
		}
		
		return dFloat;
	}
	
	public static float GetPropertyHeight(DFloat dFloat) {
		dFloat = dFloat ?? new DFloat();
		float height = LINE_HEIGHT;
		if (!dFloat.isExpanded) return height;
		height += LINE_HEIGHT;
		height += LINE_HEIGHT * dFloat.DecoratorCount;
		return height;
	}
}

/*
[CustomPropertyDrawer(typeof(IntDecorator))]
public class VariableDecoratorDrawer : PropertyDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.PropertyField(position, property, label);
	}
}
*/