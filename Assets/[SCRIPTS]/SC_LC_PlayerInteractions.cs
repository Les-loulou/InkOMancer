using System.Collections.Generic;
using UnityEngine;

public class SC_LC_PlayerInteractions : MonoBehaviour
{
	SC_LC_PlayerControls controls;

	public List<GameObject> interactList = new List<GameObject>();

	void Start()
    {
		controls = SC_LC_PlayerControls.instance;
	}

    void Update()
    {
		SortInteractions(); //Sorts every interaction from the nearest to the furthest

		if (interactList.Count == 0) //Ignore if the list is empty
			return;

		if (controls.interactPressed == true) //When interact control is pressed
		{
			SC_LC_Interactable interactable = interactList[0].gameObject.GetComponent<SC_LC_Interactable>(); //Stores the first index of the interactions list in the "interactable" variable
			interactable.Interact(); //Interacts with it
		}
	}

	void SortInteractions()
	{
		interactList.Sort((a, b) => (a.transform.position - transform.position).sqrMagnitude.CompareTo((b.transform.position - transform.position).sqrMagnitude));
	}
}
