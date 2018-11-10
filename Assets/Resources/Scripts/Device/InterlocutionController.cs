using System;
using UnityEngine;
using UnityEngine.Events;

public class InterlocutionController : DeviceController {

	public Interlocution interlocution;
	public InteractionController.PanelShowDirection showDirection;
	public float rotationAngle;
	public float horizontalOffset;
	public float verticalOffset;
	public UnityEvent correctionEvent;
	public UnityEvent incorrectionEvent;

	private WorldSpaceInterlocutionController _uiController;

	public void StartInterlocution(CharacterController characterController) {
		if (_uiController) return;
		_uiController = UIManager.GetInterlocutionUI(characterController.CompareTag(TagManager.LOCAL_PLAYER_TAG));
		float direction = (transform.position - characterController.transform.position).x > 0 ? 1f : -1f;
		Vector3 originalPos = transform.position;
		Vector3 position = new Vector3 {
			x = direction * horizontalOffset + originalPos.x,
			y = verticalOffset + originalPos.y,
			z = -2f
		};

		Vector2 pivot = new Vector2 {
			x = direction > 0 ? 0 : 1,
			y = 1f
		};

		_uiController.faceDirection = direction > 0 ? FaceDirection.Right : FaceDirection.Left;
		(_uiController.basePanel.transform as RectTransform).pivot = pivot;
		_uiController.basePanel.transform.position = position;
		_uiController.basePanel.transform.eulerAngles = new Vector3(0f, rotationAngle * direction);
		_uiController.correctAction = OnCorrection;
		_uiController.incorrectAction = OnIncorrection;
		_uiController.exitAction = EndInterlocution;
		_uiController.Show(characterController.GetComponent<InputOperator>().InputMapper, ResourcesManager.InterlocutionData[interlocution.subject][interlocution.id]);
	}

	public void EndInterlocution() {
		if (!_uiController) return;
		_uiController.Exit();
		_uiController = null;
	}

	private void OnCorrection() {
		correctionEvent?.Invoke();
	}

	private void OnIncorrection() {
		incorrectionEvent?.Invoke();
	}
}

[Serializable]
public class Interlocution {

	[Subject]
	public string subject;
	public int id;
}