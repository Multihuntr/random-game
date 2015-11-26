using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveFile
{
	private float x;
	private float y;
	private int lvl;

	public void save (int level, Vector2 pos)
	{
		lvl = level;
		x = pos.x;
		y = pos.y + 2;
	}

	public Vector2 getLoadPos ()
	{
		return new Vector2 (x, y);
	}

	public int getLoadLevel ()
	{
		return lvl;
	}

	public void disp ()
	{
		Debug.Log ("You have loaded level: " + lvl + " at  (" + x + ", " + y + ")");
	}
}
