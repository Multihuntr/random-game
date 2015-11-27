using UnityEngine;
using System.Collections;

public abstract class Health : MonoBehaviour
{
	
	public int maxHealth;
	protected int currHealth;
	
	abstract protected void takeDamage (int amount);
	
	protected void Init ()
	{
		currHealth = maxHealth;
	}
}
