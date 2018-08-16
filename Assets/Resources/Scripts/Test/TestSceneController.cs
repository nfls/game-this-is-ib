using Cinemachine;
using UnityEngine;

public class TestSceneController : MonoBehaviour {

	public CinemachineVirtualCamera virtualCamera;
	public AudioClip bgm;

	private void Start() {
		LocalDataManager.Init();
		ResourcesManager.Init();
		PlayerManager.Init();

		MacXboxControllerRumbleUtil.Init();
		SingletonManager.AddSingleton<ShaderManager>();
		SingletonManager.AddSingleton<DispatchSystem>();
		SingletonManager.AddSingleton<CameraManager>();
		SingletonManager.AddSingleton<TimeManager>();
		SingletonManager.AddSingleton<ParticleManager>();
		SingletonManager.AddSingleton<ParticleDecalManager>();
		SingletonManager.AddSingleton<ProjectileManager>();
		SingletonManager.AddSingleton<AudioManager>();
		SingletonManager.AddSingleton<InputManager>();
		SingletonManager.AddSingleton<UIManager>();

		GameObject localPlayer = CharacterFactory.GenerateLocalPlayer();
		GameObject interactionSystem = new GameObject("Interaction System");
		interactionSystem.transform.localScale = new Vector3(6f, 3f, 4f);
		interactionSystem.layer = LayerManager.InteractionLayer;
		localPlayer.AddComponent<InputOperator>().InteractionSystem = interactionSystem.AddComponent<InteractionSystem>();
		virtualCamera.Follow = localPlayer.transform;
		virtualCamera.LookAt = localPlayer.transform;
		GameObject hammer = IBSpriteFactory.GenerateIBSprite("complex_shooter");
		GameObject shooter = IBSpriteFactory.GenerateIBSprite("shooter");
		GameObject complexShooter = IBSpriteFactory.GenerateIBSprite("hammer");
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(hammer.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(shooter.GetComponent<IBSpriteController>());
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(complexShooter.GetComponent<IBSpriteController>());
		localPlayer.transform.position = new Vector3(3, 1f, 0);
		GameObject enemy1 = CharacterFactory.GenerateLocalPlayer();
		TagManager.SetAllTags(enemy1, TagManager.ENEMY_TAG);
		enemy1.name = "Enemy 1";
		enemy1.transform.position = new Vector3(10, 8, 0);
		GameObject enemy2 = CharacterFactory.GenerateLocalPlayer();
		TagManager.SetAllTags(enemy2, TagManager.ENEMY_TAG);
		enemy2.name = "Enemy 2";
		enemy2.transform.position = new Vector3(12, 8, 0);

		if (bgm) {
			AudioSource source = GetComponent<AudioSource>();
			source.clip = bgm;
			source.Play();
			source.loop = true;
			source.spatialize = false;
		}
	}
}