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
	private InteractionSystem _interactionSystem;
		
	protected override void Awake() {
		base.Awake();
		
		_inputMapper = new InputMapper();
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
	}

	private void Update() {
		_interactionSystem.transform.position = transform.position;
		_inputMapper.Refresh();
	}
}