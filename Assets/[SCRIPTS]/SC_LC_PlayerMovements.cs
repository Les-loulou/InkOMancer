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
    [SerializeField] LayerMask enemyLayer;

    [Space]
	public float currentSpeed;
    public float moveSpeed;
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

        moveSpeed = player.playerStats.movementSpeed;
    }

    void Update()
    {
        PlayerMovements();
        PlayerMouseMovements();

		PlayerSpeed(1);
    }

    void PlayerMovements()
    {
        //POSITION
        float xDirection = player.inputs.direction.x; //Stores the X position of the joystick to the "xDirection" variable
        float zDirection = player.inputs.direction.y; //Stores the Z position of the joystick to the "zDirection" variable

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
        float targetAngle = Mathf.Atan2(xDirection, zDirection) * Mathf.Rad2Deg + cam.transform.eulerAngles.y; //J'en sais rien fr�re go google

        if (player.inputs.direction.magnitude >= 0.01f) //If the player is moving
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * currentRotationSpeed), 0); //Rotates the player smoothly between it's current rotation and the previously set rotation

        //CONTROLS
        agent.Move(playerMovements * currentSpeed * Time.deltaTime); //Moves the player's agent according to the movement variable, multiplied by the speed variable, scaled with deltaTime
    }

    public void PlayerSpeed(float modifier)
    {
        if (player.inputs.sprintPressed == true) //If the player is sprinting
        {
            currentSpeed = moveSpeed * sprintMultiplier * modifier; //Sets the player's movement speed by multiplying the current movement speed with the sprint speed multiplier
            currentRotationSpeed = moveRotationSpeed * sprintMultiplier * modifier; //Sets the player's rotation speed by multiplying the current rotation speed with the sprint multiplier
		}

        else //If the player is not sprinting
        {
            currentSpeed = moveSpeed * modifier; //Sets the player's current speed to the standard movement speed
            currentRotationSpeed = moveRotationSpeed * modifier; //Sets the player's current rotation speed to the standard rotation speed
		}

        agent.speed = currentSpeed;

        //if (player.controls.isAttacking == true)
        //{
        //    currentSpeed = attackMoveSpeed;
        //    currentRotationSpeed = attackRotationSpeed;
        //}
    }


    void PlayerMouseMovements()
    {
        if (player.inputs.direction.magnitude >= 0.01f) //If the player is moving
            agent.isStopped = true; //Stop agent to move to destination

		if (player.inputs.movementClicked == true)
        {
			agent.isStopped = false;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
            {

                moveTarget.transform.position = hitInfo.point;
                agent.SetDestination(moveTarget.transform.position);
			}

            if (Physics.Raycast(ray, out RaycastHit hitInfoEnemy, Mathf.Infinity, enemyLayer))
            {
                print("enemy");
                moveTarget.transform.position = hitInfoEnemy.point;
                agent.SetDestination(moveTarget.transform.position);
            }
        }
    }
}