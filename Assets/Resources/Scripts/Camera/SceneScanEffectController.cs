using UnityEngine;

public class SceneScanEffectController : MonoBehaviour {

	[Range(0f, 40f)]
	public float distance;
	[Range(0f, 40f)]
	public float width;
	public Color color;
	public Material material;
	public Transform center;

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (material) {
			Camera camera = CameraManager.MainCamera;
			Transform transform = camera.transform;
			
			float fovWHalfTan = Mathf.Tan(camera.fieldOfView * .5f * Mathf.Deg2Rad);
			float near = camera.nearClipPlane;
			float nearFovWHalfTan = fovWHalfTan * near;
			Vector3 nearForward = transform.forward * near;
			Vector3 toRight = transform.right * nearFovWHalfTan * camera.aspect;
			Vector3 toTop = transform.up * nearFovWHalfTan;
			Vector3 nearForwardAddToRight = nearForward + toRight;
			Vector3 nearForwardSubToRight = nearForward - toRight;
			
			Vector3 topLeft = nearForwardSubToRight + toTop;
			float scale = topLeft.magnitude * camera.farClipPlane / near;
			topLeft = topLeft.normalized * scale;

			Vector3 topRight = (nearForwardAddToRight + toTop).normalized * scale;
			Vector3 bottomRight = (nearForwardAddToRight - toTop).normalized * scale;
			Vector3 bottomLeft = (nearForwardSubToRight - toTop).normalized * scale;
			
			Matrix4x4 frustumCorners = Matrix4x4.identity;
			frustumCorners.SetRow(0, topLeft);
			frustumCorners.SetRow(1, topRight);
			frustumCorners.SetRow(2, bottomRight);
			frustumCorners.SetRow(3, bottomLeft);

			Vector3 centerPos;
			if (center != null) centerPos = center.position;
			else centerPos = Vector3.zero;
			
			material.SetMatrix(ShaderManager.FRUSTUM_CORNERS_WS_KEYWORD, frustumCorners);
			material.SetColor(ShaderManager.COLOR_KEYWORD, color);
			material.SetFloat(ShaderManager.DISTANCE_KEYWORD, distance);
			material.SetFloat(ShaderManager.WIDTH_KEYWORD, width);
			material.SetVector(ShaderManager.CENTER_WORLD_SPACE_POSITION_KEYWORD, centerPos);
			Graphics.Blit(src, dest, material);
		} else Graphics.Blit(src, dest);
	}
}