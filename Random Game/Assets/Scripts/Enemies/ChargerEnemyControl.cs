using UnityEngine;
using System.Collections;
using System;

public class ChargerEnemyControl : EnemyControl
{
	public float mvSpd = 15.0f;
    private float xVel = 0;
    private LayerMask mask = 1 << 2;
	int dir = 1; // direction the enemy is moving
    

    private Vector3 startPos;
    bool horizontal = true; //If the charger is moving horizontally or not

	void Start ()
	{
		// Sort of hacky way of determining the width and height in this case
		Transform child = transform.GetChild (0);
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;
		width = mesh.bounds.size.x * child.localScale.x;
		height = mesh.bounds.size.y * child.localScale.y;
		faceLeft = transform.localScale;
		faceRight = new Vector3 (-faceLeft.x, faceLeft.y, faceLeft.z);
        startPos = transform.position;

        StartCoroutine("FindPlayer");
        
	}


    //Rayscasts left and right waiting for a player to come into vision
    IEnumerator FindPlayer()
    {
        bool foundPlayer = false;
        while (!foundPlayer)
        {
            try
            {
                if ( Physics2D.Raycast(new Vector2(transform.position.x + 1, transform.position.y), (Vector2.right), 15.0f, 1 << 2 | 1 << 0).collider.tag == "Player")
                {
                    foundPlayer = true;
                    dir = 1;
                    horizontal = true;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                //To catch raycasts that find nothing. Shouldn't come up, since it will probably be always bounded by walls.                                                       
            }
            try
            {
                if (Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y), (Vector2.left), 15.0f, 1 << 2 | 1 << 0).collider.tag == "Player")
                {
                    foundPlayer = true;
                    dir = -1;
                    horizontal = true;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                //To catch raycasts that find nothing. Shouldn't come up, since it will probably be always bounded by walls.                                                       
            }
            try
            {
                if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1), (Vector2.up), 15.0f, 1 << 2 | 1 << 0).collider.tag == "Player")
                {
                    foundPlayer = true;
                    dir = 1;
                    horizontal = false;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                //To catch raycasts that find nothing. Shouldn't come up, since it will probably be always bounded by walls.                                                       
            }
            try
            {
                if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1), (Vector2.down), 15.0f, 1 << 2 | 1 << 0).collider.tag == "Player")
                {
                    foundPlayer = true;
                    dir = -1;
                    horizontal = false;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                //To catch raycasts that find nothing. Shouldn't come up, since it will probably be always bounded by walls.                                                       
            }
            yield return null;
        }
        StartCoroutine("Charge");
    }

    IEnumerator Charge()
    {
        //Keeps charging until it hits a wall
        while (!(willHitWall(Vector2.right, xVel) || willHitWall(Vector2.left, xVel) || willHitInY(Vector2.down, yVel) || willHitInY(Vector2.up, yVel)))
        {
            xVel = 0;
            yVel = 0;
            if (horizontal)
            {
                xVel = newXVel(dir * mvSpd);
            }
            else
            {
                yVel = newXVel(dir * mvSpd);
            }

            updatePos(xVel, yVel);

            yield return null;
        }
        StartCoroutine("Returning");
    }

    IEnumerator Returning()
    {
        //Reverses to original position
        while ((transform.position - startPos).sqrMagnitude < -0.01f || (transform.position - startPos).sqrMagnitude > 0.01f)
        {
            float xVel = 0;
            yVel = 0;

            if (horizontal)
            {
                xVel = newXVel((dir * -1) * (mvSpd / 5));
            }
            else
            {
                yVel = newXVel((dir * -1) * (mvSpd / 5));
            }

            updatePos(xVel, yVel);

            yield return null;
        }
        //Once back it begins waiting for the player again.
        StartCoroutine("FindPlayer");
    }

	void Update ()
    {

       
    }
}
