using UnityEngine;

public class UIManager : MonoSingleton {

	private static WorldSpaceTipController localInteractionTipController;
	private static WorldSpaceTipController remoteInteractionTipController;

	private static WorldSpaceInterlocutionController localInterlocutionController;
	private static WorldSpaceInterlocutionController remoteInterlocutionController;
	
	private void Start () {
		localInteractionTipController = ResourcesManager.GetUI("world_space_tip").GetComponent<WorldSpaceTipController>();
		remoteInteractionTipController = ResourcesManager.GetUI("world_space_tip").GetComponent<WorldSpaceTipController>();
		localInterlocutionController = ResourcesManager.GetUI("world_space_interlocution").GetComponent<WorldSpaceInterlocutionController>();
		remoteInterlocutionController = ResourcesManager.GetUI("world_space_interlocution").GetComponent<WorldSpaceInterlocutionController>();
		localInteractionTipController.gameObject.SetActive(false);
		remoteInteractionTipController.gameObject.SetActive(false);
		localInterlocutionController.gameObject.SetActive(false);
		remoteInterlocutionController.gameObject.SetActive(false);
	}

	public static void ShowInteractionTip(string text, Vector3 position, Vector2 pivot, bool isLocal) {
		ShowInteractionTip(text, position, pivot, isLocal ? localInteractionTipController : remoteInteractionTipController);
	}

	private static void ShowInteractionTip(string text, Vector3 position, Vector2 pivot, WorldSpaceTipController controller) {
		if (controller.IsShowing) controller.Hide();
		controller.Show(text, position, pivot);
	}

	public static void HideInteractionTip(bool isLocal) {
		HideInteractionTip(isLocal ? localInteractionTipController : remoteInteractionTipController);
	}

	private static void HideInteractionTip(WorldSpaceTipController controller) {
		controller.Hide();
	}

	public static void HighlightInteractionTip(Color color, bool isLocal) {
		HighlightInteractionTip(color, isLocal ? localInteractionTipController : remoteInteractionTipController);
	}

	private static void HighlightInteractionTip(Color color, WorldSpaceTipController controller) {
		controller.Highlight(color);
	}

	public static void NormalInteractionTip(bool isLocal) {
		NormalInteractionTip(isLocal ? localInteractionTipController : remoteInteractionTipController);
	}

	private static void NormalInteractionTip(WorldSpaceTipController controller) {
		controller.Normal();
	}

	public static WorldSpaceInterlocutionController GetInterlocutionUI(bool isLocal) {
		return isLocal ? localInterlocutionController : remoteInterlocutionController;
	}
}