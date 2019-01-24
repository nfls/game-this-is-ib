using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorExtensions {

	private static readonly string[] DecoratorNames = { "Add", "Sub", "Mul", "Div", "None" };

	public static IntDecorator IntDecoratorPopup(Rect position) {
		IntDecorator decorator = null;
		int i = EditorGUI.Popup(position, 0, DecoratorNames);
		switch (i) {
			case 0: decorator = new IntAdd(); break;
			case 1: decorator = new IntSub(); break;
			case 2: decorator = new IntMul(); break;
			case 3: decorator = new IntDiv(); break;
		}
		
		return decorator;
	}
	
	public static LongDecorator LongDecoratorPopup(Rect position, string label) {
		LongDecorator decorator = null;
		int i = EditorGUI.Popup(position, label, 0, DecoratorNames);
		switch (i) {
			case 0: decorator = new LongAdd(); break;
			case 1: decorator = new LongSub(); break;
			case 2: decorator = new LongMul(); break;
			case 3: decorator = new LongDiv(); break;
		}
		
		return decorator;
	}
	
	public static FloatDecorator FloatDecoratorPopup(Rect position, string label) {
		FloatDecorator decorator = null;
		int i = EditorGUI.Popup(position, label, 0, DecoratorNames);
		switch (i) {
			case 0: decorator = new FloatAdd(); break;
			case 1: decorator = new FloatSub(); break;
			case 2: decorator = new FloatMul(); break;
			case 3: decorator = new FloatDiv(); break;
		}
		
		return decorator;
	}

	public static T ToObject<T>(this SerializedProperty prop) => (T) GetTargetObjectOfProperty(prop);
	
	public static object GetTargetObjectOfProperty(SerializedProperty prop) {
		var path = prop.propertyPath.Replace(".Array.data[", "[");
		object obj = prop.serializedObject.targetObject;
		var elements = path.Split('.');
		foreach (var element in elements) {
			if (element.Contains("[")) {
				var elementName = element.Substring(0, element.IndexOf("["));
				var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
				obj = GetValue_Imp(obj, elementName, index);
			} else obj = GetValue_Imp(obj, element);
		}
		
		return obj;
	}
	
	private static object GetValue_Imp(object source, string name) {
		if (source == null) return null;
		var type = source.GetType();
		while (type != null) {
			var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (f != null) return f.GetValue(source);
			var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (p != null) return p.GetValue(source, null);
			type = type.BaseType;
		}
		
		return null;
	}

	private static object GetValue_Imp(object source, string name, int index) {
		var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
		if (enumerable == null) return null;
		var enm = enumerable.GetEnumerator();
		for (int i = 0; i <= index; i++) if (!enm.MoveNext()) return null;
		return enm.Current;
	}
}