using UnityEngine;
using System.Collections;

public class Bow : Weapon
{

	void Start ()
	{
		damage = 0;
	}
	
	public override IEnumerator attack ()
	{
		yield return null;
	}
}
