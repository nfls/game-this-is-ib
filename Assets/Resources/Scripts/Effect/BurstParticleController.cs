public sealed class BurstParticleController : ParticleController {

	public void Burst() {
		_system.Clear();
		_system.Play();
		_started = true;
	}
}
