using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
	protected int damage;
	protected Vector2 knockback;
	public static bool attacking = false;

	public int getDamage ()
	{
		return damage;
	}

	public Vector2 getKnockback ()
	{
		return knockback;
	}

	public abstract IEnumerator attack ();
}
