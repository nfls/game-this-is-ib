public sealed class InputOperator : CharacterOperator {
	
	public InputMapper InputMapper => _inputMapper;

	private InputMapper _inputMapper;
		
	public override void Start() {
		base.Start();
		
		_inputMapper = new InputMapper();
		_inputMapper.BindHoldEvent(InputMapper.MOVE_LEFT, _characterController.MoveLeft);
		_inputMapper.BindHoldEvent(InputMapper.MOVE_RIGHT, _characterController.MoveRight);
		_inputMapper.BindPressEvent(InputMapper.ACCELERATE, _characterController.EnterAcceleratingState);
		_inputMapper.BindReleaseEvent(InputMapper.ACCELERATE, _characterController.ExitAcceleratingState);
		_inputMapper.BindPressEvent(InputMapper.JUMP, _characterController.Jump);
		_inputMapper.BindPressEvent(InputMapper.DODGE, _characterController.Dodge);
		_inputMapper.BindPressEvent(InputMapper.ATTACK, _characterController.Attack);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_PREV, _characterController.SwitchPreviousIBSprite);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_NEXT, _characterController.SwitchNextIBSprite);
		// _inputMapper.BindPressEvent(InputMapper.INTERACT, UIManager.Interact);

		_inputMapper.isInControl = true;
	}

	private void Update() {
		_inputMapper.Refresh();
	}
}