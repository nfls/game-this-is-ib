public class ShaderManager : MonoSingleton {

	public const string PREFIX = "这就是IB/";

	public const string OUTLINED_TOON_SHADER_NAME = PREFIX + "OutlinedToonShader";
	public const string TOON_SHADER_NAME = PREFIX + "ToonShader";
	public const string SINGLE_COLOR_SHADER_NAME = PREFIX + "SingleColorShader";

	public const string TOON_COLOR_KEYWORD = "_ToonColor";
	public const string BRIGHTNESS_KEYWORD = "_Brightness";
	public const string OUTLINE_COLOR_KEYWORD = "_OutlineColor";
	public const string OUTLINE_WIDTH_KEYWORD = "_OutlineWidth";
	public const string OUTLINE_DISTANCE_KEYWORD = "_OutlineDistance";
	public const string TOON_LEVEL_KEYWORD = "_ToonLevel";
	public const string TOON_STEPS_KEYWORD = "_ToonSteps";
	public const string DISSOLVE_COLOR_KEYWORD = "_DissolveColor";
	public const string DISSOLVE_EDGE_COLOR_KEYWORD = "_DissolveEdgeColor";
	public const string DISSOLVE_THRESHOLD_KEYWORD = "_DissolveThreshold";
}