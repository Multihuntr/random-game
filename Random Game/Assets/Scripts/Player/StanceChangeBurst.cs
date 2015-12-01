using UnityEngine;
using System.Collections;

public class StanceChangeBurst : MonoBehaviour
{
	public void burst (Color c)
	{
		ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem sys in systems) {
			sys.startColor = c;
			sys.Play ();
		}
		Invoke ("remove", 0.5f);
	}

	void remove ()
	{
		Destroy (this.gameObject);
	}
}
