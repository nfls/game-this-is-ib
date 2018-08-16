using System;

public class SillyAIOperator : CharacterOperator {

	public CharacterController playerController;
	public float detectionDistance = 5f;
	public float stopDistance = 1.2f;

	protected bool _isAlerted;
	protected Action _action;

	protected void Update() {
		Think();
		_action?.Invoke();
	}

	protected void Think() {
		if (!playerController) return;
		float distanceSqr = (playerController.transform.position - transform.position).sqrMagnitude;
		_isAlerted = distanceSqr <= detectionDistance * detectionDistance;
		if (_isAlerted) {
			if (distanceSqr > stopDistance * stopDistance) {
				if (playerController.transform.position.x - transform.position.x < 0) {
					_action = _characterController.MoveLeft;
				} else {
					_action = _characterController.MoveRight;
				}
			}
		}
	}
}