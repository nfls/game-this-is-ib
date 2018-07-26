using Cinemachine;
using UnityEngine;

public class TestSceneController : MonoBehaviour {

	public CinemachineVirtualCamera virtualCamera;
	public AudioClip bgm;

	private void Start() {
		LocalDataManager.Init();
		ResourcesManager.Init();
		PlayerManager.Init();

		SingletonManager.AddSingleton<ShaderManager>();
		SingletonManager.AddSingleton<DispatchSystem>();
		SingletonManager.AddSingleton<CameraManager>();
		SingletonManager.AddSingleton<TimeManager>();
		SingletonManager.AddSingleton<ParticlePool>();
		SingletonManager.AddSingleton<AudioManager>();
		SingletonManager.AddSingleton<InputManager>();

		GameObject localPlayer = CharacterFactory.GenerateLocalPlayer();
		localPlayer.AddComponent<InputOperator>();
		localPlayer.GetComponent<CharacterController>().Init();
		virtualCamera.Follow = localPlayer.transform;
		virtualCamera.LookAt = localPlayer.transform;
		GameObject hammer = IBSpriteFactory.GenerateIBSprite("hammer");
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(hammer.GetComponent<IBSpriteController>(), true);
		localPlayer.transform.position = new Vector3(3, 8.5f, 0);
		GameObject enemy = CharacterFactory.GenerateLocalPlayer();
		TagManager.SetAllTags(enemy, TagManager.ENEMY_TAG);
		enemy.name = "Enemy";
		enemy.GetComponent<CharacterController>().Init();
		enemy.transform.position = new Vector3(12, 8, 0);

		if (bgm) {
			AudioSource source = GetComponent<AudioSource>();
			source.clip = bgm;
			source.Play();
			source.loop = true;
			source.spatialize = false;
		}
	}
}