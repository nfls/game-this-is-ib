using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoSingleton {

	private static Transform projectileRoot;
	private static readonly Dictionary<string, Queue<ProjectileController>> idleProjectiles = new Dictionary<string, Queue<ProjectileController>>(3);

	private void Start() {
		GameObject go = new GameObject("Projectile Root");
		DontDestroyOnLoad(go);
		projectileRoot = go.transform;
	}
	
	public static ProjectileController Get(string name) {
		Queue<ProjectileController> projectiles;
		if (!idleProjectiles.ContainsKey(name)) {
			projectiles = new Queue<ProjectileController>(5);
			idleProjectiles[name] = projectiles;
		} else {
			projectiles = idleProjectiles[name];
		}

		ProjectileController projectile;
		if (projectiles.Count > 0) {
			projectile = projectiles.Dequeue();
			projectile.gameObject.SetActive(true);
		} else {
			projectile = ResourcesManager.GetProjectile(name).GetComponent<ProjectileController>();
			projectile.transform.parent = projectileRoot;
		}
		
		return projectile;
	}

	public static T Get<T>(string name) where T : ProjectileController {
		return Get(name) as T;
	}
	
	public static void Recycle(ProjectileController projectile) {
		projectile.Recycle();
		projectile.gameObject.SetActive(false);
		idleProjectiles[projectile.identifierName].Enqueue(projectile);
	}
}