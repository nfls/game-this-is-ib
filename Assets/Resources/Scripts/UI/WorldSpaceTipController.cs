using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceTipController : MonoBehaviour {

	public Sprite icon;
	public string text;

	public bool IsShowing => gameObject.active;

	[SerializeField]
	protected Image _iconHolder;
	[SerializeField]
	protected Text _textHolder;

	public void Show() {
		if (icon != null) {
			_iconHolder.sprite = icon;
			_iconHolder.gameObject.SetActive(true);
		} else {
			_iconHolder.gameObject.SetActive(false);
		}

		if (!string.IsNullOrEmpty(text)) {
			_textHolder.text = text;
			_textHolder.gameObject.SetActive(true);
		} else {
			_textHolder.gameObject.SetActive(false);
		}
		
		gameObject.SetActive(true);
	}

	public void Hide() {
		icon = null;
		text = string.Empty;
		gameObject.SetActive(false);
	}
}