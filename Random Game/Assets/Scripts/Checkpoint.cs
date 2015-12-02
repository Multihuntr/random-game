using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	public Vector2 getPos ()
	{
		return (Vector2)transform.position;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			setActive ();
			other.GetComponent<Health> ().fullHeal ();
		}
	}

	public void reset ()
	{
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.startColor = Color.white;
	}

	void setActive ()
	{
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.startColor = Color.yellow;
		GameState.save (this);
	}
}
