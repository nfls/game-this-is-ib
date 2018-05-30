using UnityEngine;

public class TestBehaviour : MonoBehaviour {

	public CharacterMotor motor;
	public InputController inputController;

	void Start() {
		inputController.Init(1, new KeyboardMapper());

		inputController.BindPressEvent(InputMap.Accelerate, motor.OnReceiveAccelerateCommand);
		inputController.BindHoldEvent(InputMap.MoveLeft, motor.OnReceiveMoveLeftCommand);
		inputController.BindHoldEvent(InputMap.MoveRight, motor.OnReceiveMoveRightCommand);
		inputController.BindReleaseEvent(InputMap.Accelerate, motor.OnReceiveCancelAccelerateCommand);
	}

	void NoParamFunc() {

	}

	void FloatParamFunc(float f) {

	}
}
