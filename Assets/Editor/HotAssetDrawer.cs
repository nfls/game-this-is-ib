using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HotAsset<>))]
[CustomPropertyDrawer(typeof(ItemAsset))]
[CustomPropertyDrawer(typeof(IBSpriteAsset))]
[CustomPropertyDrawer(typeof(ProjectileAsset))]
[CustomPropertyDrawer(typeof(ParticleAsset))]
[CustomPropertyDrawer(typeof(BodyAsset))]
[CustomPropertyDrawer(typeof(EyeAsset))]
[CustomPropertyDrawer(typeof(EnemyAsset))]
[CustomPropertyDrawer(typeof(BossAsset))]
[CustomPropertyDrawer(typeof(UIAsset))]
[CustomPropertyDrawer(typeof(SpriteAsset))]
[CustomPropertyDrawer(typeof(AudioAsset))]
public class HotAssetDrawer : PropertyDrawer {

	public const float LABEL_WIDTH = 60f;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		DrawGUI(position, property, label);
	}

	public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label) {
		SerializedProperty required = property.FindPropertyRelative("required");
		EditorGUI.PropertyField(new Rect(position.position, new Vector2(position.width / 2, position.height)), required, label);
		float labelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = LABEL_WIDTH;
		if (required.boolValue) EditorGUI.PropertyField(new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height), property.FindPropertyRelative("identifier"));
		EditorGUIUtility.labelWidth = LABEL_WIDTH;
	}
}