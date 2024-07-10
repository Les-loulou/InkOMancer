using UnityEngine;
using UnityEngine.AI;

public class SC_LC_PlayerMovements : MonoBehaviour
{
    public static SC_LC_PlayerMovements instance;
    SC_LC_Player player;

    Camera cam;
    NavMeshAgent agent;

    [Header("MOVEMENT")]
    [SerializeField] GameObject moveTarget;
	[SerializeField] LayerMask groundLayer;

	[Space]
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
        #region SINGLETON LEANDRE PARDON LOUIS D AVOIR TOUCHER A TON CODE
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        #endregion

        player = GetComponent<SC_LC_Player>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        PlayerMovements();
        PlayerMouseMovements();

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

    void PlayerMouseMovements()
    {
        if (player.controls.movementClicked == true)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
            {
                moveTarget.transform.position = hitInfo.point;
                agent.SetDestination(moveTarget.transform.position);
			}
        }
    }
}