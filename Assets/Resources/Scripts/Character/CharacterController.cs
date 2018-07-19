using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterMotor))]
public class CharacterController : MonoBehaviour {

	public delegate void OnDamagedHandler(float damage);
	public delegate void OnDeathHandler();

	public event OnDamagedHandler OnDamaged;
	public event OnDeathHandler OnDeath;
	
	public string name;
	public float health;
	public float speed;
	public float acceleration;
	public float jumpPower;
	public float jumpTimes;
	public float jumpPowerDecay;
	public float dodgeDistance;
	public float dodgeInvincibilityTime;
	public int dodgeCapacity;
	public float dodgeCooldown;
	public float stunnedRotation;
	public IBSpriteController[] carriedIBSpriteControllers;

	public bool hasAccelerationTrail;
	public TrailSettings accelerationTrailSettings;
	public bool hasDodgeTrail;
	public TrailSettings dodgeTrailSettings;

	public bool IsIBSpriteOn => _currentIBSpriteControllerIndex >= 0;
	public bool IsIBSpriteFull => carriedIBSpriteControllers[carriedIBSpriteControllers.Length - 1] != null;

	public int CarriedIBSpriteCount {
		get {
			for (int i = 0; i < carriedIBSpriteControllers.Length; i++) {
				if (!carriedIBSpriteControllers[i]) {
					return i;
				}
			}

			return carriedIBSpriteControllers.Length;
		}
	}

	public bool IsDodgeCooldowning => _dodgeCooldownCoroutine != null;
	public float DodgeCooldownDuration => _dodgeCooldownDuration;

	protected bool _isInitiated;
	protected bool _isAccelerating;
	protected bool _isDodging;
	protected bool _isStunned;
	protected int _jumpTimes;
	protected int _dodgeTimes;
	protected int _currentIBSpriteControllerIndex = -1;
	protected float _dodgeCooldownDuration;
	protected IBSpriteController _currentIBSpriteController;
	protected CharacterMotor _characterMotor;
	protected TrailRenderer _trailRenderer;

	protected Coroutine _stunCoroutine;
	protected Coroutine _dodgeCooldownCoroutine;

	private void Start() {
		if (_isInitiated) return;
		Init();
	}

	public virtual void Init() {
		_isInitiated = true;
		
		_characterMotor = GetComponent<CharacterMotor>();
		_characterMotor.OnGroundEnter += ResetJumpTimes;
		
		_trailRenderer = gameObject.AddComponent<TrailRenderer>();
		_trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
		_trailRenderer.receiveShadows = false;
		_trailRenderer.allowOcclusionWhenDynamic = true;
		_trailRenderer.autodestruct = false;
		_trailRenderer.emitting = false;
		
		accelerationTrailSettings.Init();
		dodgeTrailSettings.Init();
	}

	public void MoveLeft() {
		Move(FaceDirection.Left);
	}

	public void MoveRight() {
		Move(FaceDirection.Right);
	}

	public void Move(FaceDirection direction) {
		if (_isStunned) return;

		float velocity = speed * (float) direction;
		if (_isAccelerating) {
			velocity *= acceleration;
		}
		if (direction != _characterMotor.FaceDirection) {
			_characterMotor.Flip();
		}
		_characterMotor.Move(velocity);
	}

	public void EnterAcceleratingState() {
		if (_isStunned || _isDodging) return;
		_isAccelerating = true;
		if (hasAccelerationTrail) {
			EnableTrail(accelerationTrailSettings);
		}
	}

	public void ExitAcceleratingState() {
		if (!_isAccelerating) return;
		_isAccelerating = false;
		if (hasAccelerationTrail) {
			DisableTrail();
		}
	}

	public void Dodge() {
		if (_isStunned || _isDodging || _dodgeTimes == dodgeCapacity) return;
		_dodgeTimes += 1;
		_characterMotor.Dodge(dodgeDistance * (float) _characterMotor.FaceDirection);
		StartCoroutine(ExeDodgeTask(dodgeInvincibilityTime));
		if (_dodgeCooldownCoroutine == null) _dodgeCooldownCoroutine = StartCoroutine(ExeDodgeCooldownTask());
	}

	public void Attack() {
		if (_isStunned) return;
		if (!_currentIBSpriteController) return;
		_currentIBSpriteController.Attack();
	}

	public void Jump() {
		if (_isStunned) return;
		float power = jumpPower;
		if (_jumpTimes < jumpTimes) {
			for (int i = 0, l = _jumpTimes; i < l; i ++) {
				power *= jumpPowerDecay;
			}

			_jumpTimes++;
			_characterMotor.Jump(power);
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
		if (_currentIBSpriteControllerIndex < 0) {
			_currentIBSpriteControllerIndex = CarriedIBSpriteCount;
		}
		
		IBSpriteController controller = carriedIBSpriteControllers[_currentIBSpriteControllerIndex];
		if (controller) {
			_currentIBSpriteController = controller;
			_currentIBSpriteController.OnSwitchOff();
			_currentIBSpriteController.OnSwitchOn();
		} else {
			_currentIBSpriteControllerIndex = oldIndex;
		}
	}

	public void SwitchNextIBSprite() {
		if (_currentIBSpriteControllerIndex == -1) return;
		
		int oldIndex = _currentIBSpriteControllerIndex;
		_currentIBSpriteControllerIndex++;
		if (_currentIBSpriteControllerIndex == CarriedIBSpriteCount) {
			_currentIBSpriteControllerIndex = 0;
		}

		IBSpriteController controller = carriedIBSpriteControllers[_currentIBSpriteControllerIndex];
		if (controller) {
			_currentIBSpriteController = controller;
			_currentIBSpriteController.OnSwitchOff();
			_currentIBSpriteController.OnSwitchOn();
		} else {
			_currentIBSpriteControllerIndex = oldIndex;
		}
	}

	public void EquipIBSprite(IBSpriteController controller, bool autoSwitch = false) {
		if (IsIBSpriteFull) return;
		carriedIBSpriteControllers[CarriedIBSpriteCount] = controller;
		controller.characterMotor = _characterMotor;
		if (autoSwitch) {
			if (IsIBSpriteOn) {
				_currentIBSpriteController.OnSwitchOff();
			}

			_currentIBSpriteControllerIndex = CarriedIBSpriteCount - 1;
			_currentIBSpriteController = controller;
			
			controller.OnSwitchOn();
		}
	}

	public void GetDamaged(float damage) {
		if (_isDodging) return;
		SprayBlood();
		health -= damage;
		if (OnDamaged != null) {
			OnDamaged(damage);
		}
		
		if (health <= 0f) {
			health = 0f;
			Die();
		}
	}

	public void GetHit(float velocityX, float velocityY = 0f) {
		if (_isDodging) return;
		_characterMotor.GetHit(velocityX, velocityY);
	}

	public void GetStunned(float stunnedTime) {
		if (_isDodging) return;
		if (_stunCoroutine == null) {
			_currentIBSpriteController.CancelAttack();
		} else {
			ExitStunState();
		}
		
		EnterStunState(stunnedTime);
	}

	protected void SprayBlood() {
		BurstParticleController blood = ParticlePool.Get<BurstParticleController>("cubeblood");
		blood.transform.position = transform.position;
		blood.Spray();
	}
	
	protected void Die() {
		if (OnDeath != null) {
			OnDeath();
		}
	}

	protected void ResetJumpTimes() {
		_jumpTimes = 0;
	}
	
	protected void EnableTrail(TrailSettings settings) {
		_trailRenderer.Clear();
		settings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = true;
	}

	protected void DisableTrail() {
		_trailRenderer.emitting = false;
	}

	protected void EnterStunState(float stunnedTime) {
		_isStunned = true;
		_stunCoroutine = StartCoroutine(ExeStunTask(stunnedTime));
	}

	protected void ExitStunState() {
		StopCoroutine(_stunCoroutine);
		_stunCoroutine = null;
		_isStunned = false;
	}

	protected IEnumerator ExeStunTask(float stunnedTime) {
		transform.Rotate(0f, 0f, stunnedRotation * (float) _characterMotor.FaceDirection);
		yield return new WaitForSeconds(stunnedTime);
		transform.rotation = Quaternion.identity;
	}

	protected IEnumerator ExeDodgeTask(float dodgeInvincibilityTime) {
		_isDodging = true;
		if (hasDodgeTrail) {
			EnableTrail(dodgeTrailSettings);
		}
		
		yield return new WaitForSeconds(dodgeInvincibilityTime);
		if (hasDodgeTrail) {
			DisableTrail();
			if (_isAccelerating) {
				if (hasAccelerationTrail) {
					EnableTrail(accelerationTrailSettings);
				}
			}
		}
		
		_isDodging = false;
	}

	protected IEnumerator ExeDodgeCooldownTask() {
		while (true) {
			_dodgeCooldownDuration = dodgeCooldown;
			while (_dodgeCooldownDuration > 0) {
				yield return null;
				_dodgeCooldownDuration -= Time.deltaTime;
			}
			_dodgeTimes -= 1;
			if (_dodgeTimes == 0) {
				break;
			}
		}

		_dodgeCooldownCoroutine = null;
	}
}