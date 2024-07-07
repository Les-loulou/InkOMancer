using UnityEngine;
using UnityEngine.AI;

public class SC_LC_PlayerMovements : MonoBehaviour
{
	SC_LC_Player player;

    Camera cam;
    NavMeshAgent agent;

    [Header("MOVEMENT")]
    public float currentSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultiplier;

    float smoothVelocity;

    [Space]
    [Header("ROTATION")]
    public float currentRotationSpeed;
    [SerializeField] float moveRotationSpeed;

    [Space]
    [Header("ATTACK MOVE SPEED")]
    [SerializeField] float attackMoveSpeed;
    [SerializeField] float attackRotationSpeed;

    private void Awake()
    {
        player = GetComponent<SC_LC_Player>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        PlayerMovements();

        PlayerSpeed();
    }

    void PlayerMovements()
    {
        //POSITION
        float xdirection = player.controls.direction.x;
        float zdirection = player.controls.direction.y;

        Vector3 forwardCam = cam.transform.forward;
        Vector3 rightCam = cam.transform.right;

        forwardCam.y = 0;
        rightCam.y = 0;
        forwardCam = forwardCam.normalized;
        rightCam = rightCam.normalized;

        Vector3 forwardRelative = zdirection * forwardCam;
        Vector3 rightRelative = xdirection * rightCam;

        Vector3 playerMovements = forwardRelative + rightRelative;

        //ROTATION
        float targetAngle = Mathf.Atan2(xdirection, zdirection) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        if (player.controls.direction.magnitude >= 0.01f)
        {
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * currentRotationSpeed), 0);
        }

        //CONTROLS
        agent.Move(playerMovements * currentSpeed * Time.deltaTime);
    }

    void PlayerSpeed()
    {
        if (player.controls.sprintPressed == true)
        {
            currentSpeed = moveSpeed * sprintMultiplier;
            currentRotationSpeed = moveRotationSpeed * sprintMultiplier;
        }

        else
        {
            currentSpeed = moveSpeed;
            currentRotationSpeed = moveRotationSpeed;
        }

        if (player.controls.isAttacking == true)
        {
            currentSpeed = attackMoveSpeed;
            currentRotationSpeed = attackRotationSpeed;
        }
    }
}