using UnityEngine;

public class SingletonManager : MonoBehaviour {

	private static Transform singletonRoot;

	static SingletonManager() {
		GameObject root = new GameObject("Singleton Root");
		DontDestroyOnLoad(root);
		singletonRoot = root.transform;
		singletonRoot.gameObject.AddComponent<SingletonManager>();
	}

	public static T GetSingleton<T>() where T : MonoSingleton {
		return singletonRoot.GetComponent<T>();
	}

	public static T AddSingleton<T>() where T : MonoSingleton {
		return singletonRoot.GetComponent<T>() ? null : singletonRoot.gameObject.AddComponent<T>();
	}

	public static void RemoveSingleton<T>() where T : MonoSingleton {
		T t = singletonRoot.GetComponent<T>();
		if (t) {
			Destroy(t);
		}
	}
}
