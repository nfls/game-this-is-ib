using UnityEngine;

public class WaterEffectController : MonoBehaviour {

    public Vector3[] vertices;
    public Mesh mesh;
    public Material material;
    public float mytime;
    public float waveFrequency1 = 0.3f;
    public float waveFrequency2 = 0.5f;
    public float waveFrequency3 = 0.9f;
    public float waveFrequency4 = 1.5f;
    public float waveAmplitude1 = 1f;
    public float waveAmplitude2 = 1f;
    public float waveAmplitude3 = 1f;
    public float waveAmplitude4 = 1f;
    public float speed = 3;
    public int index1 = 40;
    public int index2 = 80;
    public int index3 = 120;
    private readonly Vector3 _vZero = Vector3.zero;
    private Vector2 _uvOffset = Vector2.zero;
    private Vector2 _uvDirection = new Vector2(0.5f, 0.5f);

    private void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        // material = GetComponent<Renderer>().material;
        vertices = mesh.vertices;
    }

    private void Update() {
        mytime += Time.deltaTime * speed;
        for (int i = 0; i < vertices.Length; i++) vertices[i] = new Vector3(vertices[i].x, FindHeight(i), vertices[i].z);
        mesh.vertices = vertices;
        // _uvOffset += _uvDirection * Time.deltaTime * 0.1f;
        // material.SetTextureOffset("_NormalTex", _uvOffset);
        mesh.RecalculateNormals();
    }

    private float FindHeight(int i) {
        float distance1 = Vector2.Distance(new Vector2(vertices[i].x, vertices[i].z), _vZero);
        float distance2 = Vector2.Distance(new Vector2(vertices[i].x, vertices[i].z), new Vector2(vertices[index1].x, vertices[index1].z));
        float distance3 = Vector2.Distance(new Vector2(vertices[i].x, vertices[i].z), new Vector2(vertices[index2].x, vertices[index2].z));
        float distance4 = Vector2.Distance(new Vector2(vertices[i].x, vertices[i].z), new Vector2(vertices[index3].x, vertices[index3].z));
        float H = 0f;
        H += Mathf.Sin(distance1 * waveFrequency1 * Mathf.PI + mytime) * waveAmplitude1;
        H += Mathf.Sin(distance2 * waveFrequency2 * Mathf.PI + mytime) * waveAmplitude2;
        H += Mathf.Sin(distance3 * waveFrequency3 * Mathf.PI + mytime) * waveAmplitude3;
        H += Mathf.Sin(distance4 * waveFrequency4 * Mathf.PI + mytime) * waveAmplitude4;
        return H;
    }
}