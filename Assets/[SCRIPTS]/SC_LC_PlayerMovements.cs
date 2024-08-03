using UnityEngine;
using UnityEngine.AI;

public class SC_LC_PlayerMovements : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

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

    void Start()
    {
        player = SC_LC_PlayerGlobal.instance;
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
        float xDirection = player.controls.direction.x; //Stores the X position of the joystick to the "xDirection" variable
        float zDirection = player.controls.direction.y; //Stores the Z position of the joystick to the "zDirection" variable

		Vector3 forwardCam = cam.transform.forward; //Stores the forward vector of the camera to the "forwardCam" variable
		Vector3 rightCam = cam.transform.right; //Stores the right vector of the camera to the "rightCam" variable

		forwardCam.y = 0; //Sets the Y axis of the "forwardCam" variable to 0
        rightCam.y = 0; //Sets the Y axis of the "rightCam" variable to 0
		forwardCam = forwardCam.normalized; //Normalizes the "forwardCam" variable to avoid exceeding the max speed when moving on diagonal axis
        rightCam = rightCam.normalized; //Normalizes the "rightCam" variable to avoid exceeding the max speed when moving on diagonal axis

		Vector3 forwardRelative = zDirection * forwardCam; //Multiplies the Z magnitude of the joystick and the forward vector of the camera
        Vector3 rightRelative = xDirection * rightCam; //Multiplies the X magnitude of the joystick and the right vector of the camera

		Vector3 playerMovements = forwardRelative + rightRelative; //Adds the forward and the right vectors to the player movements

        //ROTATION
        float targetAngle = Mathf.Atan2(xDirection, zDirection) * Mathf.Rad2Deg + cam.transform.eulerAngles.y; //J'en sais rien frère go google

        if (player.controls.direction.magnitude >= 0.01f) //If the player is moving
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * currentRotationSpeed), 0); //Rotates the player smoothly between it's current rotation and the previously set rotation

        //CONTROLS
        agent.Move(playerMovements * currentSpeed * Time.deltaTime); //Moves the player's agent according to the movement variable, multiplied by the speed variable, scaled with deltaTime
    }

    void PlayerSpeed()
    {
        if (player.controls.sprintPressed == true) //If the player is sprinting
        {
            currentSpeed = moveSpeed * sprintMultiplier; //Sets the player's movement speed by multiplying the current movement speed with the sprint speed multiplier
            currentRotationSpeed = moveRotationSpeed * sprintMultiplier; //Sets the player's rotation speed by multiplying the current rotation speed with the sprint multiplier
		}

        else //If the player is not sprinting
        {
            currentSpeed = moveSpeed; //Sets the player's current speed to the standard movement speed
            currentRotationSpeed = moveRotationSpeed; //Sets the player's current rotation speed to the standard rotation speed
		}

        //if (player.controls.isAttacking == true)
        //{
        //    currentSpeed = attackMoveSpeed;
        //    currentRotationSpeed = attackRotationSpeed;
        //}
    }

    void PlayerMouseMovements()
    {
        if (player.controls.direction.magnitude >= 0.01f) //If the player is moving
            agent.isStopped = true; //Stop agent to move to destination

		if (player.controls.movementClicked == true)
        {
			agent.isStopped = false;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
            {
                moveTarget.transform.position = hitInfo.point;
                agent.SetDestination(moveTarget.transform.position);
			}
        }
    }
}