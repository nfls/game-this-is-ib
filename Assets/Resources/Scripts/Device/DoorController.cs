using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorController : DeviceController {

	public float openTime;
	public float frameDoorRatio;
	public float openLag;
	public float closeLag;
	public DoorOpenDirection openDirection;
	public AudioAsset operationSound = "door_operate";

	protected Vector3 _originalPanelScale;
	protected Vector3 _originalTopPanelPosition;
	protected Vector3 _originalBottomPanelPosition;
	protected GameObject _topPanel;
	protected GameObject _bottomPanel;
	protected AudioSource _audioSource;

	protected Coroutine _openCoroutine;
	protected Coroutine _closeCoroutine;

	protected void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_audioSource.playOnAwake = false;
		_audioSource.loop = false;
		_audioSource.spatialize = true;
		_audioSource.spatialBlend = 1;
		_audioSource.rolloffMode = AudioRolloffMode.Linear;
		
		_topPanel = transform.Find("Top Panel").gameObject;
		_bottomPanel = transform.Find("Bottom Panel").gameObject;

		_originalPanelScale = _topPanel.transform.localScale;
		_originalTopPanelPosition = _topPanel.transform.localPosition;
		_originalBottomPanelPosition = _bottomPanel.transform.localPosition;
	}

	public void Open() {
		if (_closeCoroutine != null) {
			StopCoroutine(_closeCoroutine);
			_closeCoroutine = null;
		}

		if (_openCoroutine == null) _openCoroutine = StartCoroutine(ExeOpenTask());
	}

	public void Close() {
		if (_openCoroutine != null) {
			StopCoroutine(_openCoroutine);
			_openCoroutine = null;
		}

		if (_closeCoroutine == null) _closeCoroutine = StartCoroutine(ExeCloseTask());
	}

	protected IEnumerator ExeOpenTask() {
		_topPanel.GetComponent<Collider>().isTrigger = true;
		_bottomPanel.GetComponent<Collider>().isTrigger = true;
		
		Vector3 targetScale;
		Vector3 targetTopPos;
		Vector3 targetBottomPos;
		
		targetScale = _originalPanelScale;
		targetScale.y *= frameDoorRatio;
		
		switch (openDirection) {
			case DoorOpenDirection.Top:
				targetTopPos = _originalTopPanelPosition;
				targetTopPos.y += _originalPanelScale.y / 2 - targetScale.y / 2;
				targetBottomPos = targetTopPos;
				break;
			case DoorOpenDirection.Bottom:
				targetBottomPos = _originalBottomPanelPosition;
				targetBottomPos.y -= _originalPanelScale.y / 2 - targetScale.y / 2;
				targetTopPos = targetBottomPos;
				break;
			case DoorOpenDirection.TopAndBottom:
				targetTopPos = _originalTopPanelPosition;
				targetTopPos.y += _originalPanelScale.y / 2 - targetScale.y / 2;
				targetBottomPos = _originalBottomPanelPosition;
				targetBottomPos.y -= _originalPanelScale.y / 2 - targetScale.y / 2;
				break;
			default:
				targetTopPos = Vector3.zero;
				targetBottomPos = Vector3.zero;
				break;
		}
		
		yield return new WaitForSeconds(openLag);

		if (_audioSource.isPlaying) _audioSource.Pause();
		if (operationSound) _audioSource.PlayOneShot(operationSound.Source);
		
		float time = openTime * (1 - (_topPanel.transform.localPosition.y - _originalTopPanelPosition.y) / (targetTopPos.y - _originalTopPanelPosition.y));
		
		while (time > 0) {
			yield return null;
			time -= Time.deltaTime;
			float lerpCoefficient = 1 - time / openTime;
			_topPanel.transform.localScale = Vector3.Lerp(_originalPanelScale, targetScale, lerpCoefficient);
			_bottomPanel.transform.localScale = _topPanel.transform.localScale;
			_topPanel.transform.localPosition = Vector3.Lerp(_originalTopPanelPosition, targetTopPos, lerpCoefficient);
			_bottomPanel.transform.localPosition = Vector3.Lerp(_originalBottomPanelPosition, targetBottomPos, lerpCoefficient);
		}

		_topPanel.transform.localScale = targetScale;
		_bottomPanel.transform.localScale = targetScale;
		_topPanel.transform.localPosition = targetTopPos;
		_bottomPanel.transform.localPosition = targetBottomPos;

		_openCoroutine = null;
	}

	protected IEnumerator ExeCloseTask() {
		_topPanel.GetComponent<Collider>().isTrigger = false;
		_bottomPanel.GetComponent<Collider>().isTrigger = false;
		
		Vector3 targetScale;
		Vector3 targetTopPos;
		Vector3 targetBottomPos;
		
		targetScale = _originalPanelScale;
		targetScale.y *= frameDoorRatio;
		
		switch (openDirection) {
			case DoorOpenDirection.Top:
				targetTopPos = _originalTopPanelPosition;
				targetTopPos.y += _originalPanelScale.y / 2 - targetScale.y / 2;
				targetBottomPos = targetTopPos;
				break;
			case DoorOpenDirection.Bottom:
				targetBottomPos = _originalBottomPanelPosition;
				targetBottomPos.y -= _originalPanelScale.y / 2 - targetScale.y / 2;
				targetTopPos = targetBottomPos;
				break;
			case DoorOpenDirection.TopAndBottom:
				targetTopPos = _originalTopPanelPosition;
				targetTopPos.y += _originalPanelScale.y / 2 - targetScale.y / 2;
				targetBottomPos = _originalBottomPanelPosition;
				targetBottomPos.y -= _originalPanelScale.y / 2 - targetScale.y / 2;
				break;
			default:
				targetTopPos = Vector3.zero;
				targetBottomPos = Vector3.zero;
				break;
		}
		
		yield return new WaitForSeconds(closeLag);

		if (_audioSource.isPlaying) _audioSource.Pause();
		if (operationSound) _audioSource.PlayOneShot(operationSound.Source);
		
		float time = openTime * (1 - (_topPanel.transform.localPosition.y - targetTopPos.y) / (_originalTopPanelPosition.y - targetTopPos.y));

		while (time > 0) {
			yield return null;
			time -= Time.deltaTime;
			float lerpCoefficient = 1 - time / openTime;
			_topPanel.transform.localScale = Vector3.Lerp(targetScale, _originalPanelScale, lerpCoefficient);
			_bottomPanel.transform.localScale = _topPanel.transform.localScale;
			_topPanel.transform.localPosition = Vector3.Lerp(targetTopPos, _originalTopPanelPosition, lerpCoefficient);
			_bottomPanel.transform.localPosition = Vector3.Lerp(targetBottomPos, _originalBottomPanelPosition, lerpCoefficient);
		}

		_topPanel.transform.localScale = _originalPanelScale;
		_bottomPanel.transform.localScale = _originalPanelScale;
		_topPanel.transform.localPosition = _originalTopPanelPosition;
		_bottomPanel.transform.localPosition = _originalBottomPanelPosition;

		_closeCoroutine = null;
	}
}

public enum DoorOpenDirection {
	Top,
	Bottom,
	TopAndBottom
}