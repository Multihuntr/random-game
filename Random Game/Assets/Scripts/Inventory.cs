using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {	
    public Canvas inventory;

    void Start()
    {
        inventory = GetComponent<Canvas>();
    }

	void Update () 
    {
        if (GameState.paused != inventory.enabled)
        {
            inventory.enabled = GameState.paused;
        }
	}

    public void closeInventory()
    {
        inventory.enabled = false;
        GameState.togglePause();
    }
}
