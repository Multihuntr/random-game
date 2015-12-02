using UnityEngine;
using System.Collections;

public class ScuttleEnemyHealth : EnemyHealth
{
	protected override IEnumerator playInjured ()
	{
		invincible = true;

		for (float invincibleFor = 0; invincibleFor < invincibilityTime; invincibleFor += Time.deltaTime) {
			yield return null;
		}

		invincible = false;
	}
}