using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour {

	public bool oneTime;
	public PanelShowDirection showDirection;
	public float horizontalOffset;
	public float verticalOffset;
	[Multiline]
	public string text;
	public UnityEvent interactionEvent;

	public bool Interactive => interactive;

	private bool interactive = true;
	private Collider _trigger;

	public void Interact() {
		interactionEvent?.Invoke();
		if (oneTime) interactive = false;
	}

	public void Enable() {
		interactive = true;
	}

	public void Disable() {
		interactive = false;
	}
	
	public enum PanelShowDirection {
		BottomToTop = 0,
		TopToBottom = 1
	}
}