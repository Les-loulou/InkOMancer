using UnityEngine;

public class SC_LQ_ActiveOutline : MonoBehaviour
{

    [SerializeField] LayerMask allowingLayer;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfoEnemy, Mathf.Infinity, allowingLayer))
        {

        }
    }
}
