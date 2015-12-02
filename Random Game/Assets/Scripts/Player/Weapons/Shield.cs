using UnityEngine;
using System.Collections;

public class Shield : Weapon
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
