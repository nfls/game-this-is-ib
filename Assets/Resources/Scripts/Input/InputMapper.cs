using System.Collections.Generic;
using UnityEngine;

public class InputMapper {

	public const string MOVE_LEFT = "Move Left";
	public const string MOVE_RIGHT = "Move Right";
	public const string ACCELERATE = "Accelerate";
	public const string JUMP = "Jump";
	public const string ATTACK = "Attack";
	public const string DODGE = "Dodge";
	public const string INTERACT = "Interact";
	public const string RECOVER = "Recover";
	public const string SWITCH_PREV = "Switch Prev";
	public const string SWITCH_NEXT = "Switch Next";
	public const string SWITCH_CAMERA = "Swtich Camera";
	public const string PAUSE = "Pause";
	
	private static readonly Dictionary<string, InputDetector> OSXDefaultXboxOneMap;
	private static readonly Dictionary<string, InputDetector> defaultKeyboardMap;
	
	public delegate void OnPressed();
	public delegate void OnHeld();
	public delegate void OnReleased();

	public bool isInControl;

	public int Count {
		get { return _inputMap.Count; }
	}
	
	private Dictionary<string, OnPressed> _onPressedBindings;
	private Dictionary<string, OnHeld> _onHeldBindings;
	private Dictionary<string, OnReleased> _onReleasedBindings;
	private Dictionary<string, InputDetector> _inputMap;

	static InputMapper() {
		OSXDefaultXboxOneMap = new Dictionary<string, InputDetector>(10);
		OSXDefaultXboxOneMap[MOVE_LEFT] = AxisDetector.ToAxisDetector("Axis1st Negative");
		OSXDefaultXboxOneMap[MOVE_RIGHT] = AxisDetector.ToAxisDetector("Axis1st Positive");
		OSXDefaultXboxOneMap[ACCELERATE] = KeyDetector.ToKeyDetector("Joystick1Button8");
		OSXDefaultXboxOneMap[JUMP] = KeyDetector.ToKeyDetector("Joystick1Button1");
		OSXDefaultXboxOneMap[ATTACK] = KeyDetector.ToKeyDetector("Joystick1Button4");
		OSXDefaultXboxOneMap[DODGE] = KeyDetector.ToKeyDetector("Joystick1Button2");
		OSXDefaultXboxOneMap[INTERACT] = KeyDetector.ToKeyDetector("Joystick1Button5");
		OSXDefaultXboxOneMap[RECOVER] = KeyDetector.ToKeyDetector("Joystick1Button7");
		OSXDefaultXboxOneMap[SWITCH_PREV] = AxisDetector.ToAxisDetector("Axis3rd Negative");
		OSXDefaultXboxOneMap[SWITCH_NEXT] = AxisDetector.ToAxisDetector("Axis3rd Positive");
		OSXDefaultXboxOneMap[SWITCH_CAMERA] = KeyDetector.ToKeyDetector("Joystick1Button7");
		OSXDefaultXboxOneMap[PAUSE] = KeyDetector.ToKeyDetector("Joystick1Button16");
		
		defaultKeyboardMap = new Dictionary<string, InputDetector>(10);
		defaultKeyboardMap[MOVE_LEFT] = KeyDetector.ToKeyDetector("A");
		defaultKeyboardMap[MOVE_RIGHT] = KeyDetector.ToKeyDetector("D");
		defaultKeyboardMap[ACCELERATE] = KeyDetector.ToKeyDetector("N");
		defaultKeyboardMap[JUMP] = KeyDetector.ToKeyDetector("K");
		defaultKeyboardMap[ATTACK] = KeyDetector.ToKeyDetector("J");
		defaultKeyboardMap[DODGE] = KeyDetector.ToKeyDetector("L");
		defaultKeyboardMap[INTERACT] = KeyDetector.ToKeyDetector("U");
		defaultKeyboardMap[RECOVER] = KeyDetector.ToKeyDetector("H");
		defaultKeyboardMap[SWITCH_PREV] = KeyDetector.ToKeyDetector("Q");
		defaultKeyboardMap[SWITCH_NEXT] = KeyDetector.ToKeyDetector("E");
		defaultKeyboardMap[SWITCH_CAMERA] = KeyDetector.ToKeyDetector("Tab");
		defaultKeyboardMap[PAUSE] = KeyDetector.ToKeyDetector("Escape");
	}

	public InputMapper() {
		_inputMap = new Dictionary<string, InputDetector>(10);
		_onPressedBindings = new Dictionary<string, OnPressed>(10);
		_onHeldBindings = new Dictionary<string, OnHeld>(10);
		_onReleasedBindings = new Dictionary<string, OnReleased>(10);
		Reset();
	}

	public void Refresh() {
		if (isInControl) {
			foreach (string name in _onPressedBindings.Keys) {
				if (_inputMap[name].IsPressed) {
					_onPressedBindings[name]();
				}
			}
			foreach (string name in _onHeldBindings.Keys) {
				if (_inputMap[name].IsHeld) {
					_onHeldBindings[name]();
				}
			}
			foreach (string name in _onReleasedBindings.Keys) {
				if (_inputMap[name].IsReleased) _onReleasedBindings[name]();
			}
		}
	}

	public void Reset() {
		_inputMap.Clear();
		foreach (var pair in OSXDefaultXboxOneMap) {
			Remap(pair.Key, pair.Value);
		}
	}

	public void Remap(string name, InputDetector detector) {
		if (_inputMap.ContainsKey(name)) {
			InputManager.NotifyDetectorOnIdle(_inputMap[name]);
			return;
		}

		InputManager.NotifyDetectorOnBusy(detector);
		_inputMap[name] = detector;
	}
	
	public bool BindPressEvent(string name, OnPressed e) {
		bool binded = _onPressedBindings.ContainsKey(name);
		_onPressedBindings[name] = e;
		return binded;
	}

	public bool BindHoldEvent(string name, OnHeld e) {
		bool binded = _onHeldBindings.ContainsKey(name);
		_onHeldBindings[name] = e;
		return binded;
	}

	public bool BindReleaseEvent(string name, OnReleased e) {
		bool binded = _onReleasedBindings.ContainsKey(name);
		_onReleasedBindings[name] = e;
		return binded;
	}

	public bool UnbindPressEvent(string name) {
		if (!_onPressedBindings.ContainsKey(name)) {
			return false;
		}
		_onPressedBindings.Remove(name);
		return true;
	}

	public bool UnbindHoldEvent(string name) {
		if (!_onHeldBindings.ContainsKey(name)) {
			return false;
		}
		_onHeldBindings.Remove(name);
		return true;
	}

	public bool UnbindReleaseEvent(string name) {
		if (!_onReleasedBindings.ContainsKey(name)) {
			return false;
		}
		_onReleasedBindings.Remove(name);
		return true;
	}

	public OnPressed GetPressEvent(string name) {
		if (_onPressedBindings.ContainsKey(name)) {
			return _onPressedBindings[name];
		}
		return null;
	}

	public OnHeld GetHoldEvent(string name) {
		if (_onHeldBindings.ContainsKey(name)) {
			return _onHeldBindings[name];
		}
		return null;
	}

	public OnReleased GetReleaseEvent(string name) {
		if (_onReleasedBindings.ContainsKey(name)) {
			return _onReleasedBindings[name];
		}
		return null;
	}

	public InputDetector this[string name] {
		get {
			if (!_inputMap.ContainsKey(name)) {
				new UnityException("Cannot Find Input Named [" + name + "] !");
				return null;
			}
			return _inputMap[name];
		}
		set { Remap(name, value); }
	}
}