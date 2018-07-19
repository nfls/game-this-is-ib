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
		SingletonManager.AddSingleton<InputManager>();
		
		GameObject localPlayer = CharacterFactory.GenerateLocalPlayer();
		localPlayer.AddComponent<InputOperator>();
		localPlayer.GetComponent<CharacterController>().Init();
		virtualCamera.Follow = localPlayer.transform;
		virtualCamera.LookAt = localPlayer.transform;
		GameObject hammer = IBSpriteFactory.GenerateIBSprite("hammer");
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(hammer.GetComponent<IBSpriteController>(), true);

		localPlayer.transform.position = localPlayer.transform.position + new Vector3(2, 0, 0);

		if (bgm) {
			AudioSource source = GetComponent<AudioSource>();
			source.clip = bgm;
			source.Play();
			source.loop = true;
			source.spatialize = false;
		}
	}
}