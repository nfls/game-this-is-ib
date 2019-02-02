using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
	public const string SWITCH_UP = "Switch Up";
	public const string SWITCH_DOWN = "Switch Down";
	public const string SWITCH_CAMERA = "Swtich Camera";
	public const string PAUSE = "Pause";

	public static readonly Dictionary<string, InputDetector> defaultSwitchProControllerMap;
	public static readonly Dictionary<string, InputDetector> defaultKeyboardMap;
	
	public delegate void OnPressHandler();
	public delegate void OnHoldHandler();
	public delegate void OnReleaseHandler();

	public bool isInControl;

	public int Count => _inputMap.Count;

	private Dictionary<string, InputDetector> _defaultMap;
	private Dictionary<string, OnPressHandler> _onPressedBindings;
	private Dictionary<string, OnHoldHandler> _onHeldBindings;
	private Dictionary<string, OnReleaseHandler> _onReleasedBindings;
	private Dictionary<string, InputDetector> _inputMap;

	static InputMapper() {
		defaultSwitchProControllerMap = new Dictionary<string, InputDetector>(11) {
			[MOVE_LEFT] = JoystickAxisDetector.ToJoystickAxisDetector("Joystick0Axis0Negative"),
			[MOVE_RIGHT] = JoystickAxisDetector.ToJoystickAxisDetector("Joystick0Axis0Positive"),
			[ACCELERATE] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button10"),
			[JUMP] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button0"),
			[ATTACK] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button2"),
			[DODGE] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button1"),
			[INTERACT] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button3"),
			[RECOVER] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button7"),
			[SWITCH_PREV] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button13"),
			[SWITCH_NEXT] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button14"),
			[SWITCH_UP] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button11"),
			[SWITCH_DOWN] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button12"),
			[SWITCH_CAMERA] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button9"),
			[PAUSE] = JoystickButtonDetector.ToJoystickButtonDetector("Joystick0Button8")
		};
		
		defaultKeyboardMap = new Dictionary<string, InputDetector>(11) {
			[MOVE_LEFT] = KeyDetector.ToKeyDetector("A"),
			[MOVE_RIGHT] = KeyDetector.ToKeyDetector("D"),
			[ACCELERATE] = KeyDetector.ToKeyDetector("N"),
			[JUMP] = KeyDetector.ToKeyDetector("W"),
			[ATTACK] = KeyDetector.ToKeyDetector("J"),
			[DODGE] = KeyDetector.ToKeyDetector("K"),
			[INTERACT] = KeyDetector.ToKeyDetector("U"),
			[RECOVER] = KeyDetector.ToKeyDetector("F"),
			[SWITCH_PREV] = KeyDetector.ToKeyDetector("Q"),
			[SWITCH_NEXT] = KeyDetector.ToKeyDetector("E"),
			[SWITCH_UP] = KeyDetector.ToKeyDetector("Y"),
			[SWITCH_DOWN] = KeyDetector.ToKeyDetector("H"),
			[SWITCH_CAMERA] = KeyDetector.ToKeyDetector("Tab"),
			[PAUSE] = KeyDetector.ToKeyDetector("Escape")
		};
	}

	public InputMapper() {
		_inputMap = new Dictionary<string, InputDetector>(10);
		_onPressedBindings = new Dictionary<string, OnPressHandler>(10);
		_onHeldBindings = new Dictionary<string, OnHoldHandler>(10);
		_onReleasedBindings = new Dictionary<string, OnReleaseHandler>(10);
	}

	public InputMapper(Dictionary<string, InputDetector> defaultMap) : this() {
		_defaultMap = defaultMap;
		Reset();
	}

	public bool marked;

	public void Refresh() {
		if (isInControl) {
			foreach (string name in _onPressedBindings.Keys)
				if (_inputMap[name].IsPressed) _onPressedBindings[name]();
			foreach (string name in _onHeldBindings.Keys)
				if (_inputMap[name].IsHeld) _onHeldBindings[name]();
			foreach (string name in _onReleasedBindings.Keys)
				if (_inputMap[name].IsReleased) _onReleasedBindings[name]();
		}
	}

	public void Reset() {
		_inputMap.Clear();
		foreach (var pair in _defaultMap) Remap(pair.Key, pair.Value);
	}

	public void Remap(string name, InputDetector detector) {
		if (_inputMap.ContainsKey(name)) InputManager.NotifyDetectorOnIdle(_inputMap[name]);
		if (detector != null) {
			InputManager.NotifyDetectorOnBusy(detector);
			_inputMap[name] = detector;
		} else _inputMap.Remove(name);
	}
	
	public bool BindPressEvent(string name, OnPressHandler e) {
		bool binded = _onPressedBindings.ContainsKey(name);
		_onPressedBindings[name] = e;
		return binded;
	}

	public bool BindHoldEvent(string name, OnHoldHandler e) {
		bool binded = _onHeldBindings.ContainsKey(name);
		_onHeldBindings[name] = e;
		return binded;
	}

	public bool BindReleaseEvent(string name, OnReleaseHandler e) {
		bool binded = _onReleasedBindings.ContainsKey(name);
		_onReleasedBindings[name] = e;
		return binded;
	}

	public bool UnbindPressEvent(string name) {
		if (!_onPressedBindings.ContainsKey(name)) return false;
		_onPressedBindings.Remove(name);
		return true;
	}

	public bool UnbindHoldEvent(string name) {
		if (!_onHeldBindings.ContainsKey(name)) return false;
		_onHeldBindings.Remove(name);
		return true;
	}

	public bool UnbindReleaseEvent(string name) {
		if (!_onReleasedBindings.ContainsKey(name)) return false;
		_onReleasedBindings.Remove(name);
		return true;
	}

	public OnPressHandler GetPressEvent(string name) {
		if (_onPressedBindings.ContainsKey(name)) return _onPressedBindings[name];
		return null;
	}

	public OnHoldHandler GetHoldEvent(string name) {
		if (_onHeldBindings.ContainsKey(name)) return _onHeldBindings[name];
		return null;
	}

	public OnReleaseHandler GetReleaseEvent(string name) {
		if (_onReleasedBindings.ContainsKey(name)) return _onReleasedBindings[name];
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