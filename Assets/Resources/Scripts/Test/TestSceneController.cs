using Cinemachine;
using UnityEngine;

public class TestSceneController : MonoBehaviour {

	public ParticleAsset hitEffect;
	public CinemachineTargetGroup mainTargetGroup;
	public CinemachineVirtualCamera combatVirtualCamera;
	public TimeEffectRequest combatTimeEffect;
	public LevelController level;
	public AudioClip bgm;
	public bool openCombatClearShot;

	private void Start() {
		LocalDataManager.Init();
		ResourcesManager.Init();
		PlayerManager.Init();
		JoystickUtil.Init();

		SingletonManager.AddSingleton<ShaderManager>();
		SingletonManager.AddSingleton<CameraManager>();
		SingletonManager.AddSingleton<TimeManager>();
		SingletonManager.AddSingleton<ParticleManager>();
		SingletonManager.AddSingleton<ParticleDecalManager>();
		SingletonManager.AddSingleton<ProjectileManager>();
		SingletonManager.AddSingleton<AudioManager>();
		SingletonManager.AddSingleton<InputManager>();
		SingletonManager.AddSingleton<UIManager>();
		
		level.Activate();

		GameObject localPlayer = CharacterFactory.GenerateLocalPlayer();
		GameObject interactionSystem = new GameObject("Interaction System");
		interactionSystem.transform.localScale = new Vector3(6f, 3f, 4f);
		interactionSystem.layer = LayerManager.InteractionLayer;
		localPlayer.AddComponent<InputOperator>().InteractionSystem = interactionSystem.AddComponent<InteractionSystem>();
		GameObject sprite0 = IBSpriteFactory.GenerateIBSprite("up_punch");
		GameObject sprite1 = IBSpriteFactory.GenerateIBSprite("complex_shooter");
		GameObject sprite2 = IBSpriteFactory.GenerateIBSprite("hammer");
		GameObject sprite3 = IBSpriteFactory.GenerateIBSprite("rocket_launcher");
		GameObject sprite4 = IBSpriteFactory.GenerateIBSprite("complex_newton_thrower");
		GameObject sprite5 = IBSpriteFactory.GenerateIBSprite("shooter");
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite0.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite1.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite2.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite3.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite4.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(sprite5.GetComponent<IBSpriteController>());
		// localPlayer.transform.position = new Vector3(3, 1, 0);
		localPlayer.transform.position = new Vector3(4, 8, 0);
		
		mainTargetGroup.m_Targets = new[] { new CinemachineTargetGroup.Target { target = localPlayer.transform, weight = 1f, radius = 5f} };
		combatVirtualCamera.Follow = localPlayer.transform;
		combatVirtualCamera.LookAt = localPlayer.transform;
		
		GameObject enemy0 = CharacterFactory.GenerateLocalPlayer();
		TagManager.SetAllTags(enemy0, TagManager.ENEMY_TAG);
		enemy0.name = "Enemy 0";
		enemy0.transform.position = new Vector3(10, 8, 0);
		GameObject sprite6 = IBSpriteFactory.GenerateIBSprite("straight_punch");
		enemy0.GetComponent<CharacterController>().EquipIBSprite(sprite6.GetComponent<IBSpriteController>());
		enemy0.AddComponent<NaiveAIOperator>();
		/*
		GameObject enemy1 = CharacterFactory.GenerateLocalPlayer();
		TagManager.SetAllTags(enemy1, TagManager.ENEMY_TAG);
		enemy1.name = "Enemy 1";
		enemy1.transform.position = new Vector3(12, 8, 0);
		*/
		
		if (bgm) {
			AudioSource source = GetComponent<AudioSource>();
			source.clip = bgm;
			source.Play();
			source.loop = true;
			source.spatialize = false;
		}

		combatComposer = combatVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
	}

	private float endTime;
	private CinemachineComposer combatComposer;

	private void Update() {
		if (endTime > 0f) {
			combatTimeEffect.lifeRemained = 1f;
			if (endTime <= Time.time) ExitCombatClearShot();
		}
	}

	public void CombatClearShot(Transform attackerTransform, Transform enemyTransform) {
		if (!openCombatClearShot) return;
		combatVirtualCamera.Priority = 11;
		combatComposer.m_ScreenX = .5f + .2f * Mathf.Sign(attackerTransform.position.x - enemyTransform.position.x);
		if (endTime == 0f) TimeManager.HandleRequest(combatTimeEffect);
		endTime = Time.time + .5f;
	}

	private void ExitCombatClearShot() {
		combatVirtualCamera.Priority = 9;
		endTime = 0f;
		combatTimeEffect.lifeRemained = -1f;
	}
}