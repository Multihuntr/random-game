using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{	
	public Canvas inventory;
    private static bool inventoryOn = false;

	void Start ()
	{
		inventory = GetComponent<Canvas> ();
	}

	void Update ()
	{
        if (inventoryOn != inventory.enabled)
        {
            inventory.enabled = inventoryOn;
		}
	}

	public void closeInventory ()
	{
		//inventory.enabled = false;
		//GameState.togglePause ();
        toggleInventory();
	}

    //Static method allows the player to call the inventory open when they need it. This separates the
    //pausing function from the inventory function; you can pause without opening inventory.
    public static void toggleInventory()
    {
        inventoryOn = !inventoryOn;
        GameState.togglePause();
    }
}
