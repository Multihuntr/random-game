using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{

	public GameObject player;
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 (player.transform.position.x
		                                  , player.transform.position.y
		                                  , transform.position.z);
	}
}
