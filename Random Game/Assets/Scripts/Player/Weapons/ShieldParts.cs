using UnityEngine;
using System.Collections;

public class ShieldParts : MonoBehaviour
{
	Animator animator;

	void Start ()
	{
		animator = GetComponentInChildren<Animator> ();
	}
	
	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			animator.SetTrigger ("Hit");
		}
	}
}
