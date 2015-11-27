using UnityEngine;
using System.Collections;

public class EntityControl : MonoBehaviour
{

	private const float grav = 1f;
	// When an object is right up against another one, if we don't pad a little space, the raycasts
	//	along the side will return immediately with the other object.
	private const float padding = 0.01f;
	private const int mask = 1 | 1 << 8;


	protected float yVel = 0;
	protected float width;
	protected float height;


	// A function that will tell you if a face is touching something
	//	It is intended to work with either xOffset or yOffset to be 0
	// The 'dir' is the direction to check, the maxDist is how far away still counts, and
	//	the two Offsets are how far the edges are from the center on the face.
	bool touching (Vector2 dir, float maxDist, float xOffset, float yOffset, Vector3? pos = null)
	{
		if (pos == null) {
			pos = transform.position;
		}
		// Check 3 positions. Is the middle touching? Are either of the edges touching.
		return Physics2D.Raycast (pos.Value, dir, maxDist, mask).collider != null
			|| Physics2D.Raycast (new Vector2 (pos.Value.x + xOffset, pos.Value.y + yOffset), dir, maxDist, mask).collider != null
			|| Physics2D.Raycast (new Vector2 (pos.Value.x - xOffset, pos.Value.y - yOffset), dir, maxDist, mask).collider != null;
	}
	
	bool hittingInY (Vector2 dir)
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

	bool hittingSlope (Vector2 dir)
	{
		Vector3 areaOfInterest = new Vector3 (transform.position.x
		                                      , transform.position.y - height / 2 + 0.1f
		                                      , transform.position.z);
		return touching (dir, width / 2, 0, 0, areaOfInterest);
	}
	
	// This raycasts from the centre of the player, so the distance returned is from the centre
	float distToThing (Vector2 dir, float xOffset, float yOffset = 0, Vector3? pos = null)
	{
		if (pos == null) {
			pos = transform.position;
		}

		// Ray cast three times along the edge
		RaycastHit2D left = Physics2D.Raycast (new Vector2 (pos.Value.x - xOffset, pos.Value.y - yOffset), dir, Mathf.Infinity, mask);
		RaycastHit2D mid = Physics2D.Raycast (new Vector2 (pos.Value.x, pos.Value.y), dir, Mathf.Infinity, mask);
		RaycastHit2D right = Physics2D.Raycast (new Vector2 (pos.Value.x + xOffset, pos.Value.y + yOffset), dir, Mathf.Infinity, mask);

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

	float getSlopeAngle (Vector2 dir)
	{
		// Gonna raycast from the bottom
		Vector2 bottomStep = new Vector2 (transform.position.x, transform.position.y - height / 2);
		// And a known distance upwards
		Vector2 oneStepUp = new Vector2 (bottomStep.x, bottomStep.y + 0.1f);

		// It raycasts a distance for which the angle would have to be less than 1 degree not to reach.
		RaycastHit2D bottomStepHit = Physics2D.Raycast (bottomStep, dir, width / 2 + 0.01f, 1 << 8);
		RaycastHit2D oneStepUpHit = Physics2D.Raycast (oneStepUp, dir, width / 2 + 6.01f, 1 << 8);
		RaycastHit2D wallCheck = Physics2D.Raycast (oneStepUp, dir, width / 2 + 6.01f);

		if (oneStepUpHit.distance == 0 || oneStepUpHit.collider != wallCheck.collider) {
			return 90;
		}

		// Good ol' trigonometry will tell us the angle.
		return Mathf.Atan2 (0.1f, oneStepUpHit.distance - bottomStepHit.distance);
	}

	protected float newXVel (float xVel)
	{
		// Optimisation  - If we're not trying to move, we don't need to do anything.
		if (xVel == 0) {
			return 0;
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
	protected Vector2 calcMoveIncSlope (float xVel)
	{
		float setXVel = newXVel (xVel);
		float setYVel = newYVel (yVel);


		// If it has been slowed, then it might have hit a slope
		if (setXVel != xVel && (yVel <= 0 || hittingInY (Vector2.down))) {
			float dirSign = Mathf.Sign (xVel);
			Vector2 dir = new Vector2 (dirSign, 0);
			float slopeAngle = getSlopeAngle (dir);
			if (slopeAngle != 0 && slopeAngle < 70 * Mathf.Deg2Rad) {

				setXVel = xVel;
				setYVel = dirSign * Mathf.Tan (slopeAngle) * xVel;

				// Uncomment these to make the player move proportionally slower uphill based on the angle
				// setXVel = Mathf.Cos (slopeAngle) * xVel;
				// setYVel = Mathf.Sin (slopeAngle) * xVel;
				
			}
		}

		return new Vector2 (setXVel, setYVel);

	}
}
