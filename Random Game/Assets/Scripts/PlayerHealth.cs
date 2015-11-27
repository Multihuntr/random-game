using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{	
	public GameObject HealthBar;
	private Slider healthSlider;
	private PlayerControl controller;

	void Start ()
	{
		base.Init ();
		healthSlider = HealthBar.GetComponent<Slider> ();
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;
		controller = GetComponent<PlayerControl> ();
	}

	protected override void takeDamage (int amount)
	{
		currHealth -= amount;
		if (currHealth > 0) {
			healthSlider.value = currHealth;
			controller.injured ();
		} else {
			// You have died
		}
	}
}
