using System.Collections;
using UnityEngine;

public class HiddenAreaController : DeviceController {

	public bool oneTime;
	public float transitionDuration;
	[Range(0f, .2f)]
	public float edgeLength;
	[Range(0f, 1f)]
	public float startThreshold;

	private bool _isHidden;
	private MeshRenderer _renderer;
	private Material _material;
	private Coroutine _showCoroutine;
	private Coroutine _hideCoroutine;

	private void Awake() {
		_renderer = GetComponent<MeshRenderer>();
		_material = _renderer.material;
		_material.SetFloat(ShaderManager.MAX_DISTANCE_KEYWORD, CalculateMaxDistanceOnMesh(GetComponent<MeshFilter>().sharedMesh.vertices));
		_material.SetVector(ShaderManager.START_POINT_KEYWORD, new Vector4(8.5f, 9f, 0f));
		_material.SetFloat(ShaderManager.EDGE_LENGTH_KEYWORD, 0f);
	}

	private float CalculateMaxDistanceOnMesh(Vector3[] vertices) {
		float maxDistance = 0;
		for (int i = 0, l = vertices.Length; i < l; i++) {
			Vector3 v1 = vertices[i];
			for (int k = 0; k < l; k++) {
				if (i == k) continue;
				Vector3 v2 = vertices[k];
				float mag = (v1 - v2).sqrMagnitude;
				if (maxDistance < mag) maxDistance = mag;
			}
		}

		return Mathf.Sqrt(maxDistance);
	}

	public override void Play() {
		base.Play();
		_isHidden = true;
		_material.SetFloat(ShaderManager.THRESHOLD_KEYWORD, 0f);
		_renderer.enabled = true;
	}

	public void OnCharacterEnter(Vector3 position) {
		if (_isHidden) Show(position);
	}

	public void OnCharacterExit(Vector3 position) {
		if (_isHidden || oneTime) return;
		Hide(position);
	}

	private void Show(Vector3 position) {
		_isHidden = false;
		if (_hideCoroutine != null) {
			StopCoroutine(_hideCoroutine);
			_hideCoroutine = null;
		}

		_showCoroutine = _showCoroutine ?? StartCoroutine(ExeShowCoroutine(position));
	}

	private void Hide(Vector3 position) {
		_isHidden = true;
		if (_showCoroutine != null) {
			StopCoroutine(_showCoroutine);
			_showCoroutine = null;
		}

		_hideCoroutine = _hideCoroutine ?? StartCoroutine(ExeHideCoroutine(position));
	}

	private Vector4 GetPositionOnMesh(Vector3 position) {
		Vector4 pos = _renderer.bounds.ClosestPoint(position) - transform.position;
		pos.z = 0f;
		return pos;
	}

	private IEnumerator ExeShowCoroutine(Vector3 position) {
		bool flag = false;
		float startThreshold = _material.GetFloat(ShaderManager.THRESHOLD_KEYWORD);
		if (startThreshold < this.startThreshold) startThreshold = this.startThreshold;
		float startTime = Time.time - (startThreshold - this.startThreshold) / (1 - this.startThreshold) * transitionDuration;
		_material.SetVector(ShaderManager.START_POINT_KEYWORD, GetPositionOnMesh(position));
		_material.SetFloat(ShaderManager.EDGE_LENGTH_KEYWORD, edgeLength);
		while (!flag) {
			float lerp = Mathf.LerpUnclamped(this.startThreshold, 1f, (Time.time - startTime) / transitionDuration);
			if (lerp >= 1f) {
				flag = true;
				lerp = 1f;
			}
			
			_material.SetFloat(ShaderManager.THRESHOLD_KEYWORD, lerp);
			yield return null;
		}
		
		_renderer.enabled = false;
		_showCoroutine = null;
	}

	private IEnumerator ExeHideCoroutine(Vector3 position) {
		_renderer.enabled = true;
		bool flag = false;
		float startTime = Time.time - (1 - (_material.GetFloat(ShaderManager.THRESHOLD_KEYWORD) - startThreshold) / (1 - startThreshold)) * transitionDuration;
		_material.SetVector(ShaderManager.START_POINT_KEYWORD, GetPositionOnMesh(position));
		while (!flag) {
			float lerp = Mathf.LerpUnclamped(1f, startThreshold, (Time.time - startTime) / transitionDuration);
			if (lerp <= startThreshold) {
				flag = true;
				lerp = 0f;
			}
			
			_material.SetFloat(ShaderManager.THRESHOLD_KEYWORD, lerp);
			yield return null;
		}
		
		_material.SetFloat(ShaderManager.EDGE_LENGTH_KEYWORD, 0f);
		_hideCoroutine = null;
	}
}