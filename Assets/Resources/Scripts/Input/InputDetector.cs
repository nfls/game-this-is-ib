using UnityEngine;

public abstract class InputDetector : IInputDetectable {
	
	protected string _name;
	protected bool _isPressed;
	protected bool _isHeld;
	protected bool _isReleased;
	protected float _lastPressTime = -1f;

	public string Name {
		get {
			return _name;
		}
	}

	public bool IsPressed {
		get {
			return _isPressed;
		}
	}

	public bool IsHeld {
		get {
			return _isHeld;
		}
	}

	public bool IsReleased {
		get {
			return _isReleased;
		}
	}

	public float ChargeTime {
		get {
			return Time.time - _lastPressTime;
		}
	}
	
	public abstract void Refresh();

	public void Reset() {
		_isPressed = false;
		_isHeld = false;
		_isReleased = false;
	}
}