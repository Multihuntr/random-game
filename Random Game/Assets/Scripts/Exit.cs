﻿using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
	public int next;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Player")) {
			Camera.main.GetComponentInChildren<Fader> ().fadeOut (next);
		}
	}
}
