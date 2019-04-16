using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

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
	public UnityEventWithCharacterController interactionEvent;
	public UnityEventWithCharacterController counterInteractionEvent;
	public UnityEvent loseInteractionEvent;

	public delegate void OnTextChangeHandler();
	public delegate void OnDisableHandler();
	
	public event OnTextChangeHandler onTextChange;
	public event OnDisableHandler onDisable;

	public bool Interactive => _interactive;
	public string Text => hasCounterInteraction ? (_interacted ? counterInteractionText : interactionText) : interactionText;

	private bool _interacted;
	private bool _interactive = true;
	private Collider _trigger;
	private InputOperator _characterOperator;

	public void Interact(CharacterController characterController) {
		_characterOperator = characterController.GetComponent<InputOperator>();
		if (hasCounterInteraction) {
			if (_interacted) counterInteractionEvent?.Invoke(characterController);
			else interactionEvent?.Invoke(characterController);
			_interacted = !_interacted;
			onTextChange?.Invoke();
		} else {
			interactionEvent?.Invoke(characterController);
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
		onDisable?.Invoke();
	}

	public void LockOperator() {
		if (_characterOperator) _characterOperator.InputMapper.isInControl = false;
	}

	public void UnlockOperator() {
		if (_characterOperator) _characterOperator.InputMapper.isInControl = true;
	}

	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmos() {
		var pos = transform.position;
		Gizmos.color = Color.green;
		float size = .2f;
		Gizmos.DrawWireCube(pos, new Vector3(size, size, size));
		string text = interactionText;
		if (hasCounterInteraction) {
			int length = text.Length - text.LastIndexOf('\n') - 3;
			text += "\n";
			for (int i = 0; i < length; i++) text += "-";
			text += "\n";
			text += counterInteractionText;
		}

		Handles.Label(pos, text);

	}
	
	public enum PanelShowDirection {
		BottomToTop = 0,
		TopToBottom = 1
	}
}