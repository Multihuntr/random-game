using UnityEngine;
using System.Collections;

public class EnemyControl : EntityControl
{
	public int damage;

	protected Health myHealth;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.GetComponent<PlayerHealth> ().takeDamage (transform.position, damage);
		} else if (other.gameObject.tag == "WeaponAttack") {
			myHealth.takeDamage (other.gameObject.transform.position
			                     , other.transform.parent.GetComponent<Weapon> ().getDamage ());
		}
	}

	void Update ()
	{
		yVel = newYVel (yVel);
		updatePos (0, yVel);
	}
}
