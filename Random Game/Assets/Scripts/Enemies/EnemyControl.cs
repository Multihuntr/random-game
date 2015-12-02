using UnityEngine;
using System.Collections;

public class EnemyControl : EntityControl
{
	public int damage;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.GetComponent<PlayerHealth> ().takeDamage (transform.position, damage);
		}
	}

	void Update ()
	{
		yVel = newYVel (yVel);
		updatePos (0, yVel);
	}
}
