using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {

	public Collider Trigger => _trigger;

	private InteractionController _interactionController;
	private Collider _trigger;
	private Rigidbody _rigidbody;
	private List<InteractionController> _potentialInteractionControllers = new List<InteractionController>(2);

	private void Awake() {
		_trigger = gameObject.AddComponent<BoxCollider>();
		_trigger.isTrigger = true;

		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
	}

	public void Interact() {
		if (_interactionController) {
			_interactionController.Interact();
			if (!_interactionController.Interactive) {
				_potentialInteractionControllers.Remove(_interactionController);
				Refresh();
			}
		}
	}

	private void Refresh() {
		if (CalculateInteractionController()) {
			if (_interactionController) {
				float direction = transform.position.x - _interactionController.transform.position.x > 0 ? 1 : -1;
				Vector3 originalPos = _interactionController.transform.position;
				Vector3 position = new Vector3 {
					x = direction * _interactionController.horizontalOffset + originalPos.x,
					y = _interactionController.verticalOffset + originalPos.y,
					z = -2.1f
				};

				Vector2 pivot = new Vector2 {
					x = direction == 1 ? 0 : 1,
					y = (float) _interactionController.showDirection
				};

				UIManager.ShowInteractionTip(_interactionController.text, position, pivot);
			} else {
				UIManager.HideInteractionTip();
			}
		}
	}

	private bool CalculateInteractionController() {
		if (_potentialInteractionControllers.Count == 0) {
			_interactionController = null;
			return true;
		}

		InteractionController oldInteractionController = _interactionController;
		float minSqrDistance = 9999f;
		foreach (var interactionController in _potentialInteractionControllers) {
			float sqrDistance = (interactionController.transform.position - transform.position).sqrMagnitude;
			if (sqrDistance < minSqrDistance) {
				minSqrDistance = sqrDistance;
				_interactionController = interactionController;
			}
		}

		if (_interactionController != oldInteractionController) return true;
		return false;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(TagManager.INTERACTIVE_TAG)) {
			InteractionController controller = other.GetComponentInParent<InteractionController>();
			if (controller.Interactive) {
				_potentialInteractionControllers.Add(controller);
				Refresh();
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag(TagManager.INTERACTIVE_TAG)) {
			InteractionController controller = other.GetComponentInParent<InteractionController>();
			if (controller.Interactive) {
				_potentialInteractionControllers.Remove(controller);
				Refresh();
			}
		}
	}
}