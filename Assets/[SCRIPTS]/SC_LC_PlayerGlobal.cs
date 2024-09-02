using UnityEngine;

public class SC_LC_PlayerGlobal : MonoBehaviour
{
	public static SC_LC_PlayerGlobal instance;

	[HideInInspector] public SC_LC_PlayerControls inputs;
	[HideInInspector] public SC_LC_PlayerMovements movements;
	[HideInInspector] public SC_LC_PlayerInteractions interactions;
	[HideInInspector] public SC_LC_PlayerHealth health;

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
		inputs = SC_LC_PlayerControls.instance;
		movements = GetComponent<SC_LC_PlayerMovements>();
		interactions = GetComponent<SC_LC_PlayerInteractions>();
		health = GetComponent<SC_LC_PlayerHealth>();
	}
}
