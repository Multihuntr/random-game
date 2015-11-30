using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject toSpawn;
	public float distToSpawn;

	GameObject spawned;

	void Start ()
	{
		InvokeRepeating ("checkSpawn", 0, 1.0f);
	}

	void checkSpawn ()
	{
		Vector3 distFromView = Camera.main.WorldToViewportPoint (transform.position);
		if (Mathf.Abs (distFromView.x) > distToSpawn || Mathf.Abs (distFromView.y) > distToSpawn) {
			spawn ();
		}
	}

	void spawn ()
	{
		Debug.Log ("Spawned");
	}
}