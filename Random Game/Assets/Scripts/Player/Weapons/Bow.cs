using UnityEngine;
using System.Collections;

public class Bow : Weapon
{

	void Start ()
	{
		damage = 0;
		knockback = 0.3f * Vector2.one;
	}
	
	public override IEnumerator attack ()
	{
		yield return null;
	}
}
