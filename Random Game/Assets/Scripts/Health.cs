using UnityEngine;
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

	public virtual void heal (int amount)
	{
		currHealth = Mathf.Min (currHealth + amount, maxHealth);
	}

	public virtual void fullHeal ()
	{
		heal (maxHealth);
	}

	public virtual void takeDamage (Vector2 source, int amount, Vector2 knockback)
	{
		wasHit (source, knockback);
		if (invincible) {
			return; // HAHAAAA! Fools! You can't hurt me!
		}

		currHealth -= amount;
		if (currHealth > 0) {
			StartCoroutine ("playInjured");
		} else {
			currHealth = 0;
			onDeath ();
		}
	}

	protected virtual void wasHit (Vector2 source, Vector2 knockback)
	{
		GetComponent<EntityControl> ().dmgKnockback (source, knockback);
	}

	protected abstract IEnumerator playInjured ();

	protected virtual void onDeath ()
	{
		Destroy (this.gameObject);
	}
	
	protected void Init ()
	{
		currHealth = maxHealth;
	}
}