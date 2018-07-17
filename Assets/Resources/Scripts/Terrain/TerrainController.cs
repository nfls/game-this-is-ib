using UnityEngine;

public class TerrainController : MonoBehaviour {

	public Color Color {
		get { return _color; }
		set {
			_color = value;
			_material.SetColor(MaterialManager.TOON_COLOR_KEYWORD, _color);
		}
	}

	public Color OutlineColor {
		get { return _outlineColor; }
		set {
			_outlineColor = value;
			_material.SetColor(MaterialManager.OUTLINE_COLOR_KEYWORD, _outlineColor);
		}
	}

	[SerializeField] protected Color _color = new Color(0, 1f, 1f, 1f);
	[SerializeField] protected Color _outlineColor = new Color(.2f, .2f, .2f, 1f);
	protected Material _material;

	private void Start() {
		_material = GetComponent<Renderer>().material;
		SetColors(_color, _outlineColor);
	}

	public void SetColors(Color color, Color outlineColor) {
		Color = color;
		OutlineColor = outlineColor;
	}
}