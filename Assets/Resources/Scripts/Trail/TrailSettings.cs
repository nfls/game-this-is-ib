using System;
using UnityEngine;

[Serializable]
public class TrailSettings {

	public MotionVectorGenerationMode motionVectorMode;
	public Color color;
	public float brightness;
	public float lifespan;
	public float minDistance;
	public AnimationCurve width;
	public int cornerVertices;
	public int endCapVertices;
	public LineAlignment alignment;
	public Material material;

	private MaterialPropertyBlock _props;

	public void InitRenderer(ref TrailRenderer renderer) {
		if (material == null) {
			material = MaterialManager.GetMaterial(ShaderManager.SINGLE_COLOR_SHADER_NAME);
			renderer.material = material;
		}
		
		if (_props == null) _props = new MaterialPropertyBlock();
		
		_props.SetColor(ShaderManager.TOON_COLOR_KEYWORD, color);
		_props.SetFloat(ShaderManager.BRIGHTNESS_KEYWORD, brightness);
		
		renderer.SetPropertyBlock(_props);
		
		renderer.motionVectorGenerationMode = motionVectorMode;
		renderer.time = lifespan;
		renderer.minVertexDistance = minDistance;
		renderer.widthCurve = width;
		renderer.numCornerVertices = cornerVertices;
		renderer.numCapVertices = endCapVertices;
		renderer.alignment = alignment;
	}
}