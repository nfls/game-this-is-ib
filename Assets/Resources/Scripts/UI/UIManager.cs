using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton {

	private static WorldSpaceTipController interactionTipController;
	
	private void Start () {
		interactionTipController = null;
	}

	private static void InternalShowInteractionTip(Sprite icon, string text) {
		interactionTipController.icon = icon;
		interactionTipController.text = text;
		
		interactionTipController.Show();
	}

	public static void ShowInteractionTip(Sprite icon, string text) {
		if (interactionTipController.IsShowing) {
			interactionTipController.Hide();
		}

		InternalShowInteractionTip(icon, text);
	}

	public static void HideInteractionTip(InteractionController interactionController) {
		interactionTipController.Hide();
	}
}