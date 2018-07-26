using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {

	private InputOperator _inputOperator;
	private InteractionController _interactionController;
	private Collider _trigger;
	private List<InteractionController> _potentialInteractionControllers = new List<InteractionController>(2);

	private void Init(InputOperator inputOperator) {
		_inputOperator = inputOperator;
		
		_trigger = gameObject.AddComponent<Collider>();
		_trigger.isTrigger = true;
	}

	private void Update() {
		transform.position = _inputOperator.transform.position;
	}
	
	private void Interact() {
		if (_interactionController) _interactionController.Interact();
	}

	private void CalculateInteractionController() {
		if (_potentialInteractionControllers.Count == 0) {
			_interactionController = null;
			return;
		}

		float minSqrDistance = 999f;
		foreach (var interactionController in _potentialInteractionControllers) {
			float sqrDistance = (interactionController.transform.position - transform.position).sqrMagnitude;
			if (sqrDistance < minSqrDistance) {
				minSqrDistance = sqrDistance;
				_interactionController = interactionController;
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(TagManager.INTERACTIVE_TAG)) {
			InteractionController controller = other.GetComponent<InteractionController>();
			if (controller.interactive) {
				_potentialInteractionControllers.Add(controller);
				CalculateInteractionController();
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag(TagManager.INTERACTIVE_TAG)) {
			InteractionController controller = other.GetComponent<InteractionController>();
			if (controller.interactive) {
				_potentialInteractionControllers.Remove(controller);
				CalculateInteractionController();
			}
		}
	}
}