using UnityEngine;

public class UIManager : MonoSingleton {
	public static WorldSpaceInterlocutionController LocalInterlocutionController => localInterlocutionController;

	private static WorldSpaceTipController localInteractionTipController;
	private static WorldSpaceInterlocutionController localInterlocutionController;
	
	private void Start () {
		localInteractionTipController = ResourcesManager.GetUI("world_space_tip").GetComponent<WorldSpaceTipController>();
		localInterlocutionController = ResourcesManager.GetUI("world_space_interlocution").GetComponent<WorldSpaceInterlocutionController>();
		localInteractionTipController.gameObject.SetActive(false);
		localInterlocutionController.gameObject.SetActive(true);
		localInterlocutionController.gameObject.SetActive(false);
	}

	public static void ShowInteractionTip(string text, Vector3 position, Vector2 pivot) {
		if (localInteractionTipController.IsShowing) localInteractionTipController.Hide();
		localInteractionTipController.Show(text, position, pivot);
	}

	public static void HideInteractionTip() {
		localInteractionTipController.Hide();
	}
	
	public static void HighlightInteractionTip(Color color) {
		localInteractionTipController.Highlight(color);
	}

	public static void NormalInteractionTip() {
		localInteractionTipController.Normal();
	}
}