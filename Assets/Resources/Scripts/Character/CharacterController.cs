using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

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
	public float dodgeCapacity;
	public float dodgeCooldown;
	public float stunnedRotation;
	public IBSpriteController[] carriedIBSpriteControllers;

	public bool hasAccelerationTrail;
	public TrailSettings accelerationTrailSettings;
	public bool hasDodgeTrail;
	public TrailSettings dodgeTrailSettings;

	public bool IsIBSpriteOn {
		get { return currentIBSpriteControllerIndex >= 0; }
	}

	public bool IsIBSpriteFull {
		get { return carriedIBSpriteControllers[carriedIBSpriteControllers.Length - 1] != null; }
	}

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

	protected bool _isInitiated;
	protected bool _isAccelerating;
	protected bool _isDodging;
	protected bool _isStunned;
	protected int _jumpTimes;
	protected int currentIBSpriteControllerIndex = -1;
	protected IBSpriteController currentIBSpriteController;
	protected CharacterMotor _characterMotor;
	protected TrailRenderer _trailRenderer;

	protected Coroutine stunCoroutine;

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
		if (_isStunned || _isDodging) return;
		_characterMotor.Dodge(3 * (float) _characterMotor.FaceDirection);
		StartCoroutine(ExeDodgeTask(dodgeInvincibilityTime));
	}

	public void Attack() {
		if (_isStunned) return;
		if (!currentIBSpriteController) return;
		currentIBSpriteController.Attack();
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

		currentIBSpriteControllerIndex = 0;
		currentIBSpriteController = carriedIBSpriteControllers[0];
		currentIBSpriteController.OnSwitchOn();
	}

	public void SwitchOffIBSprite() {
		if (!currentIBSpriteController) return;
		
		currentIBSpriteControllerIndex = -1;
		currentIBSpriteController.OnSwitchOff();
	}

	public void SwitchPreviousIBSprite() {
		if (currentIBSpriteControllerIndex == -1) return;

		int oldIndex = currentIBSpriteControllerIndex;
		currentIBSpriteControllerIndex--;
		if (currentIBSpriteControllerIndex < 0) {
			currentIBSpriteControllerIndex = carriedIBSpriteControllers.Length;
		}
		
		IBSpriteController controller = carriedIBSpriteControllers[currentIBSpriteControllerIndex];
		if (controller) {
			currentIBSpriteController = controller;
			currentIBSpriteController.OnSwitchOff();
			currentIBSpriteController.OnSwitchOn();
		} else {
			currentIBSpriteControllerIndex = oldIndex;
		}
	}

	public void SwitchNextIBSprite() {
		if (currentIBSpriteControllerIndex == -1) return;
		
		int oldIndex = currentIBSpriteControllerIndex;
		currentIBSpriteControllerIndex++;
		if (currentIBSpriteControllerIndex == carriedIBSpriteControllers.Length) {
			currentIBSpriteControllerIndex = 0;
		}

		IBSpriteController controller = carriedIBSpriteControllers[currentIBSpriteControllerIndex];
		if (controller) {
			currentIBSpriteController = controller;
			currentIBSpriteController.OnSwitchOff();
			currentIBSpriteController.OnSwitchOn();
		} else {
			currentIBSpriteControllerIndex = oldIndex;
		}
	}

	public void EquipIBSprite(IBSpriteController controller, bool autoSwitch = false) {
		if (IsIBSpriteFull) return;
		carriedIBSpriteControllers[CarriedIBSpriteCount] = controller;
		controller.characterMotor = _characterMotor;
		if (autoSwitch) {
			if (IsIBSpriteOn) {
				currentIBSpriteController.OnSwitchOff();
			}

			currentIBSpriteControllerIndex = CarriedIBSpriteCount - 1;
			currentIBSpriteController = controller;
			
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
		if (stunCoroutine == null) {
			currentIBSpriteController.CancelAttack();
		} else {
			ExitStunState();
		}
		
		EnterStunState(stunnedTime);
	}

	protected void SprayBlood() {
		
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
		settings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = true;
	}

	protected void DisableTrail() {
		_trailRenderer.emitting = false;
	}

	protected void EnterStunState(float stunnedTime) {
		_isStunned = true;
		stunCoroutine = StartCoroutine(ExeStunTask(stunnedTime));
	}

	protected void ExitStunState() {
		StopCoroutine(stunCoroutine);
		stunCoroutine = null;
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
}