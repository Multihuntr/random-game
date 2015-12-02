using UnityEngine;
using System.Collections;

public abstract class Health : MonoBehaviour
{
	
	public int maxHealth;
	protected int currHealth;
	
	abstract public void takeDamage (Vector2 source, int amount);
	
	protected void Init ()
	{
		currHealth = maxHealth;
	}
}