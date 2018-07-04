using UnityEngine;

public static class NetworkManager {

	public static bool IsOffline {
		get { return offlineMode || !ReachesInternet; }
	}

	public static bool ReachesInternet {
		get { return Application.internetReachability != NetworkReachability.NotReachable; }
	}

	public static bool UsesWifi {
		get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
	}

	public static bool UsesData {
		get { return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork; }
	}

	public static bool offlineMode;

	static NetworkManager() {
		
	}
}