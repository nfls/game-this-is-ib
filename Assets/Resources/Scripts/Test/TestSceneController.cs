using Cinemachine;
using UnityEditor;
using UnityEngine;

public class TestSceneController : MonoBehaviour {

	public CinemachineVirtualCamera virtualCamera;

	private void Start() {
		LocalDataManager.Init();
		ResourcesManager.Init();
		PlayerManager.Init();
		SingletonManager.AddSingleton<InputManager>();
		GameObject localPlayer = CharacterFactory.GenerateLocalPlayer();
		localPlayer.AddComponent<InputOperator>();
		localPlayer.GetComponent<CharacterController>().Init();
		virtualCamera.Follow = localPlayer.transform;
		virtualCamera.LookAt = localPlayer.transform;
		GameObject hammer = IBSpriteFactory.GenerateIBSprite("hammer");
		localPlayer.GetComponent<CharacterController>().EquipIBSprite(hammer.GetComponent<IBSpriteController>(), true);
	}
}