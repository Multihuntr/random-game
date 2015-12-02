using UnityEngine;
using System.Collections;

public class EntityControl : MonoBehaviour
{
	public float runSpd;
	public Vector2 stdKnockback;
	public float knockbackTime;

	private const float grav = 1f;
	// When an object is right up against another one, if we don't pad a little space, the raycasts
	//	along the side will return immediately with the other object.
	private const float padding = 0.01f;
	private const int mask = 1 | 1 << 8;
	private const float maxFallSpd = 12.0f;

	private float xKnockbackVel = 0;
	private float knockbackTimeCounter = 0;

	protected float xVel = 0;
	protected float yVel = 0;
	protected float width;
	protected float height;

	protected Vector3 faceLeft;
	protected Vector3 faceRight;


	// A function that will tell you if a face is touching something
	//	It is intended to work with either xOffset or yOffset to be 0
	// The 'dir' is the direction to check, the maxDist is how far away still counts, and
	//	the two Offsets are how far the edges are from the center on the face.
	bool touching (Vector2 dir, float maxDist, float xOffset, float yOffset)
	{
		Vector3 pos = transform.position;

		// Check 3 positions. Is the middle touching? Are either of the edges touching.
		return Physics2D.Raycast (pos, dir, maxDist, mask).collider != null
			|| Physics2D.Raycast (new Vector2 (pos.x + xOffset, pos.y + yOffset), dir, maxDist, mask).collider != null
			|| Physics2D.Raycast (new Vector2 (pos.x - xOffset, pos.y - yOffset), dir, maxDist, mask).collider != null;
	}
	
	protected bool hittingInY (Vector2 dir)
	{
		return touching (dir, height / 2, width / 2 - padding, 0);
	}
	
	bool willHitInY (Vector2 dir, float vel)
	{
		return touching (dir, height / 2 + Mathf.Abs (vel) * Time.deltaTime, width / 2 - padding, 0);
	}
	
	bool hittingWall (Vector2 dir)
	{
		return touching (dir, width / 2, 0, height / 2 - padding);
	}
	
	bool willHitWall (Vector2 dir, float vel)
	{
		return touching (dir, width / 2 + Mathf.Abs (vel) * Time.deltaTime, 0, height / 2 - padding);
	}
	
	// This raycasts from the centre of the player, so the distance returned is from the centre
	float distToThing (Vector2 dir, float xOffset, float yOffset = 0)
	{
		Vector3 pos = transform.position;

		// Ray cast three times along the edge
		RaycastHit2D left = Physics2D.Raycast (new Vector2 (pos.x - xOffset, pos.y - yOffset), dir, Mathf.Infinity, mask);
		RaycastHit2D mid = Physics2D.Raycast (new Vector2 (pos.x, pos.y), dir, Mathf.Infinity, mask);
		RaycastHit2D right = Physics2D.Raycast (new Vector2 (pos.x + xOffset, pos.y + yOffset), dir, Mathf.Infinity, mask);

		// You want the minimum, because of ledges and overhangs and stuff.
		return Mathf.Min (left.distance, mid.distance, right.distance);
	}
	
	protected float distToYThing (Vector2 dir)
	{
		return Mathf.Max (0, distToThing (dir, width / 2 - padding) - height / 2);
	}
	
	float distToWall (Vector2 dir)
	{
		return Mathf.Max (0, distToThing (dir, 0, height / 2 - padding) - width / 2);
	}

	protected virtual float getSlopeAngle (Vector2 dir)
	{
		// Gonna raycast from the bottom
		Vector2 bottomStep = new Vector2 (transform.position.x, transform.position.y - height / 2);
		// And two known distances upwards
		Vector2 oneStepUp = new Vector2 (bottomStep.x, bottomStep.y + padding);

		// It raycasts a distance for which the angle would have to be less than 1 degree not to reach.
		RaycastHit2D bottomStepHit = Physics2D.Raycast (bottomStep, dir, width / 2 + 0.01f, 1 << 8);
		RaycastHit2D oneStepUpHit = Physics2D.Raycast (oneStepUp, dir, width / 2 + 0.61f, 1 << 8);
		RaycastHit2D wallCheck = Physics2D.Raycast (oneStepUp, dir, width / 2 + 0.61f);

		if (oneStepUpHit.distance == 0 || oneStepUpHit.collider != wallCheck.collider) {
			return 90;
		}

		// Good ol' trigonometry will tell us the angle.
		return Mathf.Atan2 (padding, oneStepUpHit.distance - bottomStepHit.distance);
	}

	protected float newXVel (float xVel)
	{
		// Optimisation  - If we're not trying to move, we don't need to do anything.
		if (xVel == 0) {
			return 0;
		}

		// If the entity is to be knocked back, overwrite the xVel
		if (knockbackTimeCounter > 0) {
			xVel = xKnockbackVel;
			knockbackTimeCounter -= Time.deltaTime;
		}

		// Which way are we trying to move?
		float dirSign = Mathf.Sign (xVel);
		Vector2 dir = new Vector2 (dirSign, 0);

		// If there's a wall in the direction, stop
		// Else if we're gonna hit something during this frame, move up against it this frame
		//		(then next frame, the first branch will execute)
		if (hittingWall (dir)) {
			xVel = 0;
		} else if (willHitWall (dir, xVel)) {
			xVel = dirSign * distToWall (dir) / Time.deltaTime;
		}

		return xVel;
	}
	
	protected float newYVel (float yVel)
	{
		if (!GameState.paused) {
			// First apply gravity
			yVel -= grav;
			yVel = Mathf.Sign (yVel) * Mathf.Min (Mathf.Abs (yVel), maxFallSpd);

			// Find out which way we're gonna go
			float dirSign = Mathf.Sign (yVel);
			Vector2 dir = new Vector2 (0, dirSign);

			// If there's a wall in that direction, stop.
			// Else if we're gonna hit something during this frame, move up against it this frame
			//		(then next frame, the first branch will execute)
			if (hittingInY (dir)) {
				yVel = 0;
			} else if (willHitInY (dir, yVel)) {
				yVel = dirSign * distToYThing (dir) / Time.deltaTime;
			}
		} else {
			yVel = 0;
		}

		return yVel;
	}

	// For if the entity can climb slopes
	protected bool calcSlopeAdjustment (float xVel, ref float setYVel)
	{

		float dirSign = Mathf.Sign (xVel);
		Vector2 dir = new Vector2 (dirSign, 0);

		float slopeAngle = getSlopeAngle (dir);

		// There should be no adjustment for a slope angle on flat ground
		if (slopeAngle != 0 && slopeAngle < 70 * Mathf.Deg2Rad) {

			setYVel = dirSign * Mathf.Tan (slopeAngle) * xVel;

			// These values could be used to move proportionally slower uphill based on the angle
			// setXVel = Mathf.Cos (slopeAngle) * xVel;
			// setYVel = Mathf.Sin (slopeAngle) * xVel;
			return true;
		}

		return false;
	}

	protected void updatePos (float xVel, float yVel)
	{
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		transform.position = new Vector3 (x + xVel * Time.deltaTime, y + yVel * Time.deltaTime, z);
	}

	public void dmgKnockback (Vector2 from)
	{
		dmgKnockback (from, stdKnockback);
	}

	public void dmgKnockback (Vector2 from, Vector2 amount)
	{
		knockbackTimeCounter = knockbackTime;
		xKnockbackVel = Mathf.Sign (transform.position.x - from.x) * amount.x;
		yVel += amount.y;
	}
}
