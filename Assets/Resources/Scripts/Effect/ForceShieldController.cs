using UnityEngine;

public class ForceShieldController : MonoBehaviour {

	[Range(0, 10)]
	public float distortionPower = 1f;
	[Range(1, 10)]
	public float rimlightPower = 1f;
	[Range(0, 5)]
	public float intersectionPower = 2f;
	public float hitAreaRadius = .3f;
	public float hitAreaTransitionTime = 2f;

	private Material _material;
	private SphereCollider _collider;
	private float[] _hitAreaStartTimes = { 0, 0, 0, 0 };
	private Vector4 _hitAreaAlpha = Vector4.zero;
	private Matrix4x4 _hits = Matrix4x4.zero;
	private int _hitIndex;

	private void Awake() {
		_material = GetComponent<Renderer>().material;
		_collider = GetComponent<SphereCollider>();
	}

	private void Update() {
		float time = Time.time;
		_hitAreaAlpha.x = 1 - (time - _hitAreaStartTimes[0]) / hitAreaTransitionTime;
		_hitAreaAlpha.y = 1 - (time - _hitAreaStartTimes[1]) / hitAreaTransitionTime;
		_hitAreaAlpha.z = 1 - (time - _hitAreaStartTimes[2]) / hitAreaTransitionTime;
		_hitAreaAlpha.w = 1 - (time - _hitAreaStartTimes[3]) / hitAreaTransitionTime;
		if (_hitAreaAlpha.x < 0) _hitAreaAlpha.x = 0;
		if (_hitAreaAlpha.y < 0) _hitAreaAlpha.y = 0;
		if (_hitAreaAlpha.z < 0) _hitAreaAlpha.z = 0;
		if (_hitAreaAlpha.w < 0) _hitAreaAlpha.w = 0;
		
		_material.SetFloat(ShaderManager.DISTORTION_POWER_KEYWORD, distortionPower);
		_material.SetFloat(ShaderManager.RIMLIGHT_POWER_KEYWORD, rimlightPower);
		_material.SetFloat(ShaderManager.INTERSECTION_POWER_KEYWORD, intersectionPower);
		_material.SetFloat(ShaderManager.MAX_DISTANCE_KEYWORD, hitAreaRadius);
		_material.SetVector(ShaderManager.HIT_AREA_ALPHAS_KEYWORD, _hitAreaAlpha);
		_material.SetMatrix(ShaderManager.HITS_KEYWORD, _hits);
	}

	private void OnCollisionEnter(Collision collision) {
		int layer = collision.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(collision.contacts[0].point));
			_hitAreaStartTimes[_hitIndex] = Time.time;
		}
	}

	private void OnCollisionExit(Collision collision) {
		int layer = collision.gameObject.layer;
		if (layer == LayerManager.SpriteLayer || layer == LayerManager.ProjectileLayer) {
			if (++_hitIndex == 4) _hitIndex = 0;
			_hits.SetRow(_hitIndex, transform.InverseTransformPoint(collision.transform.position));
			_hitAreaStartTimes[_hitIndex] = Time.time;
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
			_hitAreaStartTimes[_hitIndex] = Time.time;
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
			_hitAreaStartTimes[_hitIndex] = Time.time;
		}
	}
}