using UnityEngine;

public class SC_LC_CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] float smoothSpeed;

    private void Start()
    {
        transform.position = target.position;
    }

    void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
