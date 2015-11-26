using UnityEngine;
using System.Collections;

public class PlayerControl : EntityControl
{

	private Mesh mesh;

	public float runSpd;
	public float jumpSpd;
	public float jumpTriggerHeight;

	private bool jumpHeld;
    
	void Start ()
	{
		mesh = GetComponent<MeshFilter> ().mesh;
		jumpHeld = false;
		width = mesh.bounds.size.x; // These values might not be static in final production
		height = mesh.bounds.size.y;
	}
	
	void Update ()
	{

		// Calculate initial movement
		float xVel = Input.GetAxis ("Horizontal") != 0 ? Input.GetAxis ("Horizontal") * runSpd : 0;

		Vector2 m = calcMoveIncSlope (xVel);


		// Add Jumping Movement
		if (Input.GetAxis ("Vertical") > 0) {
			if (!jumpHeld && distToYThing (Vector2.down) < jumpTriggerHeight) {
				m = new Vector2 (m.x, jumpSpd);
			}
			jumpHeld = true;
		} else {
			jumpHeld = false;
		}


		// Apply movement
		float x = transform.position.x;
		float y = transform.position.y;

		transform.position = new Vector3 (x + m.x * Time.deltaTime, y + m.y * Time.deltaTime, 0);
		yVel = m.y;

	}

	void OnLevelWasLoaded (int level)
	{
		if (!GameState.newGame) {
			transform.position = GameState.currentSave.getLoadPos ();
		}
	}
}
