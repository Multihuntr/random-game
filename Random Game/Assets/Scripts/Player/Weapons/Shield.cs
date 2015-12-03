using UnityEngine;
using System.Collections;

public class Shield : Weapon
{
	static GameObject shieldParts;
	static Animator animator;
	static float shieldTime = 1.5f;

	void Start ()
	{
		damage = 0;
		knockback = 1.2f * Vector2.one;
		shieldParts = transform.GetChild (0).gameObject;
		animator = GetComponentInChildren<Animator> ();
		shieldParts.SetActive (false);
	}

	public override IEnumerator attack ()
	{
		if (!attacking) {
			attacking = true;
			shieldParts.SetActive (true);

			for (float timer = shieldTime; timer > 0 && attacking; timer -= Time.deltaTime) {
				yield return null;
			}

			attacking = false;
			shieldParts.SetActive (false);
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			animator.SetTrigger ("Hit");
		}
	}
}
