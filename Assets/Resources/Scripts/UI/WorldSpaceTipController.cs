using System.Collections;
using System.Text;
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
	private readonly StringBuilder _builder = new StringBuilder(12);
	private Color _originalColor;
	private Image _background;
	private Coroutine _showCoroutine;
	private Coroutine _hideCoroutine;

	private void Awake() {
		gameObject.SetActive(false);
		_background = _textHolder.GetComponent<Image>();
		_originalColor = _background.color;
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

		if (_hideCoroutine == null) _hideCoroutine = StartCoroutine(ExeHideCoroutine());
	}

	public void Highlight(Color highlightColor) {
		_background.color = highlightColor;
	}

	public void Normal() {
		_background.color = _originalColor;
	}

	private IEnumerator ExeShowCoroutine() {
		_builder.Clear();
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		int index = -1;
		while (++index < _text.Length) {
			_builder.Append(_text[index]);
			_textController.text = _builder.ToString();
			yield return waitForSeconds;
		}
		
		_showCoroutine = null;
	}

	private IEnumerator ExeHideCoroutine() {
		Normal();
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		int index = _builder.Length;
		while (_textController.text.Length > 0) {
			_textController.text = _builder.ToString(0, --index);
			yield return waitForSeconds;
		}
		
		gameObject.SetActive(false);
		_hideCoroutine = null;
	}
}