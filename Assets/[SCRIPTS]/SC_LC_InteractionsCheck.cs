using UnityEngine;

public class SC_LC_InteractionsCheck : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	private void Start()
	{
		player = SC_LC_PlayerGlobal.instance;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Interactable")) //If the collider has the tag "Interactable"
			player.interactions.interactList.Add(other.gameObject); //Adds the interaction to the interactions list
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Interactable")) //If the collider has the tag "Interactable"
			player.interactions.interactList.Remove(other.gameObject); //Removes the interaction from the interactions list
	}
}
