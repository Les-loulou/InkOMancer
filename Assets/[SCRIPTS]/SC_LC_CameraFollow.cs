using UnityEngine;

public class SC_LC_CameraFollow : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [SerializeField] float smoothSpeed;

    private void Start()
    {
        target = SC_LC_PlayerMovements.instance.transform;
        transform.position = target.position;
    }

    void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
