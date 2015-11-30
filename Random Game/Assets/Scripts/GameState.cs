using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameState : MonoBehaviour
{
	public static Checkpoint checkpoint;
	public static bool paused = false;
	public static string savePath = Application.persistentDataPath + "/savedGame";
	public static bool newGame = false;
	public static SaveFile currentSave;
    public static bool inCutscene = false;

	public static void save (Checkpoint cp)
	{
		// Update active checkpoint
		if (checkpoint != null && checkpoint != cp) {
			checkpoint.reset ();
		}
		checkpoint = cp;

		// Update SaveFile
		if (currentSave == null) {
			currentSave = new SaveFile ();
		}
		currentSave.save (Application.loadedLevel, cp.getPos ());

		// We're cool with loading, now.
		newGame = false;

		// Actually save the data to a file.
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (savePath);
		bf.Serialize (file, currentSave);
		file.Close ();
	}

	public static SaveFile load ()
	{
		if (File.Exists (savePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (savePath, FileMode.Open);
			currentSave = (SaveFile)bf.Deserialize (file);
			file.Close ();
			return currentSave;
		}
		return null;
	}

	public static bool saveFileExists ()
	{
		return File.Exists (savePath);
	}

	public static void togglePause ()
	{
		if (!paused) {
			paused = true;
			Time.timeScale = 0.0F;
		} else {
			paused = false;
			Time.timeScale = 1.0F;
		}
	}
}
