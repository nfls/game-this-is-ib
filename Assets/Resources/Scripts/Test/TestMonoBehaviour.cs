using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour {

	public Collider collider;
	public float distance = 20f;

	public void Update() {
		if (Input.GetKeyUp(KeyCode.T)) {
			// Perform a single raycast using RaycastCommand and wait for it to complete
			// Setup the command and result buffers
			var results = new NativeArray<RaycastHit>(1, Allocator.Temp);

			var commands = new NativeArray<RaycastCommand>(1, Allocator.Temp);

			// Set the data of the first command
			Ray ray = CameraManager.MainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

			commands[0] = new RaycastCommand(ray.origin, ray.direction);
			
			Debug.Log("Before Hit : " + Time.time);

			// Schedule the batch of raycasts
			JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1);

			// Wait for the batch processing job to complete
			handle.Complete();

			// Copy the result. If batchedHit.collider is null there was no hit
			RaycastHit batchedHit = results[0];
			
			Debug.Log("After Hit : " + Time.time);
			collider = batchedHit.collider;

			// Dispose the buffers
			results.Dispose();
			commands.Dispose();
		}
	}
}