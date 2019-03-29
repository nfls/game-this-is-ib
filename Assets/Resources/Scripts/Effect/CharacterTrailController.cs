using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterTrailController : MonoBehaviour {

	public float interval = .5f;
	public float fadeOutTime = 1f;

	private readonly List<TrailObjectController> _busyTrailObjects = new List<TrailObjectController>();
	private readonly Queue<TrailObjectController>  _idleTrailObjects = new Queue<TrailObjectController>();
	private string _name;
	private Mesh _mesh;
	private bool _isFiring;
	private Vector3 _lastPosition;
	private Material _material;
	private Coroutine _delayFireCoroutine;
	private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

	private void Awake() {
		Mesh mesh = new Mesh();
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
		int length = filters.Length;
		CombineInstance[] instances = new CombineInstance[length];

		for (int i = 0; i < length; i++) instances[i] = new CombineInstance { mesh = filters[i].sharedMesh, transform = filters[i].transform.localToWorldMatrix };
		mesh.CombineMeshes(instances, true, true);
		_mesh = mesh;

		_name = "New Character Trail Object";
	}

	private TrailObjectController GenerateTrailObject(Mesh mesh) {
		GameObject go = new GameObject(_name);
		go.transform.localScale = new Vector3(.8f, .8f, .8f);
		go.AddComponent<MeshFilter>().mesh = mesh;
		MeshRenderer renderer = go.AddComponent<MeshRenderer>();
		renderer.lightProbeUsage = LightProbeUsage.Off;
		renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		renderer.shadowCastingMode = ShadowCastingMode.Off;
		renderer.receiveShadows = false;
		return go.AddComponent<TrailObjectController>();
	}

	private TrailObjectController GetTrailObject() => _idleTrailObjects.Count > 0 ? _idleTrailObjects.Dequeue() : GenerateTrailObject(_mesh);

	private void Update() {
		if (_isFiring) {
			Vector3 currentPosition = transform.position;
			Vector3 direction = currentPosition - _lastPosition;
			float sqr = direction.sqrMagnitude;
			Vector3 lastPosition = _lastPosition;
			if (sqr >= interval * interval) {
				int times = (int) (Mathf.Sqrt(sqr) / interval);
				for (int i = 0; i < times; i++) {
					TrailObjectController trailObjectController = GetTrailObject().Fire(this, fadeOutTime, _material);
					Vector3 position = lastPosition + direction.normalized * interval * i;
					_lastPosition = position;
					trailObjectController.transform.SetPositionAndRotation(position, transform.rotation);
					_busyTrailObjects.Add(trailObjectController);
				}
			}
		}
	}

	public void Fire(Material material) {
		_material = material;
		if (_delayFireCoroutine != null) StopCoroutine(_delayFireCoroutine);
		_delayFireCoroutine = StartCoroutine(ExeDelayFireCoroutine());
	}

	private IEnumerator ExeDelayFireCoroutine() {
		yield return _waitForFixedUpdate;
		_isFiring = true;
		_lastPosition = transform.position;
	}

	public void Stop(bool reset = false) {
		_isFiring = false;
		if (reset) foreach (var trailObject in _busyTrailObjects) trailObject.FadeOut();
	}

	public void Recycle(TrailObjectController trailObjectController) {
		_busyTrailObjects.Remove(trailObjectController);
		_idleTrailObjects.Enqueue(trailObjectController);
	}
}

public class TrailObjectController : MonoBehaviour {

	private float _fadeOutTime;
	private float _startTime;
	private float _endTime;
	private bool _isFiring;
	private CharacterTrailController _trailController;
	private MeshRenderer _renderer;

	private void Awake() => _renderer = GetComponent<MeshRenderer>();

	private void Update() {
		if (_isFiring && Time.time > _endTime) FadeOut();
	}

	public TrailObjectController Fire(CharacterTrailController trailController, float fadeOutTime, Material material) {
		gameObject.SetActive(true);
		_trailController = trailController;
		_fadeOutTime = fadeOutTime;
		_startTime = Time.time;
		_endTime = _startTime + _fadeOutTime;
		_isFiring = true;
		_renderer.material = material;
		return this;
	}

	public void FadeOut() {
		_isFiring = false;
		gameObject.SetActive(false);
		_trailController.Recycle(this);
	}
}