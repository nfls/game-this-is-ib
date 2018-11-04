using UnityEngine;

public class ThrowerSpriteController : ShooterSpriteController {

	public float direction;
	public float gravity;
	public bool rotateWithVelocity;

	protected override void Fire() {
		GravityProjectileController projectileController = projectileType.Get<GravityProjectileController>();
		LoadUpProjectile(projectileController);
		float rad = direction * Mathf.Deg2Rad;
		Vector3 velocity = new Vector3(Mathf.Cos(rad) * (float) characterMotor.FaceDirection, Mathf.Sin(rad), 0).normalized * firePower;
		projectileController.Fire(velocity);
		if (attackSound) _audioSource.PlayOneShot(attackSound.Source);
		if (fireShake != Vector3.zero) CameraManager.Shake(transform.position, fireShake);
		if (fireEffect) {
			BurstParticleController explosion = fireEffect.Get<BurstParticleController>();
			explosion.transform.position = transform.position + new Vector3(transform.lossyScale.x / 2f, 0, 0);
			explosion.Burst();
		}
	}

	protected override void LoadUpProjectile(ProjectileController projectileController) {
		base.LoadUpProjectile(projectileController);
		GravityProjectileController controller = (GravityProjectileController) projectileController;
		controller.gravity = gravity;
		controller.rotateWithVelocity = rotateWithVelocity;
	}
}