using UnityEngine;

public class SC_LC_HealthBarFacingCam : MonoBehaviour
{
	void FixedUpdate()
	{
		transform.LookAt(Camera.main.transform.position);
	}
}
