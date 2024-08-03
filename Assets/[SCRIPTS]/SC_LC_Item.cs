using System.Collections.Generic;
using UnityEngine;

public class SC_LC_Item : SC_LC_Interactable
{
	SC_LC_PlayerGlobal player;

	public override void Interact()
	{
		base.Interact();

		PickUp();
	}

	void Start()
	{
		player = SC_LC_PlayerGlobal.instance;
	}

	void PickUp()
	{
		List<GameObject> interactions = player.interactions.interactList; //Stores the interactions list to a temporary variable

		Debug.Log("Pickup : " + transform.name);
		interactions.Remove(gameObject); //Removes the interaction from the interactions list
		Destroy(gameObject); //Destroys the interaction
	}
}
