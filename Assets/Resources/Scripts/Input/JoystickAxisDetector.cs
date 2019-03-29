using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class JoystickAxisDetector : JoystickInputDetector {
	
	private static readonly Dictionary<string, JoystickAxisDetector> joystickAxisDetectors = new Dictionary<string, JoystickAxisDetector>(6);
	
	private float _deadZone;
	private JoystickAxisDirection _direction;
	private float _offset;
	
	public JoystickAxisDirection Direction => _direction;
	public float Offset => _offset;

	private JoystickAxisDetector(int joystickIndex, int inputIndex, JoystickAxisDirection direction, float deadZone = .15f) : base(joystickIndex, inputIndex) {
		_direction = direction;
		_deadZone = deadZone;
		StringBuilder builder = new StringBuilder("Joystick", 16);
		builder.Append(joystickIndex);
		builder.Append("Axis");
		builder.Append(inputIndex);
		builder.Append((int) direction);
		_name = builder.ToString();
	}
	
	public override void Refresh() {
		_isPressed = false;
		_isReleased = false;
		bool isValid = JoystickUtil.GetAxis(_joystickIndex, _inputIndex, out _offset);
		if (isValid) {
			bool flag = _offset * (float) _direction - _deadZone > 0;
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
			_offset = 0f;
		}
	}
	
	private static JoystickAxisDetector AddNewJoystickAxisDetector(string name) {
		int kIndex = name.IndexOf("k");
		int AIndex = name.IndexOf("A");
		int joystickIndex = Int32.Parse(name.Substring(kIndex + 1, AIndex - kIndex - 1));
		name = name.Substring(kIndex + 1, name.Length - kIndex - 1);
		int sIndex = name.IndexOf("s");
		int inputIndex;
		JoystickAxisDirection direction;
		if (name.EndsWith("Negative")) {
			int NIndex = name.IndexOf("N");
			inputIndex = Int32.Parse(name.Substring(sIndex + 1, NIndex - sIndex - 1));
			direction = JoystickAxisDirection.Negative;
		} else {
			int PIndex = name.IndexOf("P");
			inputIndex = Int32.Parse(name.Substring(sIndex + 1, PIndex - sIndex - 1));
			direction = JoystickAxisDirection.Positive;
		}
		
		JoystickAxisDetector detector = new JoystickAxisDetector(joystickIndex, inputIndex, direction);
		joystickAxisDetectors.Add(name, detector);
		return detector;
	}
	
	public static JoystickAxisDetector ToJoystickAxisDetector(string name) => joystickAxisDetectors.ContainsKey(name) ? joystickAxisDetectors[name] : AddNewJoystickAxisDetector(name);
	
	public static bool operator ==(JoystickAxisDetector lhs,JoystickAxisDetector rhs) => ReferenceEquals(lhs, rhs) || !ReferenceEquals(lhs, null) && !ReferenceEquals(rhs, null) && lhs._name.Equals(rhs._name, StringComparison.CurrentCultureIgnoreCase);

	public static bool operator !=(JoystickAxisDetector lhs, JoystickAxisDetector rhs) => !(lhs == rhs);
}

public enum JoystickAxisDirection {
	Negative = -1,
	Positive = 1
}