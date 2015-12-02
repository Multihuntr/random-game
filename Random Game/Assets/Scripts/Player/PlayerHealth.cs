using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{	
	public GameObject HealthBar;

	private const int dmgFlashes = 8;
	private Slider healthSlider;

	void Start ()
	{
		base.Init ();
		healthSlider = HealthBar.GetComponent<Slider> ();
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;
	}

	protected override IEnumerator playInjured ()
	{
		invincible = true;
		healthSlider.value = currHealth;

		// The for loop makes it wait the invincibility time
		// This is mostly code for changing the colour
		Material mat = GetComponent<MeshRenderer> ().material;
		for (float invincibleFor = 0; invincibleFor < invincibilityTime; invincibleFor += Time.deltaTime) {
			float notRed = Mathf.Abs (Mathf.Sin (invincibleFor / invincibilityTime * dmgFlashes * Mathf.PI)) / 2;
			mat.color = new Color (1, 1 - notRed, 1 - notRed);
			yield return null;
		}
		mat.color = Color.white;


		invincible = false;
	}
}
