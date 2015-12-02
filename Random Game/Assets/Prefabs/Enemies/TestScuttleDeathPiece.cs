using UnityEngine;
using System.Collections;

public class TestScuttleDeathPiece : MonoBehaviour
{
	void Start ()
	{
		Invoke ("removeDeathPiece", 4.0f);
	}
	
	void removeDeathPiece ()
	{
		Destroy (this.gameObject);
	}
}
