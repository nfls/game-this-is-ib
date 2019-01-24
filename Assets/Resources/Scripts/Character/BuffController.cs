using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthController))]
public class BuffController : MonoBehaviour {

	public int enabledBuffs;
	public int updatingBuffs;
	public Buff[] buffs = new Buff[32];
	
	
}