using System.Collections.Generic;
using UnityEngine;

public class SC_LC_Item : SC_LC_Interactable
{
	public override void Interact()
	{
		base.Interact();

		PickUp();
	}

	void PickUp()
	{
		List<GameObject> interactions = SC_LC_PlayerInteractions.instance.interactList;

		Debug.Log("Pickup : " + transform.name);
		interactions.Remove(gameObject);
		Destroy(gameObject);
	}
}
