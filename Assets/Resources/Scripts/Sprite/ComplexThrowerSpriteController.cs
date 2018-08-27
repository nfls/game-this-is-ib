using System;
using System.Collections.Generic;
using UnityEngine;

public class ComplexThrowerSpriteController : ThrowerSpriteController {
	
	public List<FireStyle> fireStyles;
	
	protected override void Fire() {
		float face = (float) characterMotor.FaceDirection;
		foreach (var fireStyle in fireStyles) {
			GravityProjectileController projectileController = ProjectileManager.Get<GravityProjectileController>(projectileType);
			fireOffset = fireStyle.offset;
			fireRotation = fireStyle.rotation;
			gravity = fireStyle.gravity;
			LoadUpProjectile(projectileController);
			float rad = fireStyle.direction * Mathf.Deg2Rad;
			Vector3 velocity = new Vector3(Mathf.Cos(rad) * face, Mathf.Sin(rad), 0).normalized * firePower;
			projectileController.Fire(velocity);
		}
		
		if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
		if (fireShake != Vector3.zero) CameraManager.Shake(transform.position, fireShake);
		if (!string.IsNullOrEmpty(fireEffect)) {
			BurstParticleController explosion = ParticleManager.Get<BurstParticleController>(fireEffect);
			explosion.transform.position = transform.position + new Vector3(transform.lossyScale.x / 2f, 0, 0);
			explosion.Burst();
		}
	}
	
	[Serializable]
	public class FireStyle {
		public Vector3 offset;
		public Vector3 rotation;
		public float direction;
		public float gravity;
	}
}