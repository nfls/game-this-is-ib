using UnityEngine;

public abstract class InputDetector : IInputDetectable {
	
	protected string _name;
	protected bool _isPressed;
	protected bool _isHeld;
	protected bool _isReleased;
	protected float _lastPressTime = -1f;

	public string Name => _name;

	public bool IsPressed => _isPressed;

	public bool IsHeld => _isHeld;

	public bool IsReleased => _isReleased;

	public float ChargeTime => Time.time - _lastPressTime;

	public abstract void Refresh();

	public void Reset() {
		_isPressed = false;
		_isHeld = false;
		_isReleased = false;
	}
}