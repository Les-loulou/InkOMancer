using UnityEngine;

public class SC_LC_PlayerGlobal : MonoBehaviour
{
	public static SC_LC_PlayerGlobal instance;

	[HideInInspector] public SC_LC_PlayerControls controls;
	[HideInInspector] public SC_LC_PlayerMovements movements;
	[HideInInspector] public SC_LC_PlayerInteractions interactions;

	void Awake()
	{
		#region SINGLETON
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		#endregion
	}

	void Start()
    {
		controls = SC_LC_PlayerControls.instance;
		movements = GetComponent<SC_LC_PlayerMovements>();
		interactions = GetComponent<SC_LC_PlayerInteractions>();
	}
}
