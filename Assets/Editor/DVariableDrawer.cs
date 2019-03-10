using System;
using UnityEditor;
using UnityEngine;

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
		DVariable dVariable = GetObject(property);
		height += LINE_HEIGHT * dVariable.DecoratorCount;
		return height;
	}

	protected abstract DVariable GetObject(SerializedProperty property);
}

[CustomPropertyDrawer(typeof(DInt))]
public class DIntDrawer : DVariableDrawer {
	
	protected override DVariable GetObject(SerializedProperty property) => property.ToObject<DInt>();
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = label.text + " = " + property.FindPropertyRelative("_value").intValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DVariable dVariable = GetObject(property);
		EditorGUI.BeginChangeCheck();
		SerializedProperty realValueProperty = property.FindPropertyRelative("realValue");
		realValueProperty.intValue = EditorGUI.DelayedIntField(pos, "[Real Value]", realValueProperty.intValue);
		if (EditorGUI.EndChangeCheck()) dVariable.Refresh();
		pos.x = position.x + 3 * SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			SerializedProperty decoratorsProperty = property.FindPropertyRelative("variableDecorators");
			for (int i = 0, l = decoratorsProperty.arraySize; i < l; i++) {
				SerializedProperty decoratorProperty = decoratorsProperty.GetArrayElementAtIndex(i);
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH } ;
				EditorGUI.BeginChangeCheck();

				EditorGUI.PropertyField(rect, decoratorProperty, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					dVariable.Sort();
					dVariable.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dVariable.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				(dVariable as DInt)?.Add(new IntDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}

	public static DInt OnGUI(Rect position, DInt dInt, GUIContent label, SerializedObject serializedObject) {
		Rect pos = position;
		dInt = dInt ?? new DInt();
		string text = label.text + " = " + dInt.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		dInt.isExpanded = EditorGUI.Foldout(pos, dInt.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + 3 * SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		int result = EditorGUI.DelayedIntField(pos, "[Real Value]", dInt.realValue);
		if (EditorGUI.EndChangeCheck()) {
			dInt.Value = result;
			dInt.Refresh();
		}
		
		pos.x = position.x + SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (dInt.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			for (int i = 0, l = dInt.DecoratorCount; i < l; i++) {
				IntDecorator decorator = dInt.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH } ;
				EditorGUI.BeginChangeCheck();

				VariableDecoratorDrawer.OnGUI(rect, decorator, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					dInt.Sort();
					dInt.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dInt.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dInt.Add(new IntDecorator());
				EditorUtility.SetDirty(serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(serializedObject.targetObject);

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

[CustomPropertyDrawer(typeof(DLong))]
public class DLongDrawer : DVariableDrawer {
	
	protected override DVariable GetObject(SerializedProperty property) => property.ToObject<DLong>();
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = label.text + " = " + property.FindPropertyRelative("_value").longValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DVariable dVariable = GetObject(property);
		EditorGUI.BeginChangeCheck();
		SerializedProperty realValueProperty = property.FindPropertyRelative("realValue");
		realValueProperty.longValue = EditorGUI.LongField(pos, "[Real Value]", realValueProperty.longValue);
		if (EditorGUI.EndChangeCheck()) dVariable.Refresh();
		pos.x = position.x + 3 * SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			SerializedProperty decoratorsProperty = property.FindPropertyRelative("variableDecorators");
			for (int i = 0, l = decoratorsProperty.arraySize; i < l; i++) {
				SerializedProperty decoratorProperty = decoratorsProperty.GetArrayElementAtIndex(i);
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH } ;
				EditorGUI.BeginChangeCheck();

				EditorGUI.PropertyField(rect, decoratorProperty, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					dVariable.Sort();
					dVariable.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dVariable.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				(dVariable as DLong)?.Add(new LongDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}
	
	public static DLong OnGUI(Rect position, DLong DLong, GUIContent label, SerializedObject serializedObject) {
		Rect pos = position;
		DLong = DLong ?? new DLong();
		string text = label.text + " = " + DLong.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		DLong.isExpanded = EditorGUI.Foldout(pos, DLong.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + 3 * SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		long result = EditorGUI.LongField(pos, "[Real Value]", DLong.realValue);
		if (EditorGUI.EndChangeCheck()) {
			DLong.Value = result;
			DLong.Refresh();
		}
		
		pos.x = position.x + SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (DLong.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			for (int i = 0, l = DLong.DecoratorCount; i < l; i++) {
				LongDecorator decorator = DLong.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH } ;
				EditorGUI.BeginChangeCheck();

				VariableDecoratorDrawer.OnGUI(rect, decorator, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					DLong.Sort();
					DLong.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				DLong.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				DLong.Add(new LongDecorator());
				EditorUtility.SetDirty(serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(serializedObject.targetObject);

		return DLong;
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

[CustomPropertyDrawer(typeof(DFloat))]
public class DFloatDrawer : DVariableDrawer {
	
	protected override DVariable GetObject(SerializedProperty property) => property.ToObject<DFloat>();
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect pos = position;
		string text = label.text + " = " + property.FindPropertyRelative("_value").floatValue;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		DVariable dVariable = GetObject(property);
		EditorGUI.BeginChangeCheck();
		SerializedProperty realValueProperty = property.FindPropertyRelative("realValue");
		realValueProperty.floatValue = EditorGUI.DelayedFloatField(pos, "[Real Value]", realValueProperty.floatValue);
		if (EditorGUI.EndChangeCheck()) dVariable.Refresh();
		pos.x = position.x + 3 * SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (property.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			SerializedProperty decoratorsProperty = property.FindPropertyRelative("variableDecorators");
			for (int i = 0, l = decoratorsProperty.arraySize; i < l; i++) {
				SerializedProperty decoratorProperty = decoratorsProperty.GetArrayElementAtIndex(i);
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH} ;
				EditorGUI.BeginChangeCheck();

				EditorGUI.PropertyField(rect, decoratorProperty, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					dVariable.Sort();
					dVariable.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dVariable.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				(dVariable as DFloat)?.Add(new FloatDecorator());
				EditorUtility.SetDirty(property.serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
	}
	
	public static DFloat OnGUI(Rect position, DFloat dFloat, GUIContent label, SerializedObject serializedObject) {
		Rect pos = position;
		dFloat = dFloat ?? new DFloat();
		string text = label.text + " = " + dFloat.Value;
		pos.height = FIELD_HEIGHT;
		pos.width = text.Length * CHARACTER_WIDTH;
		EditorGUI.BeginChangeCheck();
		dFloat.isExpanded = EditorGUI.Foldout(pos, dFloat.isExpanded, text);
		bool sizeChanged = EditorGUI.EndChangeCheck();
		pos.x += pos.width + 3 * SPACE;
		pos.width = position.width - pos.width;
		pos.xMax -= 3 * SPACE;
		EditorGUI.BeginChangeCheck();
		float result = EditorGUI.DelayedFloatField(pos, "[Real Value]", dFloat.realValue);
		if (EditorGUI.EndChangeCheck()) {
			dFloat.Value = result;
			dFloat.Refresh();
		}
		
		pos.x = position.x + SPACE;
		pos.xMax = position.xMax - 2 * SPACE;
		if (dFloat.isExpanded) {
			pos.height = FIELD_HEIGHT;
			int delete = -1;

			for (int i = 0, l = dFloat.DecoratorCount; i < l; i++) {
				FloatDecorator decorator = dFloat.variableDecorators[i];
				pos.y += LINE_HEIGHT;
				Rect rect = new Rect(pos) { xMax = pos.xMax - 4 * CHARACTER_WIDTH } ;
				EditorGUI.BeginChangeCheck();

				VariableDecoratorDrawer.OnGUI(rect, decorator, GUIContent.none);
				
				if (EditorGUI.EndChangeCheck()) {
					dFloat.Sort();
					dFloat.Refresh();
				}
				
				rect.x = rect.xMax + 2 * SPACE;
				rect.xMax = pos.xMax;
				if (GUI.Button(rect, "X")) delete = i;
			}

			if (delete != -1) {
				dFloat.Remove(delete);
				sizeChanged = true;
			}
			
			pos.y += LINE_HEIGHT;
			if (GUI.Button(pos, "Add Decorator")) {
				pos.height = LINE_HEIGHT * 5;
				dFloat.Add(new FloatDecorator());
				EditorUtility.SetDirty(serializedObject.targetObject);
				sizeChanged = false;
			}
		}
		
		if (sizeChanged) EditorUtility.SetDirty(serializedObject.targetObject);

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

[CustomPropertyDrawer(typeof(IntDecorator))]
[CustomPropertyDrawer(typeof(LongDecorator))]
[CustomPropertyDrawer(typeof(FloatDecorator))]
public class VariableDecoratorDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => DVariableDrawer.LINE_HEIGHT;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect rect = new Rect(position) { width = label.text.Length * DVariableDrawer.CHARACTER_WIDTH };
		EditorGUI.LabelField(rect, label);

		rect.x += rect.width;
		rect.width = 6f * DVariableDrawer.CHARACTER_WIDTH;
		EditorGUI.LabelField(rect, "Priority");

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.width = 3 * DVariableDrawer.CHARACTER_WIDTH;
		SerializedProperty priorityProperty = property.FindPropertyRelative("priority");
		priorityProperty.intValue = EditorGUI.DelayedIntField(rect, priorityProperty.intValue);

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.width = 5 * DVariableDrawer.CHARACTER_WIDTH;
		SerializedProperty typeProperty = property.FindPropertyRelative("type");
		typeProperty.enumValueIndex = EditorGUI.Popup(rect, typeProperty.enumValueIndex, typeProperty.enumDisplayNames);

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.xMax = position.xMax;
		SerializedProperty valueProperty = property.FindPropertyRelative("value");
		switch (valueProperty.type) {
			case "int": valueProperty.intValue = EditorGUI.DelayedIntField(rect, valueProperty.intValue); break;
			case "long": valueProperty.longValue = EditorGUI.LongField(rect, valueProperty.longValue); break;
			case "float": valueProperty.floatValue = EditorGUI.DelayedFloatField(rect, valueProperty.floatValue); break;
		}
		
		EditorGUI.EndProperty();
	}

	public static T OnGUI<T>(Rect position, T decorator, GUIContent label) where T : VariableDecorator, new () {
		decorator = decorator ?? new T();
		
		Rect rect = new Rect(position) { width = label.text.Length * DVariableDrawer.CHARACTER_WIDTH };
		EditorGUI.LabelField(rect, label);

		rect.x += rect.width;
		rect.width = 6f * DVariableDrawer.CHARACTER_WIDTH;
		EditorGUI.LabelField(rect, "Priority");

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.width = 3 * DVariableDrawer.CHARACTER_WIDTH;
		decorator.priority = EditorGUI.DelayedIntField(rect, decorator.priority);

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.width = 5 * DVariableDrawer.CHARACTER_WIDTH;
		decorator.type = (VariableDecoratorType) EditorGUI.EnumPopup(rect, decorator.type);

		rect.x += rect.width + DVariableDrawer.SPACE;
		rect.xMax = position.xMax;

		if (typeof(T) == typeof(IntDecorator)) (decorator as IntDecorator).value = EditorGUI.DelayedIntField(rect, (decorator as IntDecorator).value);
		else if (typeof(T) == typeof(LongDecorator)) (decorator as LongDecorator).value = EditorGUI.LongField(rect, (decorator as LongDecorator).value);
		else if (typeof(T) == typeof(FloatDecorator)) (decorator as FloatDecorator).value = EditorGUI.DelayedFloatField(rect, (decorator as FloatDecorator).value);

		return decorator;
	}
}