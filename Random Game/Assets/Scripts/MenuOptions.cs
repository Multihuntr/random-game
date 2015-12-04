using UnityEngine;
using System.Collections;


public class MenuOptions : MonoBehaviour
{
	void Awake ()
	{
		GameObject.Find ("Continue Game Button").SetActive (GameState.saveFileExists ());
	}

	public void NewGame ()
	{
		GameState.newGame = true;

		//Takes an int representing the index of the scene in the build settings
		Application.LoadLevel (2);
	}

	public void ContinueGame ()
	{
		SaveFile saved = GameState.load ();
		Application.LoadLevel (saved.getLoadLevel ());
	}

	public void exitGame ()
	{
		Application.Quit ();
	}
}