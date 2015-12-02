using UnityEngine;
using System.Collections;

public class Sword : Weapon
{

	private static GameObject swordBlade;
	private Transform swordTransform;

	private Quaternion startingPos = Quaternion.Euler (new Vector3 (0, 0, 100));
	private Quaternion targetPos = Quaternion.Euler (new Vector3 (0, 0, -30));
	private const float swingSpeed = 300f;
	private static bool attacking = false;

	// Use this for initialization
	void Start ()
	{
		damage = 10;
		swordTransform = GetComponent<Transform> ().transform;
		swordBlade = GameObject.Find ("SwordBlade");
		swordBlade.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (attacking) {  
			swordTransform.rotation = Quaternion.RotateTowards (swordTransform.rotation, targetPos, swingSpeed * Time.deltaTime);

			if (swordTransform.rotation == targetPos) {
				swordTransform.rotation = startingPos;
				attacking = false;
				swordBlade.SetActive (false);
			}
		}
	}

	public static void attack ()
	{
		if (!attacking) {
			attacking = true;
			swordBlade.SetActive (true);
		}
	}
}
