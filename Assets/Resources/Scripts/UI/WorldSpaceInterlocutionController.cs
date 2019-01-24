using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceInterlocutionController : MonoBehaviour {

	public float interval;
	public float releaseTime;
	public string releaseTip;
	public AudioAsset showSound;
	public AudioAsset hideSound;
	public AudioAsset switchSound;
	public AudioAsset correctSound;
	public AudioAsset incorrectSound;
	public AudioAsset releaseTipSound;
	public ParticleAsset correctEffect;
	public ParticleAsset incorrectEffect;
	public Action correctAction;
	public Action incorrectAction;
	public Action exitAction;
	public Color releaseTipColor;
	public Color normalColor;
	public Color highlightedColor;
	public Color correctColor;
	public Color incorrectColor;
	public GameObject basePanel;
	public HorizontalLayoutGroup releaseTipGroup;
	public VerticalLayoutGroup optionGroup;
	public Image optionABackgroud;
	public Image optionBBackgroud;
	public Image optionCBackgroud;
	public Image optionDBackgroud;
	public Image releaseTipBackground;
	public Text questionTextHolder;
	public Text optionATextHolder;
	public Text optionBTextHolder;
	public Text optionCTextHolder;
	public Text optionDTextHolder;
	public Text releaseTipTextHolder;

	public bool IsShowing => _hideCoroutine == null && gameObject.active;

	public Option SelectedOption {
		get { return _selectedOption; }
		set {
			_selectedOption = value;
			RefreshHighlight();
		}
	}

	public FaceDirection faceDirection {
		set {
			if (value == FaceDirection.Left) {
				releaseTipGroup.childAlignment = TextAnchor.UpperRight;
				optionGroup.childAlignment = TextAnchor.UpperRight;
				optionGroup.padding.left = 0;
				optionGroup.padding.right = 30;
			} else {
				releaseTipGroup.childAlignment = TextAnchor.UpperLeft;
				optionGroup.childAlignment = TextAnchor.UpperLeft;
				optionGroup.padding.left = 30;
				optionGroup.padding.right = 0;
			}
		}
	}

	private bool _releaseTipEmerged;
	[SerializeField]
	private InputMapper _inputMapper;
	private InterlocutionData _interlocutionData;
	private Text[] _textHolders = new Text[5];
	private StringBuilder[] _contents = {
		new StringBuilder(20),
		new StringBuilder(20),
		new StringBuilder(20),
		new StringBuilder(20),
		new StringBuilder(20)
	};
	private bool[] _finishSituations = { false, false, false, false, false };
	private Option _selectedOption;
	private AudioSource _audioSource;
	private Coroutine _showCoroutine;
	private Coroutine _hideCoroutine;

	private void Awake() {
		_audioSource = gameObject.AddComponent<AudioSource>();
		_audioSource.spatialize = false;

		_textHolders[0] = questionTextHolder;
		_textHolders[1] = optionATextHolder;
		_textHolders[2] = optionBTextHolder;
		_textHolders[3] = optionCTextHolder;
		_textHolders[4] = optionDTextHolder;
		
		_inputMapper = new InputMapper(InputMapper.defaultKeyboardMap);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_UP, SwitchUp);
		_inputMapper.BindPressEvent(InputMapper.SWITCH_DOWN, SwitchDown);
		_inputMapper.BindHoldEvent(InputMapper.INTERACT, CheckExit);
		_inputMapper.BindReleaseEvent(InputMapper.INTERACT, ConfirmOrExit);
	}
	
	private void Update() => _inputMapper.Refresh();
	
	private void RefreshHighlight() {
		optionABackgroud.color = normalColor;
		optionBBackgroud.color = normalColor;
		optionCBackgroud.color = normalColor;
		optionDBackgroud.color = normalColor;
		switch (SelectedOption) {
			case Option.A: optionABackgroud.color = highlightedColor;
				break;
			case Option.B: optionBBackgroud.color = highlightedColor;
				break;
			case Option.C: optionCBackgroud.color = highlightedColor;
				break;
			case Option.D: optionDBackgroud.color = highlightedColor;
				break;
		}
	}

	private void SwitchUp() {
		Debug.Log("Switch Up");
		if (SelectedOption == 0) SelectedOption = (Option) 3;
		else SelectedOption -= 1;
		if (switchSound) _audioSource.PlayOneShot(switchSound.Source);
	}

	private void SwitchDown() {
		Debug.Log("Switch Down");
		if ((int) SelectedOption == 3) SelectedOption = 0;
		else SelectedOption += 1;
		if (switchSound) _audioSource.PlayOneShot(switchSound.Source);
	}

	private void ConfirmOrExit() {
		if (_inputMapper[InputMapper.INTERACT].ChargeTime > releaseTime) exitAction?.Invoke();
		else Confirm();
	}

	private void Confirm() {
		if (SelectedOption == _interlocutionData.answer) OnCorrect();
		else OnIncorrect();
	}

	private void CheckExit() {
		if (!_releaseTipEmerged && _inputMapper[InputMapper.INTERACT].ChargeTime > releaseTime) ShowReleaseTip();
	}

	private void ShowReleaseTip() {
		_releaseTipEmerged = true;
		if (releaseTipSound) _audioSource.PlayOneShot(releaseTipSound.Source);
		releaseTipBackground.color = releaseTipColor;
		releaseTipTextHolder.text = releaseTip;
	}

	private void HideReleaseTip() {
		_releaseTipEmerged = false;
		Color color = releaseTipBackground.color;
		color.a = 0f;
		releaseTipBackground.color = color;
		releaseTipTextHolder.text = " ";
	}

	private void BindInput(InputMapper inputMapper) {
		_inputMapper[InputMapper.INTERACT] = inputMapper[InputMapper.INTERACT];
		_inputMapper[InputMapper.SWITCH_UP] = inputMapper[InputMapper.SWITCH_UP];
		_inputMapper[InputMapper.SWITCH_DOWN] = inputMapper[InputMapper.SWITCH_DOWN];
		_inputMapper.UDebug();
	}

	private void ActivateInput() => _inputMapper.isInControl = true;

	private void DeactivateInput() => _inputMapper.isInControl = false;

	private void OnCorrect() {
		if (correctSound) _audioSource.PlayOneShot(correctSound.Source);
		Vector3 position;
		switch (SelectedOption) {
			case Option.A:
				optionABackgroud.color = correctColor;
				position = optionABackgroud.transform.position;
				break;
			case Option.B:
				optionBBackgroud.color = correctColor;
				position = optionBBackgroud.transform.position;
				break;
			case Option.C:
				optionCBackgroud.color = correctColor;
				position = optionCBackgroud.transform.position;
				break;
			case Option.D:
				optionDBackgroud.color = correctColor;
				position = optionDBackgroud.transform.position;
				break;
			default:
				position = Vector3.zero;
				break;
		}
		
		if (correctEffect) {
			BurstParticleController effect = correctEffect.Get<BurstParticleController>();
			effect.transform.position = position;
			effect.Burst();
		}
		
		correctAction?.Invoke();
	}

	private void OnIncorrect() {
		if (incorrectSound) _audioSource.PlayOneShot(incorrectSound.Source);
		Vector3 position;
		switch (SelectedOption) {
			case Option.A:
				optionABackgroud.color = incorrectColor;
				position = optionABackgroud.transform.position;
				break;
			case Option.B:
				optionBBackgroud.color = incorrectColor;
				position = optionBBackgroud.transform.position;
				break;
			case Option.C:
				optionCBackgroud.color = incorrectColor;
				position = optionCBackgroud.transform.position;
				break;
			case Option.D:
				optionDBackgroud.color = incorrectColor;
				position = optionDBackgroud.transform.position;
				break;
			default:
				position = Vector3.zero;
				break;
		}
		
		if (incorrectEffect) {
			BurstParticleController effect = incorrectEffect.Get<BurstParticleController>();
			effect.transform.position = position;
			effect.Burst();
		}
		
		incorrectAction?.Invoke();
	}
	
	public void Show(InputMapper inputMapper, InterlocutionData data) {
		_interlocutionData = data;
		SelectedOption = 0;
		gameObject.SetActive(true);
		BindInput(inputMapper);
		ActivateInput();
		if (_showCoroutine != null) {
			StopCoroutine(_hideCoroutine);
			_showCoroutine = null;
		}
		
		if (_hideCoroutine != null) {
			StopCoroutine(_hideCoroutine);
			_hideCoroutine = null;
		}
		
		if (showSound) _audioSource.PlayOneShot(showSound.Source);
		_showCoroutine = StartCoroutine(ExeShowCoroutine());
	}

	public void Hide() {
		if (_showCoroutine != null) {
			StopCoroutine(_showCoroutine);
			_showCoroutine = null;
		}
		
		if (_hideCoroutine != null) {
			StopCoroutine(_hideCoroutine);
			_hideCoroutine = null;
		}

		if (hideSound) _audioSource.PlayOneShot(hideSound.Source);
		if (_hideCoroutine == null) _hideCoroutine = StartCoroutine(ExeHideCoroutine());
	}
	
	public void Exit() {
		DeactivateInput();
		HideReleaseTip();
		Hide();
	}

	private IEnumerator ExeShowCoroutine() {
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		for (int i = 0, l = _finishSituations.Length; i < l; i++) {
			_textHolders[i].text = string.Empty;
			_contents[i].Clear();
			_finishSituations[i] = false;
		}

		bool finished = false;
		int index = -1;
		while (!finished) {
			index++;
			for (int i = 0, l = _textHolders.Length; i < l; i++) {
				if (_finishSituations[i]) continue;
				if (_textHolders[i].text.Length == _interlocutionData[i].Length) {
					_finishSituations[i] = true;
					continue;
				}

				_contents[i].Append(_interlocutionData[i][index]);
				_textHolders[i].text = _contents[i].ToString();
			}

			for (int i = 0, l = _finishSituations.Length; i < l; i++) {
				if (!_finishSituations[i]) break;
				finished = true;
			}

			yield return waitForSeconds;
		}

		_showCoroutine = null;
	}

	private IEnumerator ExeHideCoroutine() {
		WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
		for (int i = 0, l = _finishSituations.Length; i < l; i++) _finishSituations[i] = false;

		bool finished = false;
		int index = -1;
		while (!finished) {
			index++;
			for (int i = 0, l = _textHolders.Length; i < l; i++) {
				if (_finishSituations[i]) continue;
				if (_textHolders[i].text.Length == 0) {
					_finishSituations[i] = true;
					continue;
				}
				
				_textHolders[i].text = _contents[i].ToString(0, _contents[i].Length - index - 1);
			}

			for (int i = 0, l = _finishSituations.Length; i < l; i++) {
				if (!_finishSituations[i]) break;
				finished = true;
			}

			yield return waitForSeconds;
		}

		gameObject.SetActive(false);
		_hideCoroutine = null;
	}
}