using UnityEngine;

public class ForceShieldController : MonoBehaviour {

	[Range(0, 10)]
	public float distortionPower = 1f;
	[Range(0, 10)]
	public float rimlightPower = 1f;
	public float intersectionPower = 2f;
	public float distanceFactor = 60f;
	public float timeFactor = 30f;
	public float totalFactor = 1f;
	[Range(0, 2)]
	public float waveWidth = .3f;
	public float waveSpeed = .3f;
	public float maxWaveDistance = 1f;

	private Camera _camera;
	private Material _material;
	private SphereCollider _collider;
	private float[] _waveStartTimes = { 0, 0, 0, 0 };
	private float[] _waveDistances = { 10000f, 10000f, 10000f, 10000f };
	private Matrix4x4 _hits = Matrix4x4.zero;
	private int _hitIndex;

	private void Awake() {
		_camera = Camera.main;
		_material = GetComponent<Renderer>().material;
		_collider = GetComponent<SphereCollider>();
	}

	private void Update() {
		for (int i = 0; i < 4; i++) {
			float dt = Time.time - _waveStartTimes[i];
			_waveDistances[i] = dt * waveSpeed;
		}
		
		_material.SetFloat(ShaderManager.DISTORTION_POWER_KEYWORD, distortionPower);
		_material.SetFloat(ShaderManager.RIMLIGHT_POWER_KEYWORD, rimlightPower);
		_material.SetFloat(ShaderManager.INTERSECTION_POWER_KEYWORD, intersectionPower);
		_material.SetFloat(ShaderManager.DISTANCE_FACTOR_KEYWORD, distanceFactor);
		_material.SetFloat(ShaderManager.TIME_FACTOR_KEYWORD, timeFactor);
		_material.SetFloat(ShaderManager.TOTAL_FACTOR_KEYWORD, totalFactor);
		_material.SetFloat(ShaderManager.WAVE_WIDTH_KEYWORD, waveWidth);
		_material.SetFloat(ShaderManager.MAX_DISTANCE_KEYWORD, maxWaveDistance);
		_material.SetVector(ShaderManager.CURRENT_WAVE_DISTS_KEYWORD, new Vector4(_waveDistances[0], _waveDistances[1], _waveDistances[2], _waveDistances[3]));
		_material.SetMatrix(ShaderManager.HITS_KEYWORD, _hits);
	}

	private void OnCollisionEnter(Collision collision) {
		int layer = collision.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(collision.contacts[0].point));
			_waveStartTimes[_hitIndex] = Time.time;
		}
	}
	
	private void OnCollisionExit(Collision collision) {
		int layer = collision.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(collision.contacts[0].point));
			_waveStartTimes[_hitIndex] = Time.time;
		}
	}

	private void OnTriggerEnter(Collider other) {
		int layer = other.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			Vector3 diff = transform.position - other.transform.position;
			RaycastHit hitInfo;
			Vector3 contactPosition = other.Raycast(new Ray(other.transform.position - diff * 2, diff), out hitInfo, diff.magnitude * 2) ? hitInfo.point : _collider.ClosestPoint(other.transform.position);
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(contactPosition));
			_waveStartTimes[_hitIndex] = Time.time;
		}
	}

	private void OnTriggerExit(Collider other) {
		int layer = other.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			Vector3 diff = transform.position - other.transform.position;
			RaycastHit hitInfo;
			Vector3 contactPosition = other.Raycast(new Ray(other.transform.position - diff * 2, diff), out hitInfo, diff.magnitude * 2) ? hitInfo.point : _collider.ClosestPoint(other.transform.position);
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(contactPosition));
			_waveStartTimes[_hitIndex] = Time.time;
		}
	}
}