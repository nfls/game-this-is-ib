using System.Collections.Generic;

public class KeyboardMapper : IInputMapper {

	static readonly Dictionary<InputMap, KeyDetector> defaultInputMap;

	Dictionary<InputMap, KeyDetector> inputMap;

	static KeyboardMapper() {
		defaultInputMap = new Dictionary<InputMap, KeyDetector>(InputController.INPUT_MAP_NUM);
		defaultInputMap[InputMap.MoveLeft] = KeyDetector.A;
		defaultInputMap[InputMap.MoveRight] = KeyDetector.D;
		defaultInputMap[InputMap.Jump] = KeyDetector.W;
		defaultInputMap[InputMap.Accelerate] = KeyDetector.LeftShift;
		defaultInputMap[InputMap.Attack] = KeyDetector.J;
		defaultInputMap[InputMap.Dodge] = KeyDetector.L;
		defaultInputMap[InputMap.Parry] = KeyDetector.K;
		defaultInputMap[InputMap.Interact] = KeyDetector.F;
		defaultInputMap[InputMap.SwitchPrev] = KeyDetector.Q;
		defaultInputMap[InputMap.SwitchNext] = KeyDetector.E;
		defaultInputMap[InputMap.Pause] = KeyDetector.Escape;
	}

	public KeyboardMapper() {
		inputMap = new Dictionary<InputMap, KeyDetector>(InputController.INPUT_MAP_NUM);
		Reset();
	}

	public void Refresh() {
		foreach (KeyDetector detector in inputMap.Values) {
			detector.Refresh();
		}
	}

	public void Remap(InputMap toMap, string input) {
		Remap(toMap, KeyDetector.ToKeyDector(input));
	}

	public void Remap(InputMap toMap, KeyDetector keyDetector) {
		if (inputMap.ContainsValue(keyDetector)) {
			foreach (var pair in inputMap) {
				if (pair.Value == keyDetector) {
					inputMap[pair.Key] = inputMap[toMap];
					break;
				}
			}
		}
		inputMap[toMap] = keyDetector;
	}

	public void Reset() {
		foreach (var pair in defaultInputMap) {
			Remap(pair.Key, pair.Value);
		}
	}

	public KeyDetector this[InputMap map] {
		get {
			return inputMap[map];
		}
		set {
			Remap(map, value);
		}
	}
}
