using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RenderTexture))]
public class DialogPortraitController : MonoBehaviour {

	public Camera camera;

	public Color activeColor = Color.clear;
	public Color inactiveColor = Color.gray;
	
	private RawImage _portrait;

	private void Awake() {
		_portrait = GetComponent<RawImage>();
		
		RenderTexture texture = RenderTexture.GetTemporary(512, 512, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		_portrait.texture = texture;

		camera.targetTexture = texture;
		camera.Render();
		camera.gameObject.SetActive(false);
		
		// Deactivate();
	}
	
	public void Refresh(GameObject go) {
		
	}

	public void Activate() {
		_portrait.color = activeColor;
	}

	public void Deactivate() {
		_portrait.color = inactiveColor;
	}
}