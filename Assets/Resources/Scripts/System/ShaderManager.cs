using UnityEngine;

public class ShaderManager : MonoSingleton {

	public const string PREFIX = "这就是IB/";

	public const string OUTLINED_TOON_SHADER_NAME = PREFIX + "OutlinedToonShader";
	public const string TOON_SHADER_NAME = PREFIX + "ToonShader";
	public const string SINGLE_COLOR_SHADER_NAME = PREFIX + "SingleColorShader";

	#region Shader Keywords
	
	public static readonly int MAIN_TEXTURE_KEYWORD = Shader.PropertyToID("_MainTex");
	public static readonly int NOISE_TEXTURE_KEYWORD = Shader.PropertyToID("_NoiseTex");
	public static readonly int COLOR_KEYWORD = Shader.PropertyToID("_Color");
	public static readonly int DISTANCE_KEYWORD = Shader.PropertyToID("_Distance");
	public static readonly int WIDTH_KEYWORD = Shader.PropertyToID("_Width");
	public static readonly int FRUSTUM_CORNERS_WS_KEYWORD = Shader.PropertyToID("_FrustumCornersWS");
	public static readonly int CENTER_WORLD_SPACE_POSITION_KEYWORD = Shader.PropertyToID("_CenterWorldSpacePos");
	public static readonly int TOON_COLOR_KEYWORD = Shader.PropertyToID("_ToonColor");
	public static readonly int THICKNESS_KEYWORD = Shader.PropertyToID("_Thickness");
	public static readonly int BRIGHTNESS_KEYWORD = Shader.PropertyToID("_Brightness");
	public static readonly int OUTLINE_COLOR_KEYWORD = Shader.PropertyToID("_OutlineColor");
	public static readonly int OUTLINE_WIDTH_KEYWORD = Shader.PropertyToID("_OutlineWidth");
	public static readonly int OUTLINE_DISTANCE_KEYWORD = Shader.PropertyToID("_OutlineDistance");
	public static readonly int TOON_LEVEL_KEYWORD = Shader.PropertyToID("_ToonLevel");
	public static readonly int TOON_STEPS_KEYWORD = Shader.PropertyToID("_ToonSteps");
	public static readonly int THRESHOLD_KEYWORD = Shader.PropertyToID("_Threshold");
	public static readonly int MAX_DISTANCE_KEYWORD = Shader.PropertyToID("_MaxDistance");
	public static readonly int START_POINT_KEYWORD = Shader.PropertyToID("_StartPoint");
	public static readonly int EDGE_LENGTH_KEYWORD = Shader.PropertyToID("_EdgeLength");
	public static readonly int BLUR_FACTOR_KEYWORD = Shader.PropertyToID("_BlurFactor");
	public static readonly int BLUR_CENTER_KEYWORD = Shader.PropertyToID("_BlurCenter");
	public static readonly int BLUR_DISTANCE_KEYWORD = Shader.PropertyToID("_BlurDistance");
	public static readonly int DISSOLVE_COLOR_KEYWORD = Shader.PropertyToID("_DissolveColor");
	public static readonly int DISSOLVE_EDGE_COLOR_KEYWORD = Shader.PropertyToID("_DissolveEdgeColor");
	public static readonly int DISSOLVE_THRESHOLD_KEYWORD = Shader.PropertyToID("_DissolveThreshold");
	public static readonly int DISTORTION_POWER_KEYWORD = Shader.PropertyToID("_DistortionPower");
	public static readonly int RIMLIGHT_COLOR_KEYWORD = Shader.PropertyToID("_RimlightColor");
	public static readonly int RIMLIGHT_POWER_KEYWORD = Shader.PropertyToID("_RimlightPower");
	public static readonly int INTERSECTION_POWER_KEYWORD = Shader.PropertyToID("_IntersectionPower");
	public static readonly int HITS_KEYWORD = Shader.PropertyToID("_Hits");
	public static readonly int HIT_AREA_ALPHAS_KEYWORD = Shader.PropertyToID("_HitAreaAlphas");
	
	#endregion
}