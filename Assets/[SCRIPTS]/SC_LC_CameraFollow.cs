using UnityEngine;

public class SC_LC_CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] float smoothSpeed;

    private void Start()
    {
        target = SC_LC_PlayerGlobal.instance.transform;

        transform.position = target.transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime); //Smoothly follows the player's movements
	}
}
