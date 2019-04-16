using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(CharacterMotor))]
public class CharacterController : MonoBehaviour {

	public event Action<float> onDamaged;
	public event Action onDeath;
	public event Action<CharacterController> onJump;

	public float dodgeAngle = -45f;

	public string characterName;
	public float health;
	public float maxHealth;
	public float maxStamina;
	public float stamina;
	public float staminaRecoveryDelay;
	public float staminaRecoveryRate;
	public float speed;
	public float acceleration;
	public float jumpPower;
	public float jumpTimes;
	public float jumpPowerDecay;
	public float dodgeSpeed;
	public float dodgeDuration;
	public int dodgeCapacity;
	public float dodgeCooldown;
	public ParticleAsset bloodSprayEffect = "cube_blood";
	public Gradient bloodColor;
	public AudioAsset bloodSpraySound = "blood_spray_0";
	public IBSpriteController[] carriedIBSpriteControllers;

	public bool hasAccelerationTrail;
	public TrailSettings accelerationTrailSettings;

	public bool hasDodgeTrail;
	public TrailSettings dodgeTrailSettings;
	public Material dodgeTrailMaterial;

	public bool IsIBSpriteOn => _currentIBSpriteControllerIndex >= 0;
	public bool IsIBSpriteFull => carriedIBSpriteControllers[carriedIBSpriteControllers.Length - 1] != null;

	public int CarriedIBSpriteCount {
		get {
			for (int i = 0; i < carriedIBSpriteControllers.Length; i++)
				if (!carriedIBSpriteControllers[i]) return i;
			return carriedIBSpriteControllers.Length;
		}
	}

	public bool IsDodgeCooldowning => _dodgeCooldownCoroutine != null;
	public FaceDirection FaceDirection => _characterMotor.FaceDirection;

	protected bool _isAccelerating;
	protected bool _isDodging;
	protected bool _isStunned;
	protected int _jumpTimes;
	protected int _dodgeTimes;
	protected int _currentIBSpriteControllerIndex = -1;
	protected IBSpriteController _currentIBSpriteController;
	protected CharacterMotor _characterMotor;
	protected CharacterTrailController _characterTrailController;
	protected TrailRenderer _trailRenderer;

	protected Coroutine _stunCoroutine;
	protected Coroutine _dodgeCooldownCoroutine;
	protected Coroutine _staminaRecoveryCoroutine;
/*
	[Conditional("UNITY_EDITOR")]
	private void OnGUI() {
		GUI.Button(new Rect(0f, 0f, 100f, 50f), (int) stamina + "/" + (int) maxStamina);
	}
*/
	public virtual void Awake() {
		_characterMotor = GetComponent<CharacterMotor>();
		_characterMotor.onGroundEnter += ResetJumpTimes;
		_characterMotor.onDodgeExit += FinishDodge;

		_characterTrailController = gameObject.AddComponent<CharacterTrailController>();
		
		_trailRenderer = gameObject.AddComponent<TrailRenderer>();
		_trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
		_trailRenderer.receiveShadows = false;
		_trailRenderer.allowOcclusionWhenDynamic = true;
		_trailRenderer.autodestruct = false;
		_trailRenderer.emitting = false;
	}

	private void OnDestroy() {
		StopAllCoroutines();
		foreach (var ibSpriteController in carriedIBSpriteControllers)
			if (ibSpriteController) Destroy(ibSpriteController.gameObject);
		carriedIBSpriteControllers = null;
	}

	public void MoveLeft() => Move(FaceDirection.Left);

	public void MoveRight() => Move(FaceDirection.Right);

	public void Move(FaceDirection direction) {
		if (_isStunned || _isDodging) return;
		float velocity = speed * (float) direction;
		if (_isAccelerating) velocity *= acceleration;
		if (direction != _characterMotor.FaceDirection) _characterMotor.Turn();
		_characterMotor.Move(velocity);
	}

	public void Turn() {
		if (_isStunned || _isDodging) return;
		_characterMotor.Turn();
	}

	public void Flip() {
		if (_isStunned || _isDodging) return;
		_characterMotor.Flip();
	}
	
	public void EnterAcceleratingState() {
		if (_isStunned || _isDodging) return;
		_isAccelerating = true;
		if (hasAccelerationTrail) EnableTrail(accelerationTrailSettings);
	}

	public void ExitAcceleratingState() {
		if (!_isAccelerating) return;
		_isAccelerating = false;
		if (hasAccelerationTrail) DisableTrail();
	}

	public void Dodge() {
		if (_isStunned || _isDodging || _dodgeTimes == dodgeCapacity) return;
		_dodgeTimes += 1;
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, dodgeAngle);
		_characterMotor.Dodge(dodgeSpeed * (float) _characterMotor.FaceDirection, dodgeDuration);
		_isDodging = true;
		if (hasDodgeTrail) EnableDodgeTrail(); /* EnableTrail(dodgeTrailSettings); */
		if (_dodgeCooldownCoroutine == null) _dodgeCooldownCoroutine = StartCoroutine(ExeDodgeCooldownCoroutine());
	}

	public void OnReceiveAttackCommand() {
		if (_isStunned) return;
		if (!_currentIBSpriteController) return;
		if (stamina <= 0f) return;
		_currentIBSpriteController.OnReceiveAttackCommand();
	}

	public void OnFinishAttackCommand() {
		if (!_currentIBSpriteController) return;
		_currentIBSpriteController.OnFinishAttackCommand();
	}

	public void Jump() {
		if (_isStunned || _isDodging) return;
		float power = jumpPower;
		if (_jumpTimes < jumpTimes) {
			for (int i = 0, l = _jumpTimes; i < l; i ++) power *= jumpPowerDecay;
			_jumpTimes++;
			_characterMotor.Jump(power);
			onJump?.Invoke(this);
		}
	}

	public void SwitchOnIBSprite() {
		if (carriedIBSpriteControllers.Length == 0) return;
		_currentIBSpriteControllerIndex = 0;
		_currentIBSpriteController = carriedIBSpriteControllers[0];
		_currentIBSpriteController.OnSwitchOn();
	}

	public void SwitchOffIBSprite() {
		if (!_currentIBSpriteController) return;
		_currentIBSpriteControllerIndex = -1;
		_currentIBSpriteController.OnSwitchOff();
	}

	public void SwitchPreviousIBSprite() {
		if (_currentIBSpriteControllerIndex == -1) return;
		int oldIndex = _currentIBSpriteControllerIndex;
		_currentIBSpriteControllerIndex--;
		if (_currentIBSpriteControllerIndex < 0) _currentIBSpriteControllerIndex = CarriedIBSpriteCount - 1;
		IBSpriteController controller = carriedIBSpriteControllers[_currentIBSpriteControllerIndex];
		if (controller) {
			_currentIBSpriteController = controller;
			carriedIBSpriteControllers[oldIndex].OnSwitchOff();
			_currentIBSpriteController.OnSwitchOn();
		} else _currentIBSpriteControllerIndex = oldIndex;
	}

	public void SwitchNextIBSprite() {
		if (_currentIBSpriteControllerIndex == -1) return;
		int oldIndex = _currentIBSpriteControllerIndex;
		_currentIBSpriteControllerIndex++;
		if (_currentIBSpriteControllerIndex == CarriedIBSpriteCount) _currentIBSpriteControllerIndex = 0;
		IBSpriteController controller = carriedIBSpriteControllers[_currentIBSpriteControllerIndex];
		if (controller) {
			_currentIBSpriteController = controller;
			carriedIBSpriteControllers[oldIndex].OnSwitchOff();
			_currentIBSpriteController.OnSwitchOn();
		} else _currentIBSpriteControllerIndex = oldIndex;
	}

	public void EquipIBSprite(IBSpriteController controller, bool autoSwitch = true) {
		if (IsIBSpriteFull) return;
		carriedIBSpriteControllers[CarriedIBSpriteCount] = controller;
		controller.characterMotor = _characterMotor;
		controller.characterController = this;
		DetectionSettings detectionSettings = new DetectionSettings();
		if (CompareTag(TagManager.ENEMY_TAG)) {
			detectionSettings.detectsLocalPlayer = true;
			detectionSettings.detectsEnemy = false;
		} else {
			detectionSettings.detectsLocalPlayer = false;
			detectionSettings.detectsEnemy = true;
		}
		
		controller.DetectionSettings = detectionSettings;
		if (autoSwitch) {
			if (IsIBSpriteOn) _currentIBSpriteController.OnSwitchOff();
			_currentIBSpriteControllerIndex = CarriedIBSpriteCount - 1;
			_currentIBSpriteController = controller;
			controller.OnSwitchOn();
		}
	}
	
	public void CostStamina(float value) {
		stamina -= value;
		if (stamina < 0f) stamina = 0f;
	}

	public void StartStaminaRecovery() {
		if (_staminaRecoveryCoroutine == null) _staminaRecoveryCoroutine = StartCoroutine(ExeStaminaRecoveryCoroutine());
	}

	public void InterruptStaminaRecovery() {
		if (_staminaRecoveryCoroutine != null) {
			StopCoroutine(_staminaRecoveryCoroutine);
			_staminaRecoveryCoroutine = null;
		}
	}

	public void GetDamaged(float damage) {
		if (_isDodging) return;
		SprayBlood();
		health -= damage;
		onDamaged?.Invoke(damage);
		if (health <= 0f) {
			health = 0f;
			Die();
		}
	}

	public void GetHit(float velocityX, float velocityY = 0f) {
		if (_isDodging) return;
		_characterMotor.GetHit(velocityX, velocityY);
	}

	public void GetStunned(float stunnedAngle, float stunnedTime) {
		if (_isDodging) return;
		if (_stunCoroutine == null) _currentIBSpriteController?.CancelAttack();
		else ExitStunState();
		EnterStunState(stunnedAngle * (float) FaceDirection, stunnedTime);
	}

	public void ComeAlive() {
		
		health = maxHealth;
		stamina = maxStamina;
	}

	protected void SprayBlood() {
		BurstParticleController blood = bloodSprayEffect.Get<BurstParticleController>();
		ParticleSystem.MainModule main = blood.ParticleSystem.main;
		main.startColor = bloodColor;
		blood.transform.position = transform.position;
		blood.Burst();
		AudioManager.PlayAtPoint(bloodSpraySound, transform.position);
	}
	
	protected void Die() {
		onDeath?.Invoke();
		Destroy(gameObject);
	}

	protected void ResetJumpTimes() => _jumpTimes = 0;

	protected void FinishDodge() {
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		_isDodging = false;
		if (hasDodgeTrail) {
			// DisableTrail();
			DisableDodgeTrail();
			if (_isAccelerating)
				if (hasAccelerationTrail) EnableTrail(accelerationTrailSettings);
		}
	}
	
	protected void EnableTrail(TrailSettings settings) {
		_trailRenderer.Clear();
		settings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = true;
	}

	protected void DisableTrail() {
		_trailRenderer.emitting = false;
		_trailRenderer.Clear();
	}

	protected void EnableDodgeTrail() => _characterTrailController.Fire(dodgeTrailMaterial);

	protected void DisableDodgeTrail() => _characterTrailController.Stop();

	protected void EnterStunState(float stunnedAngle, float stunnedTime) {
		_isStunned = true;
		_stunCoroutine = StartCoroutine(ExeStunCoroutine(stunnedAngle, stunnedTime));
	}

	protected void ExitStunState() {
		StopCoroutine(_stunCoroutine);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		_stunCoroutine = null;
		_isStunned = false;
	}

	protected IEnumerator ExeStunCoroutine(float stunnedAngle, float stunnedTime) {
		float yRotation = transform.eulerAngles.y;
		transform.eulerAngles = new Vector3(0, yRotation, stunnedAngle);
		yield return new WaitForSeconds(stunnedTime);
		transform.eulerAngles = new Vector3(0, yRotation, 0);
		ExitStunState();
	}

	protected IEnumerator ExeDodgeCooldownCoroutine() {
		while (true) {
			float endTime = Time.time + dodgeCooldown;
			do yield return null;
			while (Time.time < endTime);
			_dodgeTimes -= 1;
			if (_dodgeTimes == 0) break;
		}

		_dodgeCooldownCoroutine = null;
	}

	protected IEnumerator ExeStaminaRecoveryCoroutine() {
		yield return new WaitForSeconds(staminaRecoveryDelay);
		while (stamina < maxStamina) {
			yield return null;
			stamina += maxStamina * staminaRecoveryRate * Time.deltaTime;
		}
		
		stamina = maxStamina;
		_staminaRecoveryCoroutine = null;
	}
}