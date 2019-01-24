using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour {

	[Header("[Settings]")]
	public int healthMax;
	public float recoverDelay;
	public float recoverRate;
	public UnityEvent onDeath;

	[Header("[Runtime]")]
	public int health;

	public void Process(AttackEffectSettings settings) {
		
	}
}