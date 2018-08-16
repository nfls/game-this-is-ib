using UnityEngine;

public class ThrowerSpriteController : ShooterSpriteController {

	public float direction;
	public float gravity;
	public bool rotateWithVelocity;

	protected override void Fire() {
		GravityProjectileController projectileController = ProjectileManager.Get<GravityProjectileController>(projectileType);
		LoadUpProjectile(projectileController);
		float rad = direction * Mathf.Deg2Rad;
		Vector3 velocity = new Vector3(Mathf.Cos(rad) * (float) characterMotor.FaceDirection * firePower, Mathf.Sin(rad) * firePower, 0);
		projectileController.Fire(velocity);
		if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
		if (fireShake != Vector3.zero) CameraManager.Shake(transform.position, fireShake);
		if (!string.IsNullOrEmpty(fireEffect)) {
			BurstParticleController explosion = ParticleManager.Get<BurstParticleController>(fireEffect);
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