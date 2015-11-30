using UnityEngine;
using System.Collections;

public class EnemyControl : EntityControl
{
	public int damage;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.GetComponent<PlayerHealth> ().takeDamage (1000);
		}
	}

	void Start ()
	{
		// Sort of hacky way of determining the width and height in this case
		Transform child = transform.GetChild (0);
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;
		width = mesh.bounds.size.x * child.localScale.x;
		height = mesh.bounds.size.y * child.localScale.y;
	}

	void Update ()
	{
		yVel = newYVel (yVel);
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		transform.position = new Vector3 (x, y + yVel * Time.deltaTime, z);
	}
}
