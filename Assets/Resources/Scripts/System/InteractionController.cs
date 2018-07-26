using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractionController : MonoBehaviour {

	public bool oneTime;
	public bool remoteInteractive;
	public float horizontalOffset;
	public float verticalOffset;
	public Sprite icon;
	public string text;
	public UnityAction interactionAction;
	
	public bool interactive {
		get;
		private set;
	}

	private Collider _trigger;

	private void Start() {
		_trigger = GetComponent<Collider>();
		if (!_trigger.isTrigger) _trigger.isTrigger = true;
	}

	public void Interact() {
		interactionAction?.Invoke();
		if (oneTime) interactive = false;
	}

	public void Enable() {
		interactive = true;
	}

	public void Disable() {
		interactive = false;
	}
}