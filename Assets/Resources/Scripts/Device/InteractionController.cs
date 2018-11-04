using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour {

	public bool oneTime;
	public bool hasCounterInteraction;
	public bool disappearWhenDisabled;
	public PanelShowDirection showDirection;
	public float horizontalOffset;
	public float verticalOffset;
	public Color highlightColor = Color.red;
	[Multiline]
	public string interactionText;
	[Multiline]
	public string counterInteractionText;
	public UnityEvent interactionEvent;
	public UnityEvent counterInteractionEvent;

	public delegate void OnTextChangeHandler();
	
	public event OnTextChangeHandler onTextChange;

	public bool Interactive => _interactive;
	public string Text => hasCounterInteraction ? (_interacted ? counterInteractionText : interactionText) : interactionText;

	private bool _interacted;
	private bool _interactive = true;
	private Collider _trigger;

	public void Interact() {
		if (hasCounterInteraction) {
			if (_interacted) counterInteractionEvent?.Invoke();
			else interactionEvent?.Invoke();
			_interacted = !_interacted;
			onTextChange?.Invoke();
		} else {
			interactionEvent?.Invoke();
			_interacted = true;
		}
		
		if (oneTime) Disable();
	}

	public void Enable() {
		_interactive = true;
		gameObject.SetActive(disappearWhenDisabled);
	}

	public void Disable() {
		_interactive = false;
		gameObject.SetActive(!disappearWhenDisabled);
	}
	
	public enum PanelShowDirection {
		BottomToTop = 0,
		TopToBottom = 1
	}
}