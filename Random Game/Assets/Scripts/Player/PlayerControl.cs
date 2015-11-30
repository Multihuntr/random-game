using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : EntityControl
{

	private Mesh mesh;

	public float runSpd;
	public float jumpSpd;
	public float jumpTriggerHeight;
	public float jumpExtendTime;

	public int facing = 1;

	private float xVel;

	private bool jumpHeld;
	private bool jumping = false;

	private Vector3 faceLeft;
	private Vector3 faceRight;
    
	void Start ()
	{
		mesh = GetComponent<MeshFilter> ().mesh;
		jumpHeld = false;
		width = mesh.bounds.size.x; // These values might not be static in final production
		height = mesh.bounds.size.y;
		faceLeft = transform.localScale;
		faceRight = new Vector3 (-faceLeft.x, faceLeft.y, faceLeft.z);
	}

	IEnumerator Jump ()
	{
		if (!jumping) {
			jumpHeld = true;
			jumping = true;
			float startTime = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup - startTime < jumpExtendTime && jumpHeld) {
				yVel = jumpSpd;
				yield return null;
			}
			jumping = false;
		}
	}

	protected override float getSlopeAngle (Vector2 dir)
	{
		return jumping ? 0 : base.getSlopeAngle (dir);
	}

	void Update ()
	{
		//Check to see if the player is in a cutscene. Different controls if they are.
		if (!GameState.inCutscene) {
			// Check Pause
			if (Input.GetButtonDown ("Pause")) {
				GameState.togglePause ();
			}

			// Calculate initial movement
			xVel = Mathf.Lerp (xVel, Input.GetAxis ("Horizontal") * runSpd, 0.3f);

			Vector2 m = calcMoveIncSlope (xVel);


			// Add Jumping Movement
			if (!jumpHeld && Input.GetAxis ("Vertical") > 0 && distToYThing (Vector2.down) < jumpTriggerHeight) {
				StartCoroutine ("Jump");
			}
			jumpHeld = Input.GetAxis ("Vertical") > 0;

			// Apply movement
			updatePos (m.x, m.y);
			xVel = m.x;
			yVel = m.y;


			// Facing Direction
			if (xVel > 0) {
				transform.localScale = faceLeft;
				facing = 1;
			} else if (xVel < 0) {
				transform.localScale = faceRight;
				facing = -1;
			}
		}
	}

	void OnLevelWasLoaded (int level)
	{
		if (!GameState.newGame) {
			transform.position = GameState.currentSave.getLoadPos ();
		}
	}

	public void injured ()
	{
		// play injury animation
	}
}
