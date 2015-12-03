using UnityEngine;
using System.Collections;

public class EnemyControl : EntityControl
{
	public int damage;
	public Vector2 knockbackModOnHit;

	protected Health myHealth;

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.GetComponent<PlayerHealth> ().takeDamage (transform.position, damage, knockbackModOnHit);
		} else if (other.gameObject.tag == "WeaponAttack") {
			Weapon w = other.transform.parent.GetComponent<Weapon> ();
			myHealth.takeDamage (other.gameObject.transform.position, w.getDamage (), w.getKnockback ());
		}
	}

	void Update ()
	{
		yVel = newYVel (yVel);
		updatePos (0, yVel);
	}
}
