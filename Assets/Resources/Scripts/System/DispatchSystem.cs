using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DispatchSystem : MonoSingleton {
	
	private static Dictionary<string, Action> globalDispatchesNoArgs = new Dictionary<string, Action>(3);
	private static Dictionary<string, Action<DispatchArgs>> globalDispatchesWithArgs = new Dictionary<string, Action<DispatchArgs>>(3);
	
	private static Dictionary<string, Dictionary<string, Action>> localDispatchesNoArgs = new Dictionary<string, Dictionary<string, Action>>(2);
	private static Dictionary<string, Dictionary<string, Action<DispatchArgs>>> localDispatchesWithArgs = new Dictionary<string, Dictionary<string, Action<DispatchArgs>>>(2);
	
	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		localDispatchesNoArgs.Add(scene.name, new Dictionary<string, Action>(3));
		localDispatchesWithArgs.Add(scene.name, new Dictionary<string, Action<DispatchArgs>>(3));
	}

	private void OnSceneUnloaded(Scene scene) {
		localDispatchesNoArgs.Remove(scene.name);
		localDispatchesWithArgs.Remove(scene.name);
		GC.Collect();
	}

	public static void RegisterGlobalDispatch(string name, Action dispatch) {
		if (globalDispatchesNoArgs.ContainsKey(name)) {
			new UnityException("Global Dispatch (No Args) [" + name + "] Has Already Existed !");
			return;
		}

		globalDispatchesNoArgs[name] = dispatch;
	}
	
	public static void RegisterGlobalDispatch(string name, Action<DispatchArgs> dispatch) {
		if (globalDispatchesWithArgs.ContainsKey(name)) {
			new UnityException("Global Dispatch (With Args) [" + name + "] Has Already Existed !");
			return;
		}

		globalDispatchesWithArgs[name] = dispatch;
	}

	public static void RegisterLocalDispatch(string scene, string name, Action dispatch) {
		if (!localDispatchesNoArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (localDispatchesNoArgs[scene].ContainsKey(name)) {
			new UnityException("Local Dispatch (No Args) [" + name + "] Has Already Existed In Scene [" + scene + "] !");
			return;
		}

		localDispatchesNoArgs[scene][name] = dispatch;
	}
	
	public static void RegisterLocalDispatch(Scene scene, string name, Action dispatch) {
		RegisterLocalDispatch(scene.name, name, dispatch);
	}

	public static void RegisterLocalDispatch(string name, Action dispatch) {
		RegisterLocalDispatch(SceneManager.GetActiveScene(), name, dispatch);
	}

	public static void RegisterLocalDispatch(string scene, string name, Action<DispatchArgs> dispatch) {
		if (!localDispatchesWithArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (localDispatchesWithArgs[scene].ContainsKey(name)) {
			new UnityException("Local Dispatch (With Args) [" + name + "] Has Already Existed In Scene [" + scene + "] !");
			return;
		}

		localDispatchesWithArgs[scene][name] = dispatch;
	}
	
	public static void RegisterLocalDispatch(Scene scene, string name, Action<DispatchArgs> dispatch) {
		RegisterLocalDispatch(scene.name, name, dispatch);
	}
	
	public static void RegisterLocalDispatch(string name, Action<DispatchArgs> dispatch) {
		RegisterLocalDispatch(SceneManager.GetActiveScene(), name, dispatch);
	}

	public static void RegisterDispatch(string name, Action dispatch) {
		RegisterGlobalDispatch(name, dispatch);
	}
	
	public static void RegisterlDispatch(string name, Action<DispatchArgs> dispatch) {
		RegisterGlobalDispatch(name, dispatch);
	}
	
	public static void RegisterDispatch(string scene, string name, Action dispatch) {
		RegisterLocalDispatch(scene, name, dispatch);
	}
	
	public static void RegisterDispatch(string scene, string name, Action<DispatchArgs> dispatch) {
		RegisterLocalDispatch(scene, name, dispatch);
	}
	
	public static void RegisterDispatch(Scene scene, string name, Action dispatch) {
		RegisterLocalDispatch(scene.name, name, dispatch);
	}
	
	public static void RegisterDispatch(Scene scene, string name, Action<DispatchArgs> dispatch) {
		RegisterLocalDispatch(scene.name, name, dispatch);
	}

	public static void DeregisterGlobalDispatchNoArgs(string name) {
		if (!globalDispatchesNoArgs.ContainsKey(name)) {
			new UnityException("Cannot Find Global Disptach (No Args) [" + name + "] !");
			return;
		}

		globalDispatchesNoArgs.Remove(name);
	}

	public static void DeregisterGlobalDispatchWithArgs(string name) {
		if (!globalDispatchesWithArgs.ContainsKey(name)) {
			new UnityException("Cannot Find Global Disptach (With Args) [" + name + "] !");
			return;
		}

		globalDispatchesWithArgs.Remove(name);
	}

	public static void DeregisterGlobalDispatch(string name, bool withArgs = false) {
		if (withArgs) {
			DeregisterGlobalDispatchWithArgs(name);
		} else {
			DeregisterGlobalDispatchNoArgs(name);
		}
	}

	public static void DeregisterLocalDispatchNoArgs(string scene, string name) {
		if (!localDispatchesNoArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (!localDispatchesNoArgs[scene].ContainsKey(name)) {
			new UnityException("Cannot Find Local Dispatch (No Args) [" + name + "] In Scene [" + scene + "] !");
			return;
		}

		localDispatchesNoArgs[scene].Remove(name);
	}
	
	public static void DeregisterLocalDispatchNoArgs(Scene scene, string name) {
		DeregisterLocalDispatchNoArgs(scene.name, name);
	}
	
	public static void DeregisterLocalDispatchNoArgs(string name) {
		DeregisterLocalDispatchNoArgs(SceneManager.GetActiveScene(), name);
	}
	
	public static void DeregisterLocalDispatchWithArgs(string scene, string name) {
		if (!localDispatchesWithArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (!localDispatchesWithArgs[scene].ContainsKey(name)) {
			new UnityException("Cannot Find Local Dispatch (With Args) [" + name + "] In Scene [" + scene + "] !");
			return;
		}

		localDispatchesWithArgs[scene].Remove(name);
	}
	
	public static void DeregisterLocalDispatchWithArgs(Scene scene, string name) {
		DeregisterLocalDispatchWithArgs(scene.name, name);
	}
	
	public static void DeregisterLocalDispatchWithArgs(string name) {
		DeregisterLocalDispatchWithArgs(SceneManager.GetActiveScene(), name);
	}

	public static void DeregisterLocalDispatch(string scene, string name, bool withArgs = false) {
		if (withArgs) {
			DeregisterDispatchWithArgs(scene, name);
		} else {
			DeregisterDispatchNoArgs(scene, name);
		}
	}

	public static void DeregisterLocalDispatch(Scene scene, string name, bool withArgs = false) {
		DeregisterLocalDispatch(scene.name, name, withArgs);
	}
	
	public static void DeregisterLocalDispatch(string name, bool withArgs = false) {
		DeregisterLocalDispatch(SceneManager.GetActiveScene(), name, withArgs);
	}

	public static void DeregisterDispatchNoArgs(string name) {
		DeregisterGlobalDispatchNoArgs(name);
	}
	
	public static void DeregisterDispatchNoArgs(string scene, string name) {
		DeregisterLocalDispatchNoArgs(scene, name);
	}
	
	public static void DeregisterDispatchNoArgs(Scene scene, string name) {
		DeregisterLocalDispatchNoArgs(scene, name);
	}

	public static void DeregisterDispatchWithArgs(string name) {
		DeregisterGlobalDispatchWithArgs(name);
	}
	
	public static void DeregisterDispatchWithArgs(string scene, string name) {
		DeregisterLocalDispatchWithArgs(scene, name);
	}
	
	public static void DeregisterDispatchWithArgs(Scene scene, string name) {
		DeregisterLocalDispatchWithArgs(scene, name);
	}

	public static void DeregisterDispatch(string name, bool withArgs = false) {
		DeregisterGlobalDispatch(name, withArgs);
	}

	public static void DeregisterDispatch(string scene, string name, bool withArgs = false) {
		DeregisterDispatch(scene, name, withArgs);
	}

	public static void DeregisterDispatch(Scene scene, string name, bool withArgs = false) {
		DeregisterDispatch(scene.name, name, withArgs);
	}

	public static void ExeDispatch(Dispatch dispatch) {
		if (dispatch.scene == null) {
			ExeGlobalDispatch(dispatch);
		} else {
			ExeLocalDispatch(dispatch);
		}
	}

	public static void ExeGlobalDispatch(Dispatch dispatch) {
		if (dispatch.args == null) {
			DispatchGlobally(dispatch.name);
		} else {
			DispatchGlobally(dispatch.name, dispatch.args);
		}
	}

	public static void ExeLocalDispatch(Dispatch dispatch) {
		if (dispatch.args == null) {
			DispatchLocally(dispatch.scene, dispatch.name);
		} else {
			DispatchLocally(dispatch.scene, dispatch.name, dispatch.args);
		}
	}
	
	public static void DispatchGlobally(string name) {
		if (!globalDispatchesNoArgs.ContainsKey(name)) {
			new UnityException("Cannot Find Global Disptach (No Args) [" + name + "] !");
			return;
		}

		globalDispatchesNoArgs[name]();
	}
	
	public static void DispatchGlobally(string name, DispatchArgs dispatchArgs) {
		if (!globalDispatchesWithArgs.ContainsKey(name)) {
			new UnityException("Cannot Find Global Disptach (With Args) [" + name + "] !");
			return;
		}

		globalDispatchesWithArgs[name](dispatchArgs);
	}

	public static void DispatchLocally(string scene, string name) {
		if (!localDispatchesNoArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (!localDispatchesNoArgs[scene].ContainsKey(name)) {
			new UnityException("Cannot Find Local Dispatch (No Args) [" + name + "] In Scene [" + scene + "] !");
			return;
		}

		localDispatchesNoArgs[scene][name]();
	}
	
	public static void DispatchLocally(Scene scene, string name) {
		DispatchLocally(scene.name, name);
	}

	public static void DispatchLocally(string name) {
		DispatchLocally(SceneManager.GetActiveScene(), name);
	}

	public static void DispatchLocally(string scene, string name, DispatchArgs dispatchArgs) {
		if (!localDispatchesWithArgs.ContainsKey(scene)) {
			new UnityException("Cannot Find Scene [" + scene + "] !");
			return;
		}

		if (!localDispatchesWithArgs[scene].ContainsKey(name)) {
			new UnityException("Cannot Find Local Dispatch (With Args) [" + name + "] In Scene [" + scene + "] !");
			return;
		}

		localDispatchesWithArgs[scene][name](dispatchArgs);
	}
	
	public static void DispatchLocally(Scene scene, string name, DispatchArgs dispatchArgs) {
		DispatchLocally(scene.name, name, dispatchArgs);
	}

	public static void DispatchLocally(string name, DispatchArgs dispatchArgs) {
		DispatchLocally(SceneManager.GetActiveScene(), name, dispatchArgs);
	}

	public static void Dispatch(string name) {
		DispatchGlobally(name);
	}

	public static void Dispatch(string name, DispatchArgs dispatchArgs) {
		DispatchGlobally(name, dispatchArgs);
	}
	
	public static void Dispatch(string scene, string name) {
		DispatchLocally(scene, name);
	}
	
	public static void Dispatch(Scene scene, string name) {
		DispatchLocally(scene, name);
	}

	public static void Dispatch(string scene, string name, DispatchArgs dispatchArgs) {
     	DispatchLocally(scene, name, dispatchArgs);
    }
	
	public static void Dispatch(Scene scene, string name, DispatchArgs dispatchArgs) {
		DispatchLocally(scene, name, dispatchArgs);
	}
}

public class Dispatch {

	public string scene;
	public string name;
	public DispatchArgs args;

	public Dispatch(string name) {
		this.name = name;
	}
	
	public Dispatch(string name, DispatchArgs args) {
		this.name = name;
		this.args = args;
	}
	
	public Dispatch(string scene, string name) {
		this.scene = scene;
		this.name = name;
	}
	
	public Dispatch(string scene, string name, DispatchArgs args) {
		this.scene = scene;
		this.name = name;
		this.args = args;
	}
	
	public Dispatch(Scene scene, string name) {
		this.scene = scene.name;
		this.name = name;
	}
	
	public Dispatch(Scene scene, string name, DispatchArgs args) {
		this.scene = scene.name;
		this.name = name;
		this.args = args;
	}

	public void Exe() {
		DispatchSystem.ExeDispatch(this);
	}
}

public class DispatchArgs : Dictionary<string, object> {

	public DispatchArgs() : base(1) { }

	public DispatchArgs(int capacity) : base(capacity) { }
}
