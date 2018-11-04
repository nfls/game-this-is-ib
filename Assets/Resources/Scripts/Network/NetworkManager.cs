using UnityEngine;

public static class NetworkManager {

	public static bool IsOffline => offlineMode || !ReachesInternet;
	public static bool ReachesInternet => Application.internetReachability != NetworkReachability.NotReachable;
	public static bool UsesWifi => Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
	public static bool UsesData => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
	public static bool offlineMode;
	
	
}