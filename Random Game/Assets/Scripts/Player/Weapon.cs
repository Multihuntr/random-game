using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
	protected int damage;
	protected static bool attacking = false;

	public int getDamage ()
	{
		return damage;
	}

	public abstract IEnumerator attack ();
}
