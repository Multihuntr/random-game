using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{	
	public GameObject HealthBar;
	public int invincibilityFrames;
	public int dmgFlashes;

	private Slider healthSlider;
	private PlayerControl controller;

	private bool invincible = false;

	void Start ()
	{
		base.Init ();
		healthSlider = HealthBar.GetComponent<Slider> ();
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;
		controller = GetComponent<PlayerControl> ();
	}

	public override void takeDamage (Vector2 source, int amount)
	{
		if (invincible) {
			return; // HAHAAAA! Fools! You can't hurt me!
		}

		currHealth -= amount;
		if (currHealth > 0) {
			healthSlider.value = currHealth;
			controller.dmgKnockback (source);
			StartCoroutine ("injured");
		} else {
			currHealth = 0;
			// You have died
		}
	}

	IEnumerator injured ()
	{
		invincible = true;
		Material mat = GetComponent<MeshRenderer> ().material;
		for (int invincibleFor = 0; invincibleFor < invincibilityFrames; ++ invincibleFor) {
			float notRed = Mathf.Abs (Mathf.Sin ((float)invincibleFor / invincibilityFrames * dmgFlashes * Mathf.PI)) / 2;
			mat.color = new Color (1, 1 - notRed, 1 - notRed);
			yield return null;
		}
		mat.color = Color.white;
		invincible = false;
	}
}
