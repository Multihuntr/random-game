using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public Transform toSpawn;
	public float distToSpawn;

	static float checkTime = 5.0f;

	Object spawned;

	void Start ()
	{
		InvokeRepeating ("checkSpawn", 0, checkTime);
	}

	void checkSpawn ()
	{
		// Only called every 'checkTime' seconds, and it checks if the camera is far enough away
		Vector3 distFromView = Camera.main.WorldToViewportPoint (transform.position);
		if (Mathf.Abs (distFromView.x) > distToSpawn || Mathf.Abs (distFromView.y) > distToSpawn) {
			spawn ();
		}
	}

	void spawn ()
	{
		// Allow only one thing spawned per spawner
		if (spawned == null) {
			spawned = Instantiate (toSpawn, transform.position, Quaternion.identity);
		}
	}
}