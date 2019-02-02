public abstract class JoystickInputDetector : InputDetector {

	protected int _joystickIndex;
	protected int _inputIndex;
	protected bool _isValid;

	public int JoystickIndex => _joystickIndex;

	public int InputIndex => _inputIndex;

	public bool IsValid => _isValid;

	protected JoystickInputDetector(int joystickIndex, int inputIndex) {
		_joystickIndex = joystickIndex;
		_inputIndex = inputIndex;
	}
}