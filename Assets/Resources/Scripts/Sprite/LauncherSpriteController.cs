using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class LauncherSpriteController : ShooterSpriteController {

	public int launchPerFire = 1;
	public float regenerationInterval = 2f;
	public float correctionPower;
	public Vector3 lockBoxOffset;
	public Vector3 lockBoxSize;
	public List<FireStyle> fireStyles;
	
	protected RocketProjectileController[] _rocketControllers;
	protected Collider[] _lockResults = new Collider[5];
	protected Queue<int> _emptySlots = new Queue<int>();
	
	protected Coroutine _regenerateCoroutine;

	protected override void Awake() {
		base.Awake();
		_rocketControllers = new RocketProjectileController[fireStyles.Count];
		for (int i = 0, l = _rocketControllers.Length; i < l; i++) _emptySlots.Enqueue(i);
	}

	public override void OnSwitchOn() {
		base.OnSwitchOn();
		for (int i = 0, l = _rocketControllers.Length; i < l; i++) _rocketControllers[i]?.gameObject.SetActive(true);
		if (_regenerateCoroutine == null && _emptySlots.Count > 0) _regenerateCoroutine = TimeManager.Instance.StartCoroutine(ExeRegenerateCoroutine());
	}

	public override void OnSwitchOff() {
		base.OnSwitchOff();
		for (int i = 0, l = _rocketControllers.Length; i < l; i++) _rocketControllers[i]?.gameObject.SetActive(false);
	}

	protected override void Update() {
		base.Update();
		float face = (float) characterMotor.FaceDirection;
		for (int i = 0, l =  _rocketControllers.Length; i < l; i++) {
			RocketProjectileController rocket = _rocketControllers[i];
			if (rocket) {
				Vector3 targetPosition = fireStyles[i].offset;
				targetPosition.x *= face;
				targetPosition += transform.position;
				rocket.transform.position = targetPosition;
				Vector3 targetRotation = fireStyles[i].rotation;
				targetRotation.z *= face;
				rocket.transform.eulerAngles = targetRotation;
			}
		}
	}

	protected override void Fire() {
		if (_regenerateCoroutine == null) _regenerateCoroutine = TimeManager.Instance.StartCoroutine(ExeRegenerateCoroutine());
		float face = (float) characterMotor.FaceDirection;
		Vector3 targetPos = Vector3.zero;
		Vector3 halfExtents = lockBoxSize / 2f;
		Vector3 offset = lockBoxOffset + new Vector3(halfExtents.x, halfExtents.y, 0f);
		halfExtents.x = Mathf.Abs(halfExtents.x);
		halfExtents.y = Mathf.Abs(halfExtents.y);
		offset.x *= face;
		int num = Physics.OverlapBoxNonAlloc(transform.position + offset, halfExtents, _lockResults, Quaternion.identity, 1 << LayerManager.CharacterLayer, QueryTriggerInteraction.Ignore);
		bool locked = false;
		Vector3 pos = transform.position;
		float minSqr = 1000f;
		Collider target = null;
		for (int i = 0; i < num; i++) {
			Collider collider = _lockResults[i];
			if (DetectionSettings.detectsEnemy && collider.CompareTag(TagManager.ENEMY_TAG) || DetectionSettings.detectsLocalPlayer && collider.CompareTag(TagManager.LOCAL_PLAYER_TAG) || DetectionSettings.detectsRemotePlayer && collider.CompareTag(TagManager.REMOTE_PLAYER_TAG)) {
				float deltaSqr = (collider.transform.position - pos).sqrMagnitude;
				if (deltaSqr < minSqr) {
					minSqr = deltaSqr;
					target = collider;
					locked = true;
				}
			}
		}

		if (!locked) {
			targetPos = transform.position;
			targetPos.x += face * 1000f;
		} else targetPos = target.transform.position;
		
		int shots = launchPerFire;
		for (int i = 0, l = _rocketControllers.Length; i < l; i++) {
			RocketProjectileController rocket = _rocketControllers[i];
			if (!rocket) continue;
			rocket.power = firePower;
			rocket.targetPos = targetPos;
			float rad = rocket.transform.eulerAngles.z * Mathf.Deg2Rad;
			rocket.Fire(new Vector3(Mathf.Cos(rad) * face, Mathf.Sin(rad), 0).normalized * firePower);
			_rocketControllers[i] = null;
			_emptySlots.Enqueue(i);
			if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
			if (fireShake != Vector3.zero) CameraManager.Shake(rocket.transform.position, fireShake);
			if (!string.IsNullOrEmpty(fireEffect)) {
				BurstParticleController explosion = ParticleManager.Get<BurstParticleController>(fireEffect);
				explosion.transform.position = rocket.transform.position;
				explosion.Burst();
			}
			
			shots--;
			if (shots == 0) break;
		}
	}

	protected IEnumerator ExeRegenerateCoroutine() {
		WaitForSeconds interval = new WaitForSeconds(regenerationInterval);

		do {
			yield return interval;
			for (int i = 0; i < launchPerFire; i++) {
				int index = _emptySlots.Dequeue();
				RocketProjectileController rocketController = ProjectileManager.Get<RocketProjectileController>(projectileType);
				LoadUpProjectile(rocketController);
				_rocketControllers[index] = rocketController;
				if (!gameObject.active) rocketController.gameObject.SetActive(false);
				if (_emptySlots.Count == 0) break;
			}
			
		} while (_emptySlots.Count > 0);

		_regenerateCoroutine = null;
	}

	protected override void LoadUpProjectile(ProjectileController projectileController) {
		base.LoadUpProjectile(projectileController);
		RocketProjectileController rocketController = projectileController as RocketProjectileController;
		rocketController.correctionPower = correctionPower;
	}

	[Serializable]
	public class FireStyle {
		public Vector3 offset;
		public Vector3 rotation;
	}
}