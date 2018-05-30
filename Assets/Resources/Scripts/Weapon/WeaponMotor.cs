using UnityEngine;
using System.Collections;

public class WeaponMotor : MonoBehaviour {

	Coroutine attackTask;

	void Start() {

	}

	public virtual Coroutine Attack() {
		attackTask = StartCoroutine(ExeAttackTask());
		return attackTask;
	}

	public virtual void CancelAttack() {
		StopCoroutine(attackTask);
	}

	protected virtual IEnumerator ExeAttackTask() {
		yield return null;
	}
}
