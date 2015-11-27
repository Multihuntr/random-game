using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
	
	public static Checkpoint checkpoint;
    public static bool paused;

	// Use this for initialization
	void Start ()
	{
        paused = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            togglePause();
        }
    }

    public static void togglePause()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0.0F;
        }
        else
        {
            paused = false;
            Time.timeScale = 1.0F;
        }
    }

}
