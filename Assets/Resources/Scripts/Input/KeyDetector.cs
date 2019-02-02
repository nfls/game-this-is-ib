using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class KeyDetector : InputDetector {

	private static readonly Dictionary<string, KeyDetector> keyDetectors = new Dictionary<string, KeyDetector>(67);
	private static readonly string[] names = {
		"A",
		"B",
		"C",
		"D",
		"E",
		"F",
		"G",
		"H",
		"I",
		"J",
		"K",
		"L",
		"M",
		"N",
		"O",
		"P",
		"Q",
		"R",
		"S",
		"T",
		"U",
		"V",
		"W",
		"X",
		"Y",
		"Z",
		"Alpha0",
		"Alpha1",
		"Alpha2",
		"Alpha3",
		"Alpha4",
		"Alpha5",
		"Alpha6",
		"Alpha7",
		"Alpha8",
		"Alpha9",
		"Tab",
		"Backspace",
		"Return",
		"Escape",
		"Space",
		"UpArrow",
		"DownArrow",
		"RightArrow",
		"LeftArrow",
		"CapsLock",
		"RightShift",
		"LeftShift",
		"RightControl",
		"LeftControl",
		"RightAlt",
		"LeftAlt",
		"RightApple",
		"LeftApple",
		"RightCommand",
		"LeftCommand",
		"LeftWindows",
		"RightWindows",
		"LeftBracket",
		"RightBracket",
		"Slash",
		"Backslash",
		"Colon",
		"Semicolon",
		"Quote",
		"BackQuote",
		"Underscore"
	};

	static KeyDetector() {
		foreach (string name in names) keyDetectors[name] = new KeyDetector(name);
	}

	private readonly KeyCode _keyCode;

	private KeyDetector(string name) {
		_name = name;
		_keyCode = name.ToEnum<KeyCode>();
	}

	public override void Refresh() {
		_isPressed = false;
		_isReleased = false;
		if (Input.GetKeyDown(_keyCode)) {
			_isPressed = true;
			_isHeld = true;
			_lastPressTime = Time.time;
		} else if (Input.GetKeyUp(_keyCode)) {
			_isReleased = true;
			_isHeld = false;
		}
	}

	public static KeyDetector ToKeyDetector(string name) {
		return keyDetectors.ContainsKey(name) ? keyDetectors[name] : null;
	}

	public static KeyDetector GetHeldDetector() {
		foreach (KeyDetector detector in keyDetectors.Values) {
			if (Input.GetKey(detector._keyCode)) {
				Debug.Log(detector.Name);
				return detector;
			}
		}

		return null;
	}

	public static bool operator ==(KeyDetector lhs, KeyDetector rhs) => ReferenceEquals(lhs, rhs) || !ReferenceEquals(lhs, null) && !ReferenceEquals(rhs, null) && lhs._keyCode == rhs._keyCode;

	public static bool operator !=(KeyDetector lhs, KeyDetector rhs) => !(lhs == rhs);
}