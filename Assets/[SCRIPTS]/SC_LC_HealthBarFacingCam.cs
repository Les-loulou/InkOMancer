using UnityEngine;

public class SC_LC_HealthBarFacingCam : MonoBehaviour
{
	void Update()
	{
		transform.LookAt(Camera.main.transform.position);
	}
}
