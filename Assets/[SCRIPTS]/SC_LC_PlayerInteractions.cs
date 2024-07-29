using System.Collections.Generic;
using UnityEngine;

public class SC_LC_PlayerInteractions : MonoBehaviour
{
	SC_LC_PlayerControls controls;

	[SerializeField] Transform playerTransform;

	public List<GameObject> interactList = new List<GameObject>();

	public float newOutlineWidth;

	#region SINGLETON
	public static SC_LC_PlayerInteractions instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}
	#endregion

	private void Start()
	{
		controls = SC_LC_PlayerControls.instance;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Interactable"))
			interactList.Add(other.gameObject);
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Interactable"))
			interactList.Remove(other.gameObject);
	}

	void Update()
	{
		SortInteractions();

		if (controls.interactPressed)
		{
			if (interactList.Count > 0 && interactList[0] != null)
			{
				SC_LC_Interactable interactable = interactList[0].gameObject.GetComponent<SC_LC_Interactable>();

				if (interactable != null)
					interactable.Interact();
			}
		}
	}

	void SortInteractions()
	{
		interactList.Sort((a, b) => (a.transform.position - transform.position).sqrMagnitude.CompareTo((b.transform.position - transform.position).sqrMagnitude));
	}
}
