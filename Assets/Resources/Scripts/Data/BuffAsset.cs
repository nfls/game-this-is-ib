using UnityEngine;

public class BuffAsset : ScriptableObject {

	public string name;

	public int inactivatedReductionRate;
	public int activatedReductionRate;
	public int activationAccumulationLimit;

	private int _accumulation;
	private bool _isActivated;
	
}