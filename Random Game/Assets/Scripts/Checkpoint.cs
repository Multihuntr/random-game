using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			Checkpoint prev = GameState.checkpoint;
			if (prev != null) {
				prev.reset ();
			}
			setActive ();
		}
	}

	void reset ()
	{
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.startColor = Color.white;
	}

	void setActive ()
	{
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.startColor = Color.yellow;
		GameState.checkpoint = this;
	}
}
