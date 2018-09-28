using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour {

	public bool oneTime;
	public bool disappearWhenDisabled;
	public PanelShowDirection showDirection;
	public float horizontalOffset;
	public float verticalOffset;
	public Color highlightColor = Color.red;
	[Multiline]
	public string text;
	public UnityEvent interactionEvent;

	public bool Interactive => interactive;

	private bool interactive = true;
	private Collider _trigger;

	public void Interact() {
		interactionEvent?.Invoke();
		if (oneTime) Disable();
	}

	public void Enable() {
		interactive = true;
		gameObject.SetActive(disappearWhenDisabled);
	}

	public void Disable() {
		interactive = false;
		gameObject.SetActive(!disappearWhenDisabled);
	}
	
	public enum PanelShowDirection {
		BottomToTop = 0,
		TopToBottom = 1
	}
}