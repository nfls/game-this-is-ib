using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceTipController : MonoBehaviour {

	public float interval;

	public bool IsShowing => _hideCoroutine == null && gameObject.active;
	
	[SerializeField]
	private GameObject _textHolder;
	[SerializeField]
	private Text _textController;

	private string _text;
	private Coroutine _showCoroutine;
	private Coroutine _hideCoroutine;

	private void Start() {
		gameObject.SetActive(false);
	}

	public void Show(string text, Vector3 position, Vector3 pivot) {
		if (_hideCoroutine != null) {
			StopCoroutine(_hideCoroutine);
			_hideCoroutine = null;
			_textController.text = string.Empty;
			gameObject.SetActive(false);
		}
		
		(_textHolder.transform as RectTransform).pivot = pivot;
		_textHolder.transform.position = position;
		_text = text;

		gameObject.SetActive(true);
		_showCoroutine = StartCoroutine(ExeShowCoroutine());
	}

	public void Hide() {
		if (_showCoroutine != null) {
			StopCoroutine(_showCoroutine);
			_showCoroutine = null;
		}

		if (_hideCoroutine == null) {
			_hideCoroutine = StartCoroutine(ExeHideCoroutine());
		}
	}

	private IEnumerator ExeShowCoroutine() {
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		while (_textController.text.Length < _text.Length) {
			_textController.text = _text.Substring(0, _textController.text.Length + 1);
			yield return waitForSeconds;
		}

		_showCoroutine = null;
	}

	private IEnumerator ExeHideCoroutine() {
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		while (_textController.text.Length > 0) {
			_textController.text = _textController.text.Substring(0, _textController.text.Length - 1);
			yield return waitForSeconds;
		}
		
		gameObject.SetActive(false);
		_hideCoroutine = null;
	}
}