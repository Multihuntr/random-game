using UnityEngine;
using System.Collections;

public class ScuttleEnemyControl : EnemyControl
{
	const float mvSpd = 3.0f;
	int dir = 1;

	void Start ()
	{
		// Sort of hacky way of determining the width and height in this case
		Transform child = transform.GetChild (0);
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;
		width = mesh.bounds.size.x * child.localScale.x;
		height = mesh.bounds.size.y * child.localScale.y;
	}

	void Update ()
	{
		float xVel = newXVel (dir * mvSpd);
		
		if (xVel == 0) {
			dir = -dir;
		}

		yVel = newYVel (yVel);

		updatePos (xVel, yVel);
	}
}
