using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public const int INPUT_MAP_NUM = 11;

	public int playerId;

	public delegate void OnPressed();
	public delegate void OnDoublePressed();
	public delegate void OnHeld();
	public delegate void OnReleased();
	public delegate void OnCharged(float seconds);

	Dictionary<InputMap, OnPressed> onPressedBindings;
	Dictionary<InputMap, OnDoublePressed> onDoublePressedBindings;
	Dictionary<InputMap, OnHeld> onHeldBindings;
	Dictionary<InputMap, OnReleased> onReleasedBindings;
	Dictionary<InputMap, OnCharged> onChargedBindings;

	bool inControl;
	IInputMapper inputMapper;

	List<InputMap> temperMap;

	public void Init(int playerId, IInputMapper inputMapper) {
		this.playerId = playerId;
		this.inputMapper = inputMapper;

		temperMap = new List<InputMap>(INPUT_MAP_NUM);

		onPressedBindings = new Dictionary<InputMap, OnPressed>(INPUT_MAP_NUM);
		onDoublePressedBindings = new Dictionary<InputMap, OnDoublePressed>(INPUT_MAP_NUM);
		onHeldBindings = new Dictionary<InputMap, OnHeld>(INPUT_MAP_NUM);
		onReleasedBindings = new Dictionary<InputMap, OnReleased>(INPUT_MAP_NUM);
		onChargedBindings = new Dictionary<InputMap, OnCharged>(INPUT_MAP_NUM);

		inControl = true;
	}

	void Update() {

		if (inControl) {
			inputMapper.Refresh();
		}
		foreach (InputMap map in onPressedBindings.Keys) {
			if (inputMapper[map].IsPressed) {
				if (!inputMapper[map].IsDoublePressed) {
					onPressedBindings[map]();
				}
			}
		}
		foreach (InputMap map in onDoublePressedBindings.Keys) {
			if (inputMapper[map].IsDoublePressed) {
				onDoublePressedBindings[map]();
			}
		}
		foreach (InputMap map in onHeldBindings.Keys) {
			if (inputMapper[map].IsHeld) {
				onHeldBindings[map]();
			}
		}
		foreach (InputMap map in onReleasedBindings.Keys) {
			if (inputMapper[map].IsReleased) {
				if (!inputMapper[map].IsCharged) {
					onReleasedBindings[map]();
				}
			}
		}
		foreach (InputMap map in onChargedBindings.Keys) {
			if (inputMapper[map].IsCharged) {
				onChargedBindings[map](inputMapper[map].ChargeTime);
			}
		}
	}

	public bool BindPressEvent(InputMap map, OnPressed e) {
		bool binded = onPressedBindings.ContainsKey(map);
		onPressedBindings[map] = e;
		return binded;
	}

	public bool BindDoublePressEvent(InputMap map, OnDoublePressed e) {
		bool binded = onDoublePressedBindings.ContainsKey(map);
		onDoublePressedBindings[map] = e;
		return binded;
	}

	public bool BindHoldEvent(InputMap map, OnHeld e) {
		bool binded = onHeldBindings.ContainsKey(map);
		onHeldBindings[map] = e;
		return binded;
	}

	public bool BindReleaseEvent(InputMap map, OnReleased e) {
		bool binded = onReleasedBindings.ContainsKey(map);
		onReleasedBindings[map] = e;
		return binded;
	}

	public bool BindChargeEvent(InputMap map, OnCharged e) {
		bool binded = onChargedBindings.ContainsKey(map);
		onChargedBindings[map] = e;
		return binded;
	}

	public bool UnbindPressEvent(InputMap map) {
		if (!onPressedBindings.ContainsKey(map)) {
			return false;
		}
		onPressedBindings.Remove(map);
		return true;
	}

	public bool UnbindDoublePressEvent(InputMap map) {
		if (!onDoublePressedBindings.ContainsKey(map)) {
			return false;
		}
		onDoublePressedBindings.Remove(map);
		return true;
	}

	public bool UnbindHoldEvent(InputMap map) {
		if (!onHeldBindings.ContainsKey(map)) {
			return false;
		}
		onHeldBindings.Remove(map);
		return true;
	}

	public bool UnbindReleaseEvent(InputMap map) {
		if (!onReleasedBindings.ContainsKey(map)) {
			return false;
		}
		onReleasedBindings.Remove(map);
		return true;
	}

	public bool UnbindChargeEvent(InputMap map) {
		if (!onChargedBindings.ContainsKey(map)) {
			return false;
		}
		onChargedBindings.Remove(map);
		return true;
	}

	public OnPressed GetPressEvent(InputMap map) {
		if (onPressedBindings.ContainsKey(map)) {
			return onPressedBindings[map];
		}
		return null;
	}

	public OnDoublePressed GetDoublePressEvent(InputMap map) {
		if (onDoublePressedBindings.ContainsKey(map)) {
			return onDoublePressedBindings[map];
		}
		return null;
	}

	public OnHeld GetHoldEvent(InputMap map) {
		if (onHeldBindings.ContainsKey(map)) {
			return onHeldBindings[map];
		}
		return null;
	}

	public OnReleased GetReleaseEvent(InputMap map) {
		if (onReleasedBindings.ContainsKey(map)) {
			return onReleasedBindings[map];
		}
		return null;
	}

	public OnCharged GetChargeEvent(InputMap map) {
		if (onChargedBindings.ContainsKey(map)) {
			return onChargedBindings[map];
		}
		return null;
	}
}