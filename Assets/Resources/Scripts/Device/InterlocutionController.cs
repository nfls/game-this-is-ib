using System;
using UnityEngine;

public class InterlocutionController : SignalSender {

	public Interlocution interlocution;

	private InteractionController _interactionController;

	private void Awake() {
		_interactionController = GetComponent<InteractionController>();
		_interactionController.interactionEvent.AddListener(StartInterlocution);
	}

	public void StartInterlocution() {
		
	}

	private void OnDestroy() {
		if (_interactionController) _interactionController.interactionEvent.RemoveListener(StartInterlocution);
	}
}

[Serializable]
public class Interlocution {

	[Subject]
	public string subject;
	public int id;
}