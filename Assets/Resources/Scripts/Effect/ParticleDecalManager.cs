using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleDecalManager : MonoSingleton {

	public const int CAPACITY = 2000;
	
	private static Transform decalRoot;

	private static int index;
	private static ParticleSystem particleSystem;
	private static readonly ParticleDecalData[] data = new ParticleDecalData[CAPACITY];
	private static readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[CAPACITY];
	
	private void Awake() {
		decalRoot = transform;
		particleSystem = gameObject.AddComponent<ParticleSystem>();
		particleSystem.loop = false;
		particleSystem.playOnAwake = false;
		particleSystem.enableEmission = false;
		particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
		ParticleSystem.ShapeModule shape = particleSystem.shape;
		shape.enabled = false;
		ParticleSystemRenderer renderer = decalRoot.GetComponent<ParticleSystemRenderer>();
		renderer.renderMode = ParticleSystemRenderMode.Mesh;
		renderer.alignment = ParticleSystemRenderSpace.World;
		renderer.enableGPUInstancing = true;
		renderer.castShadows = false;
		renderer.receiveShadows = false;
		renderer.material = Resources.Load<Material>("Materials/ParticleDecalMaterial");
		renderer.mesh = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Quad);
		
		for (int i = 0; i < CAPACITY; i++) data[i] = new ParticleDecalData();
	}

	public static void OnParticleHit(ParticleCollisionEvent @event, float size, Color color) {
		SetParticleData(@event, size, color);
	}

	private static void SetParticleData(ParticleCollisionEvent @event, float size, Color color) {
		if (index >= CAPACITY) index = 0;
		ParticleDecalData data = ParticleDecalManager.data[index];
		Vector3 euler = Quaternion.LookRotation(@event.normal).eulerAngles;
		euler.z = Random.Range(0, 360);
		data.position = @event.intersection;
		data.rotation = euler;
		data.size = size;
		data.color = color;

		index++;                 
	}

	public static void DisplayParticles() {
		for (int i = 0, l = data.Length; i < l; i++) {
			ParticleDecalData data = ParticleDecalManager.data[i];
			particles[i].position = data.position;
			particles[i].rotation3D = data.rotation;
			particles[i].startSize = data.size;
			particles[i].startColor = data.color;
		}
		
		particleSystem.SetParticles(particles, CAPACITY);
	}
}

[Serializable]
public class ParticleDecalData {

	public float size;
	public Vector3 position;
	public Vector3 rotation;
	public Color color;
}