using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class JoystickButtonDetector : JoystickInputDetector {
	
	private static readonly Dictionary<string, JoystickButtonDetector> joystickButtonDetectors = new Dictionary<string, JoystickButtonDetector>(15);
	
	private JoystickButtonDetector(int joystickIndex, int inputIndex) : base(joystickIndex, inputIndex) {
		StringBuilder builder = new StringBuilder("Joystick", 16);
		builder.Append(joystickIndex);
		builder.Append("Button");
		builder.Append(inputIndex);
		_name = builder.ToString();
	}

	public override void Refresh() {
		_isPressed = false;
		_isReleased = false;
		bool flag;
		bool isValid = JoystickUtil.GetButton(_joystickIndex, _inputIndex, out flag);
		if (isValid) {
			if (flag && !_isHeld) {
				_isPressed = true;
				_isHeld = true;
				_lastPressTime = Time.time;
			} else if (!flag && _isHeld) {
				_isReleased = true;
				_isHeld = false;
			}
		} else if (_isValid) {
			_isValid = false;
			_isHeld = false;
		}
	}

	private static JoystickButtonDetector AddNewJoystickButtonDetector(string name) {
		int kIndex = name.IndexOf("k");
		int BIndex = name.IndexOf("B");
		int nIndex = name.IndexOf("n");
		int joystickIndex = Int32.Parse(name.Substring(kIndex + 1, BIndex - kIndex - 1));
		int inputIndex = Int32.Parse(name.Substring(nIndex + 1, name.Length - nIndex - 1));
		JoystickButtonDetector detector = new JoystickButtonDetector(joystickIndex, inputIndex);
		joystickButtonDetectors.Add(name, detector);
		return detector;
	}
	
	public static JoystickButtonDetector ToJoystickButtonDetector(string name) => joystickButtonDetectors.ContainsKey(name) ? joystickButtonDetectors[name] : AddNewJoystickButtonDetector(name);

	public static bool operator ==(JoystickButtonDetector lhs, JoystickButtonDetector rhs) => ReferenceEquals(lhs, rhs) || !ReferenceEquals(lhs, null) && !ReferenceEquals(rhs, null) && lhs._joystickIndex == rhs._joystickIndex && lhs._inputIndex == rhs._inputIndex;

	public static bool operator !=(JoystickButtonDetector lhs, JoystickButtonDetector rhs) => !(lhs == rhs);
}