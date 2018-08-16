using UnityEngine;

public class UIManager : MonoSingleton {

	private static WorldSpaceTipController interactionTipController;
	
	private void Start () {
		interactionTipController = ResourcesManager.GetUI("world_space_tip").GetComponent<WorldSpaceTipController>();
	}

	public static void ShowInteractionTip(string text, Vector3 position, Vector2 pivot) {
		if (interactionTipController.IsShowing) {
			interactionTipController.Hide();
		}
		
		interactionTipController.Show(text, position, pivot);
	}

	public static void HideInteractionTip() {
		interactionTipController.Hide();
	}
}