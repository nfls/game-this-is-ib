public class BurstParticleController : ParticleController {

	public void Spray() {
		_system.Clear();
		_system.Play();
		_started = true;
	}
}
