using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScuttleEnemyHealth : EnemyHealth
{
	public GameObject deathPiece;

	private List<GameObject> deathPieces;

	protected override IEnumerator playInjured ()
	{
		invincible = true;

		for (float invincibleFor = 0; invincibleFor < invincibilityTime; invincibleFor += Time.deltaTime) {
			yield return null;
		}

		invincible = false;
	}

	protected override void onDeath ()
	{
		// Ok, reader, the plan is to make some pieces and make them fly apart.

		// Gather up some dimensions
		Transform child = transform.GetChild (0);
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;

		Vector3 scale = child.localScale;
		float width = mesh.bounds.size.x * scale.x;
		float height = mesh.bounds.size.y * scale.y;

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		
		base.onDeath ();

		// Make those pieces and set them to fly
		deathPieces = new List<GameObject> ();
		for (int across = -1; across <= 1; across += 2) {
			for (int down = -1; down <= 1; down += 2) {
				GameObject piece = (GameObject)Instantiate (deathPiece);
				piece.GetComponent<Rigidbody2D> ().velocity = new Vector2 (across * 5.0f, down * 5.0f);
				piece.transform.position = new Vector3 (x + across * width / 4, y + down * height / 4, z);
				piece.transform.localScale = new Vector3 (scale.x / 2, scale.y / 2, scale.z);
				deathPieces.Add (deathPiece);
			}
		}
	}
}