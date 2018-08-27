using UnityEngine;

public sealed class InputOperator : CharacterOperator {
	
	public InputMapper InputMapper => _inputMapper;

	public InteractionSystem InteractionSystem {
		get { return _interactionSystem; }
		set {
			_interactionSystem = value;
			if (_interactionSystem) {
				_inputMapper.BindPressEvent(InputMapper.INTERACT, _interactionSystem.Interact);
				Physics.IgnoreCollision(GetComponent<Collider>(), _interactionSystem.Trigger, true);
			} else {
				_inputMapper.UnbindPressEvent(InputMapper.INTERACT);
				Physics.IgnoreCollision(GetComponent<Collider>(), _interactionSystem.Trigger, false);
			}
		}
	}

	private bool _hasStarted;
	private InputMapper _inputMapper;
	private InputMapper _temperInputMapper;
	private InteractionSystem _interactionSystem;
		
	protected override void Awake() {
		base.Awake();
		
		_inputMapper = new InputMapper(InputMapper.defaultKeyboardMap);
		_inputMapper.BindHoldEvent(InputMapper.MOVE_LEFT, _characterController.MoveLeft);
		_inputMapper.BindHoldEvent(InputMapper.MOVE_RIGHT, _characterController.MoveRight);
		_inputMapper.BindPressEvent(InputMapper.ACCELERATE, _characterController.EnterAcceleratingState);
		_inputMapper.BindReleaseEvent(InputMapper.ACCELERATE, _characterController.ExitAcceleratingState);
		_inputMapper.BindPressEvent(InputMapper.JUMP, _characterController.Jump);
		_inputMapper.BindPressEvent(InputMapper.DODGE, _characterController.Dodge);
		_inputMapper.BindPressEvent(InputMapper.ATTACK, _characterController.OnReceiveAttackCommand);
		_inputMapper.BindReleaseEvent(InputMapper.ATTACK, _characterController.OnFinishAttackCommand);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_PREV, _characterController.SwitchPreviousIBSprite);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_NEXT, _characterController.SwitchNextIBSprite);

		_inputMapper.isInControl = true;
		
		_temperInputMapper = new InputMapper(InputMapper.defaultMacXboxOneMap);
		_temperInputMapper.BindHoldEvent(InputMapper.MOVE_LEFT, _characterController.MoveLeft);
		_temperInputMapper.BindHoldEvent(InputMapper.MOVE_RIGHT, _characterController.MoveRight);
		_temperInputMapper.BindPressEvent(InputMapper.ACCELERATE, _characterController.EnterAcceleratingState);
		_temperInputMapper.BindReleaseEvent(InputMapper.ACCELERATE, _characterController.ExitAcceleratingState);
		_temperInputMapper.BindPressEvent(InputMapper.JUMP, _characterController.Jump);
		_temperInputMapper.BindPressEvent(InputMapper.DODGE, _characterController.Dodge);
		_temperInputMapper.BindPressEvent(InputMapper.ATTACK, _characterController.OnReceiveAttackCommand);
		_temperInputMapper.BindReleaseEvent(InputMapper.ATTACK, _characterController.OnFinishAttackCommand);
		_temperInputMapper.BindPressEvent(InputMapper.SWITCH_PREV, _characterController.SwitchPreviousIBSprite);
		_temperInputMapper.BindPressEvent(InputMapper.SWITCH_NEXT, _characterController.SwitchNextIBSprite);

		_temperInputMapper.isInControl = true;
	}

	private void Update() {
		_interactionSystem.transform.position = transform.position;
		_inputMapper.Refresh();
		_temperInputMapper.Refresh();
	}
}