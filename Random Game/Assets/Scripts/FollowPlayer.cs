using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{

	public GameObject player;
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 target = new Vector3 (player.transform.position.x + player.GetComponent<PlayerControl> ().facing * 2
		                                  , player.transform.position.y
		                                  , transform.position.z);
		transform.position = Vector3.Lerp (transform.position, target, 0.05f);
	}
}
