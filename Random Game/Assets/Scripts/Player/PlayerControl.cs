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
				Inventory.toggleInventory();
			}

			// Calculate movement
			move ();


			// Facing Direction
			if (xVel > 0) {
				transform.localScale = faceLeft;
				facing = 1;
			} else if (xVel < 0) {
				transform.localScale = faceRight;
				facing = -1;
			}
			
			if (Input.GetButtonDown("ActionBtn"))
            {
                Sword.attack();
            }
		}
	}

	void OnLevelWasLoaded (int level)
	{
		if (!GameState.newGame) {
			transform.position = GameState.currentSave.getLoadPos ();
		}
	}

	void move ()
	{
		// Determine desired xVel based on input
		xVel = Mathf.Lerp (xVel, Input.GetAxis ("Horizontal") * runSpd, 0.3f);

		// Detect any collisions in the x-direction
		float setXVel = newXVel (xVel);

		// If it has been slowed, then it might have hit a slope
		bool sloped = false;
		if (setXVel != xVel && (yVel <= 0 || hittingInY (Vector2.down))) {
			float setYVel = 0;
			sloped = calcSlopeAdjustment (xVel, ref setYVel);
			if (sloped) {
				yVel = setYVel;
				setXVel = xVel;
			}
		}

		// Move along the x-axis
		updatePos (setXVel, 0);
		xVel = setXVel;

		// Before we determine if the player is jumping, we check for collisions (ignoring this if we're on a slope)
		yVel = sloped ? yVel : newYVel (yVel);
		// If jumping we'll just set yVel to something else anyway
		if (!jumpHeld && Input.GetAxis ("Vertical") > 0 && distToYThing (Vector2.down) < jumpTriggerHeight) {
			StartCoroutine ("Jump");
		}
		jumpHeld = Input.GetAxis ("Vertical") > 0;
		
		
		// Apply y movement
		updatePos (0, yVel);
	}

	public void injured ()
	{
		// play injury animation
	}
}
