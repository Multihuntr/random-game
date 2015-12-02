﻿using UnityEngine;
using System.Collections;

public abstract class Health : MonoBehaviour
{
	
	public int maxHealth;
	protected int currHealth;
	
	public float invincibilityTime;
	protected bool invincible = false;

	void Start ()
	{
		Init ();
	}

	public virtual void takeDamage (Vector2 source, int amount)
	{
		if (invincible) {
			return; // HAHAAAA! Fools! You can't hurt me!
		}

		currHealth -= amount;
		if (currHealth > 0) {
			tookDamage (source);
			StartCoroutine ("playInjured");
		} else {
			currHealth = 0;
			// You have died
		}
	}

	protected virtual void tookDamage (Vector2 source)
	{
		GetComponent<EntityControl> ().dmgKnockback (source);
	}

	protected abstract IEnumerator playInjured ();
	
	protected void Init ()
	{
		currHealth = maxHealth;
	}
}