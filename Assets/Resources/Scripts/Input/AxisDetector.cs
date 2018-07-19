using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AxisDetector : InputDetector {
	
	private static readonly Dictionary<string, AxisDetector> axisDetectors;

	static AxisDetector() {
		axisDetectors = new Dictionary<string, AxisDetector>(56);

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
			AxisDetector positiveDetector = new AxisDetector(str + " axis", AxisDirection.Positive);
			AxisDetector negativeDetector = new AxisDetector(str + " axis", AxisDirection.Negative);

			axisDetectors["Axis" + str + " Positive"] = positiveDetector;
			axisDetectors["Axis" + str + " Negative"] = negativeDetector;
		}
	}
	
	public AxisDirection Direction => _direction;
	public float Offset => _offset;

	private float _deadZone;
	private AxisDirection _direction;
	private float _offset;

	private AxisDetector(string name, AxisDirection direction, float deadZone = .15f) {
		_name = name;
		_direction = direction;
		_deadZone = deadZone;
	}

	public override void Refresh() {
		_offset = Input.GetAxis(_name);
		_isPressed = false;
		_isReleased = false;
		bool flag = _offset * (float) _direction - _deadZone > 0;
		if (_isHeld) {
			if (!flag) {
				_isReleased = true;
				_isHeld = false;
			}
		} else {
			if (flag) {
				_isPressed = true;
				_isHeld = true;
				_lastPressTime = Time.time;
			}
		}
	}

	public static AxisDetector ToAxisDetector(string name) {
		return axisDetectors.ContainsKey(name) ? axisDetectors[name] : null;
	}

	public static AxisDetector GetHeldDetector() {
		foreach (AxisDetector detector in axisDetectors.Values) {
			if (Input.GetAxis(detector.Name) * (float) detector._direction > detector._deadZone) {
				Debug.Log(detector.Name + " | " + detector.Direction);
				return detector;
			}
		}

		return null;
	}

	public static bool operator ==(AxisDetector lhs, AxisDetector rhs) {
		return lhs._name.Equals(rhs._name, StringComparison.CurrentCultureIgnoreCase);
	}

	public static bool operator !=(AxisDetector lhs, AxisDetector rhs) {
		return !lhs._name.Equals(rhs._name, StringComparison.CurrentCultureIgnoreCase);
	}
}

public enum AxisDirection {
	Negative = -1,
	Positive = 1
}
