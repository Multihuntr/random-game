using UnityEngine;
using System.Collections;

public class Sword : Weapon
{

	private static GameObject swordBlade;
	private Transform swordTransform;

	private Quaternion startingPos = Quaternion.Euler (new Vector3 (0, 0, 100));
	private Quaternion targetPos = Quaternion.Euler (new Vector3 (0, 0, -30));
	private const float swingSpeed = 300f;

	void Start ()
	{
		damage = 10;
		swordTransform = GetComponent<Transform> ().transform;
		swordBlade = GameObject.Find ("SwordBlade");
		swordBlade.SetActive (false);
	}

	public override IEnumerator attack ()
	{
		if (!attacking) {
			attacking = true;
			swordBlade.SetActive (true);

			while (swordTransform.rotation != targetPos && attacking) {
				swordTransform.rotation = Quaternion.RotateTowards (swordTransform.rotation, targetPos, swingSpeed * Time.deltaTime);
				yield return null;
			}

			swordTransform.rotation = startingPos;
			attacking = false;
			swordBlade.SetActive (false);
		}
	}
}