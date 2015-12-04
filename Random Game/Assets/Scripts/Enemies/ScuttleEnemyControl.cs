using UnityEngine;
using System.Collections;

public class ScuttleEnemyControl : EnemyControl
{
	int dir = 1; // direction the enemy is moving

	void Start ()
	{
		// Sort of hacky way of determining the width and height in this case
		Transform child = transform.GetChild (0);
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;
		width = mesh.bounds.size.x * child.localScale.x;
		height = mesh.bounds.size.y * child.localScale.y;
		faceLeft = transform.localScale;
		faceRight = new Vector3 (-faceLeft.x, faceLeft.y, faceLeft.z);

		myHealth = GetComponent<EnemyHealth> ();
	}

	void Update ()
	{
		xVel = newXVel (dir * runSpd);
		
		if (Mathf.Abs (xVel) < 0.01f || (willHitInY (Vector2.down, yVel) && pokingOut ())) {
			dir = -dir;
			transform.localScale = dir > 0 ? faceRight : faceLeft;
		}

		yVel = newYVel (yVel);

		updatePos (xVel, yVel);
	}

	bool pokingOut ()
	{
		Vector2 leadingFoot = new Vector2 (transform.position.x + dir * width / 2
		                                  , transform.position.y - height / 2);
		RaycastHit2D hit = Physics2D.Raycast (leadingFoot, Vector2.down, width);
		return hit.distance > 0.01f || hit.collider == null;
	}
}
