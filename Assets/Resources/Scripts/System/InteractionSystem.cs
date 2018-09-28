using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {

	public bool isLocalPlayer;

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

	public void Preinteract() {
		if (_interactionController) UIManager.HighlightInteractionTip(_interactionController.highlightColor, isLocalPlayer);
	}

	public void Interact() {
		if (_interactionController) {
			UIManager.NormalInteractionTip(isLocalPlayer);
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
				Vector3 originalPos = _interactionController.transform.position;
				Vector3 position = new Vector3 {
					x = _interactionController.horizontalOffset + originalPos.x,
					y = _interactionController.verticalOffset + originalPos.y,
					z = -1.1f
				};

				Vector2 pivot = new Vector2 {
					x = _interactionController.horizontalOffset > 0 ? 0 : 1,
					y = (float) _interactionController.showDirection
				};

				UIManager.ShowInteractionTip(_interactionController.text, position, pivot, isLocalPlayer);
			} else UIManager.HideInteractionTip(isLocalPlayer);
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