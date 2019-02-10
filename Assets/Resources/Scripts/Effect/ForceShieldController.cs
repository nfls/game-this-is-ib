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
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (_collider.Raycast(ray, out hitInfo, 100f)) {
				if (++_hitIndex == 4) _hitIndex = 0;
				_hits.SetRow(_hitIndex, transform.InverseTransformPoint(hitInfo.point));
				_waveStartTimes[_hitIndex] = Time.time;
			}
		}

		for (int i = 0; i < 4; i++) {
			float dt = Time.time - _waveStartTimes[i];
			_waveDistances[i] = dt * waveSpeed;
		}
		
		_material.SetFloat("_DistortionPower", distortionPower);
		_material.SetFloat("_RimlightPower", rimlightPower);
		_material.SetFloat("_IntersectionPower", intersectionPower);
		_material.SetFloat("_DistanceFactor", distanceFactor);
		_material.SetFloat("_TimeFactor", timeFactor);
		_material.SetFloat("_TotalFactor", totalFactor);
		_material.SetFloat("_WaveWidth", waveWidth);
		_material.SetFloat("_MaxWaveDistance", maxWaveDistance);
		_material.SetVector("_CurrentWaveDists", new Vector4(_waveDistances[0], _waveDistances[1], _waveDistances[2], _waveDistances[3]));
		_material.SetMatrix("_Hits", _hits);
	}
}