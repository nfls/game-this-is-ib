using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {

	public bool isLocalPlayer;

	public Collider Trigger => _trigger;

	private InteractionController _interactionController;
	private Collider _trigger;
	private Rigidbody _rigidbody;
	private List<InteractionController> _potentialInteractionControllers = new List<InteractionController>(2);

	private InteractionController interactionController {
		get { return _interactionController; }
		set {
			if (_interactionController) _interactionController.onTextChange -= RefreshInteractionTip;
			_interactionController = value;
			if (_interactionController) _interactionController.onTextChange += RefreshInteractionTip;
		}
	}

	private void Awake() {
		_trigger = gameObject.AddComponent<BoxCollider>();
		_trigger.isTrigger = true;

		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
	}

	public void Preinteract() {
		if (interactionController) UIManager.HighlightInteractionTip(interactionController.highlightColor, isLocalPlayer);
	}

	public void Interact() {
		if (interactionController) {
			UIManager.NormalInteractionTip(isLocalPlayer);
			interactionController.Interact();
			if (!interactionController.Interactive) {
				_potentialInteractionControllers.Remove(interactionController);
				Refresh();
			}
		}
	}

	private void Refresh() {
		if (CalculateInteractionController()) RefreshInteractionTip();
	}

	private void RefreshInteractionTip() {
		if (interactionController) {
			Vector3 originalPos = interactionController.transform.position;
			Vector3 position = new Vector3 {
				x = interactionController.horizontalOffset + originalPos.x,
				y = interactionController.verticalOffset + originalPos.y,
				z = -1.1f
			};

			Vector2 pivot = new Vector2 {
				x = interactionController.horizontalOffset > 0 ? 0 : 1,
				y = (float) interactionController.showDirection
			};

			UIManager.ShowInteractionTip(interactionController.Text, position, pivot, isLocalPlayer);
		} else UIManager.HideInteractionTip(isLocalPlayer);
	}

	private bool CalculateInteractionController() {
		if (_potentialInteractionControllers.Count == 0) {
			interactionController = null;
			return true;
		}

		InteractionController oldInteractionController = interactionController;
		float minSqrDistance = 9999f;
		foreach (var controller in _potentialInteractionControllers) {
			float sqrDistance = (controller.transform.position - transform.position).sqrMagnitude;
			if (sqrDistance < minSqrDistance) {
				minSqrDistance = sqrDistance;
				interactionController = controller;
			}
		}

		if (interactionController != oldInteractionController) return true;
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