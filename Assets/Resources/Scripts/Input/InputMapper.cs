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
	
	public static readonly Dictionary<string, InputDetector> defaultMacXboxOneMap;
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
		defaultMacXboxOneMap =
			new Dictionary<string, InputDetector>(11) {
				[MOVE_LEFT] = AxisDetector.ToAxisDetector("Axis1st Negative"),
				[MOVE_RIGHT] = AxisDetector.ToAxisDetector("Axis1st Positive"),
				[ACCELERATE] = KeyDetector.ToKeyDetector("Joystick1Button14"),
				[JUMP] = KeyDetector.ToKeyDetector("Joystick1Button16"),
				[ATTACK] = KeyDetector.ToKeyDetector("Joystick1Button18"),
				[DODGE] = KeyDetector.ToKeyDetector("Joystick1Button17"),
				[INTERACT] = KeyDetector.ToKeyDetector("Joystick1Button19"),
				[RECOVER] = KeyDetector.ToKeyDetector("Joystick1Button13"),
				[SWITCH_PREV] = KeyDetector.ToKeyDetector("Joystick1Button7"),
				[SWITCH_NEXT] = KeyDetector.ToKeyDetector("Joystick1Button8"),
				[SWITCH_UP] = KeyDetector.ToKeyDetector("Joystick1Button5"),
				[SWITCH_DOWN] = KeyDetector.ToKeyDetector("Joystick1Button6"),
				[SWITCH_CAMERA] = KeyDetector.ToKeyDetector("Joystick1Button12"),
				[PAUSE] = KeyDetector.ToKeyDetector("Joystick1Button10")
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

	public bool marked;

	public InputMapper(Dictionary<string, InputDetector> defaultMap) {
		_inputMap = new Dictionary<string, InputDetector>(10);
		_onPressedBindings = new Dictionary<string, OnPressHandler>(10);
		_onHeldBindings = new Dictionary<string, OnHoldHandler>(10);
		_onReleasedBindings = new Dictionary<string, OnReleaseHandler>(10);

		_defaultMap = defaultMap;
		Reset();
	}

	public void Refresh() {
		if (isInControl)
			foreach (string name in _onPressedBindings.Keys)
				if (_inputMap[name].IsPressed) _onPressedBindings[name]();
			foreach (string name in _onHeldBindings.Keys)
				if (_inputMap[name].IsHeld) _onHeldBindings[name]();
			foreach (string name in _onReleasedBindings.Keys)
				if (_inputMap[name].IsReleased) _onReleasedBindings[name]();
	}

	public void UDebug() {
		Debug.Log(_onPressedBindings.Count);
		Debug.Log(_onHeldBindings.Count);
		Debug.Log(_onReleasedBindings.Count);
	}

	public void Reset() {
		_inputMap.Clear();
		foreach (var pair in _defaultMap) Remap(pair.Key, pair.Value);
	}

	public void Remap(string name, InputDetector detector) {
		if (_inputMap.ContainsKey(name)) {
			InputManager.NotifyDetectorOnIdle(_inputMap[name]);
			return;
		}

		InputManager.NotifyDetectorOnBusy(detector);
		_inputMap[name] = detector;
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