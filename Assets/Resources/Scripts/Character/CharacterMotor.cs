using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {

	public float jumpPower;
	public float speed;
	public float acceleration;
	public float dodgeTime;
	public float dodgeDistance;
	public int maxJumpTimes;
	public bool moveInAir;

	int jumpTimes;
	bool inAir;
	bool stunned;
	bool accelerating;
	bool attacking;
	bool dodging;
	bool parrying;
	FaceDirection faceDirection;

	CharacterController characterController;
	WeaponMotor weaponMotor;

	Coroutine jumpTask;
	Coroutine attackTask;
	Coroutine dodgeTask;

	float temperFloat;
	Vector3 temperVector3 = Vector3.zero;

	void Start() {
		characterController = GetComponent<CharacterController>();
		weaponMotor = GetComponent<WeaponMotor>();
		faceDirection = FaceDirection.Left;
	}

	public virtual void OnReceiveMoveLeftCommand() {
		if (inAir) {
			if (!moveInAir) {
				return;
			}
		}
		Move(FaceDirection.Left);
	}

	public virtual void OnReceiveMoveRightCommand() {
		if (inAir) {
			if (!moveInAir) {
				return;
			}
		}
		Move(FaceDirection.Right);

	}

	public virtual void OnReceiveJumpCommand() {
		if (jumpTimes < maxJumpTimes) {
			Jump();
		}
	}

	public virtual void OnReceiveAccelerateCommand() {
		Accelerate();
	}

	public virtual void OnReceiveCancelAccelerateCommand() {
		CancelAccelerate();
	}

	public virtual void OnReceiveAttackCommand() {
		if (!dodging && !parrying) {
			Attack();
		}
	}

	public virtual void OnReceiveDodgeCommand() {
		if (!parrying) {
			Dodge();
		}
	}

	public virtual void OnReceiveParryCommand() {
		if (!dodging) {
			Parry();
		}
	}

	public virtual void OnReceiveCancelParryCommand() {

	}

	protected virtual void Turn() {
		faceDirection = (FaceDirection)(-(int)faceDirection);
		transform.Rotate(0, 180, 0);
	}

	protected virtual void Move(FaceDirection faceDirection) {
		if (this.faceDirection != faceDirection) {
			Turn();
		}
		temperFloat = speed * (accelerating ? acceleration : 1);
		Debug.Log(temperFloat);
		temperVector3.x = speed * (int)faceDirection;
		characterController.Move(temperVector3);
	}

	protected virtual void Jump() {
		jumpTimes++;
	}

	protected virtual void Accelerate() {
		accelerating = true;
	}

	protected virtual void CancelAccelerate() {
		accelerating = false;
	}

	protected virtual void Attack() {
		attacking = true;

	}

	protected virtual void CancelAttack() {
		attacking = false;

	}

	protected virtual void Dodge() {
		dodging = true;

	}

	protected virtual void Parry() {
		parrying = true;
	}

	protected virtual void CancelParry() {
		parrying = false;
	}

	protected virtual IEnumerator ExeJumpTask() {

		yield return null;
	}

	protected virtual IEnumerator ExeAttackTask() {
		yield return weaponMotor.Attack();
	}

	protected virtual IEnumerator ExeDodgeTask() {
		yield return null;
	}
}

public enum FaceDirection {
	Left = -1,
	Right = 1
}