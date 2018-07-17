using UnityEditor;

public static class InputManagerTool {

	[MenuItem("这就是IB/输入管理/自动配置虚拟轴")]
	public static void AutoConfigAxes() {
		for (int i = 1; i <= 28; i ++) {
			string str = i.ToString();
			switch (i) {
				case 1: str += "st";
					break;
				case 2: str += "nd";
					break;
				case 3: str += "rd";
					break;
				case 21: str += "st";
					break;
				case 22: str += "nd";
					break;
				case 23: str += "rd";
					break;
				default: str += "th";
					break;
			}

			str += " axis";
			AddAxis(new InputAxis {
				name = str,
				sensitivity = 1f,
				type = AxisType.JoystickAxis,
				axis = i,
				joyNum = 0
			});
		}
	}

	public static void AddAxis(InputAxis axis) {
		if (IsAxisDefined(axis.name)) return;

		var serializedObject =
			new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		var axesProperty = serializedObject.FindProperty("m_Axes");

		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();

		var axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

		GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int) axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

		serializedObject.ApplyModifiedProperties();
	}
	
	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
		var child = parent.Copy();
		child.Next(true);
		do {
			if (child.name == name) return child;
		} while (child.Next(false));

		return null;
	}

	private static bool IsAxisDefined(string axisName) {
		var serializedObject =
			new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		var axesProperty = serializedObject.FindProperty("m_Axes");

		axesProperty.Next(true);
		axesProperty.Next(true);
		while (axesProperty.Next(false)) {
			var axis = axesProperty.Copy();
			axis.Next(true);
			if (axis.stringValue == axisName) return true;
		}

		return false;
	}

	public class InputAxis {
		public string altNegativeButton;
		public string altPositiveButton;

		public int axis;
		public float dead;
		public string descriptiveName;
		public string descriptiveNegativeName;

		public float gravity;
		public bool invert = false;
		public int joyNum;
		public string name;
		public string negativeButton;
		public string positiveButton;
		public float sensitivity;

		public bool snap = false;

		public AxisType type;
	}
	
	public enum AxisType {
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	}
}