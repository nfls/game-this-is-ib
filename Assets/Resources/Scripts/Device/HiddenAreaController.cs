using UnityEngine;

public class HiddenAreaController : DeviceController {

	public bool oneTime;

	private bool _isHidden;
	private MeshRenderer[] _renderers;

	private void Awake() {
		_renderers = GetComponentsInChildren<MeshRenderer>();
		StaticBatchingUtility.Combine(gameObject);
	}

	public override void Play() {
		base.Play();
		Hide();
	}

	public void OnCharacterExit() {
		if (oneTime) return;
		Hide();
	}

	public void Show() {
		foreach (var renderer in _renderers) renderer.enabled = false;
	}

	public void Hide() {
		foreach (var renderer in _renderers) renderer.enabled = true;
	}
}