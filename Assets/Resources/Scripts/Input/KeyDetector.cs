using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class KeyDetector : InputDetector {

	private static readonly Dictionary<string, KeyDetector> keyDetectors;

	static KeyDetector() {
		keyDetectors = new Dictionary<string, KeyDetector>(184);

		string[] names = Enum.GetNames(typeof(KeyCode));

		foreach (string name in names) {
			keyDetectors[name] = new KeyDetector(name);
		}
	}

	private readonly KeyCode keyCode;

	private KeyDetector(string name) {
		_name = name;
		keyCode = name.ToEnum<KeyCode>();
	}

	public override void Refresh() {
		_isPressed = false;
		_isReleased = false;
		if (Input.GetKeyDown(keyCode)) {
			_isPressed = true;
			_isHeld = true;
			_lastPressTime = Time.time;
		} else if (Input.GetKeyUp(keyCode)) {
			_isReleased = true;
			_isHeld = false;
		}
	}

	public static KeyDetector ToKeyDetector(string name) {
		return keyDetectors.ContainsKey(name) ? keyDetectors[name] : null;
	}

	public static KeyDetector getHelDetector() {
		foreach (KeyDetector detector in keyDetectors.Values) {
			if (Input.GetKey(detector.keyCode)) {
				return detector;
			}
		}

		return null;
	}

	public static bool operator ==(KeyDetector lhs, KeyDetector rhs) {
		return lhs.keyCode == rhs.keyCode;
	}

	public static bool operator !=(KeyDetector lhs, KeyDetector rhs) {
		return lhs.keyCode != rhs.keyCode;
	}
}