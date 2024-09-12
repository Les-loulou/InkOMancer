using UnityEngine;

public class SC_LQ_VisualTarget : MonoBehaviour
{

    public SC_LQ_Player_AimStateSecond aimState;

    private MeshRenderer mesh;
    [SerializeField] bool active;

    [SerializeField] LayerMask ground;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();

        aimState.OnEnterState += Active;
        aimState.OnExitState += Desactive;
    }

    public void Active()
    {
        active = true;
    }

    public void Desactive()
    {
        active = false;
    }


    void FixedUpdate()
    {

        mesh.enabled = aimState.target != null && active == true ? true : false;

        if (active == false || aimState.target == null)
        {
            return;
        }

        
        transform.position = aimState.target.position;

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground);
        
        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }
}
