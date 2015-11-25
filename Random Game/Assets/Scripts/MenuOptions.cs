using UnityEngine;
using System.Collections;


public class MenuOptions : MonoBehaviour
{

    //Takes an int representing the index of the scene in the build settings
    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}