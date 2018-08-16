using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterOperator : MonoBehaviour {
	
	protected CharacterController _characterController;

	protected virtual void Awake() {
		_characterController = GetComponent<CharacterController>();
	}
}