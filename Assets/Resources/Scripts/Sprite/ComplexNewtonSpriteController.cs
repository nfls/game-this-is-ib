using UnityEngine;

public class ComplexNewtonSpriteController : ComplexThrowerSpriteController {
	
	public bool limitedByBounceTime;
	public bool limitedByLifeSpan;
	public int maxBounceTime;
	[Range(0f, 1f)]
	public float bounciness = 1f;
	
	protected override void LoadUpProjectile(ProjectileController projectileController) {
		base.LoadUpProjectile(projectileController);
		NewtonProjectileController controller = (NewtonProjectileController) projectileController;
		controller.limitedByBounceTime = limitedByBounceTime;
		controller.limitedByLifeSpan = limitedByLifeSpan;
		controller.maxBounceTime = maxBounceTime;
		controller.bounciness = bounciness;
	}
}