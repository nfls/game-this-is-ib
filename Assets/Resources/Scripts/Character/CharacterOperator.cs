using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterOperator : MonoBehaviour {
	
	protected CharacterController _characterController;

	public virtual void Start() {
		_characterController = GetComponent<CharacterController>();
	}
}