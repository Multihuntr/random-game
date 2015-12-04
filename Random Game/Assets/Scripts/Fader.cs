using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
	const float fadeSpd = 2.0f;

	int levelToLoad;
	SpriteRenderer ren;

	void Awake ()
	{
		ren = GetComponent<SpriteRenderer> ();
		ren.enabled = false;
	}

	public void fadeOut (int lvl)
	{
		levelToLoad = lvl;
		StartCoroutine ("fadeOutCo");
	}

	IEnumerator fadeOutCo ()
	{
		ren.enabled = true;
		ren.color = Color.clear;
		yield return null;

		float newA;
		while (ren.color.a < 0.99f) {
			newA = Mathf.Lerp (ren.color.a, 1.2f, fadeSpd * Time.deltaTime);
			ren.color = new Color (0, 0, 0, newA);
			yield return null;
		}

		Application.LoadLevel (levelToLoad);
	}

	IEnumerator fadeIn ()
	{
		ren.enabled = true;
		ren.color = Color.black;
		yield return null;

		Debug.Log ("Getting past here");
		
		float newA;
		while (ren.color.a > 0.01f) {
			Debug.Log ("This is going");
			newA = Mathf.Lerp (ren.color.a, -0.2f, fadeSpd * Time.deltaTime);
			ren.color = new Color (0, 0, 0, newA);
			yield return null;
		}
		
		ren.enabled = false;
	}

	void OnLevelWasLoaded (int lvl)
	{
		StartCoroutine ("fadeIn");
	}
}
