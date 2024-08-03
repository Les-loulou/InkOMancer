using UnityEngine;

public class SC_LC_PlayerControls : MonoBehaviour
{
    public static SC_LC_PlayerControls instance;
    //SC_GamePauseState pauseState;

    [HideInInspector] public PlayerInput input;

    [HideInInspector] public Vector2 direction;

    //public bool isDamaging = false;
    //public bool isAttacking = false;

    [Header("   ---= INPUT CHECKERS =---")]
    [Header("MOVEMENTS")]
    public bool movementPressed;
    public bool sprintPressed;
    public bool movementClicked;

    [Space]
    [Header("CAMERA")]
    public bool focusPressed;
    public bool rightRotatePressed;
    public bool leftRotatePressed;

    [Space]
    [Header("PLAYER ACTIONS")]
    public bool interactPressed;
    public bool attackPressed;

    [Space]
    [Header("INTERFACE")]
    public bool inventoryPressed;
    public bool pausePressed;

    void OnEnable()
    {
        input.CharacterControls.Enable();
        input.Interface.Enable();

        //input.CharacterControls.Movement.performed += OnMovementPerformed;
        //input.CharacterControls.Movement.canceled += OnMovementCancelled;
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
        input.Interface.Disable();

        //input.CharacterControls.Movement.performed -= OnMovementPerformed;
        //input.CharacterControls.Movement.canceled -= OnMovementCancelled;
    }

    void Awake()
    {
        #region SINGLETON
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        #endregion

        input = new PlayerInput();

        //MOVE
        input.CharacterControls.Movement.performed += ctx =>
        {
            direction = ctx.ReadValue<Vector2>();
            movementPressed = true;
        };
        input.CharacterControls.Movement.canceled += ctx =>
        {
            direction = Vector2.zero;
            movementPressed = false;
        };

        //SPRINT
        input.CharacterControls.Sprint.performed += ctx => sprintPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Sprint.canceled += ctx => sprintPressed = ctx.ReadValueAsButton();

        //CAMERA FOCUS
        input.CharacterControls.CameraFocus.performed += ctx => focusPressed = ctx.ReadValueAsButton();
        input.CharacterControls.CameraFocus.canceled += ctx => focusPressed = ctx.ReadValueAsButton();
    }

    void Start()
    {
        //pauseState = SC_GamePauseState.instance;
    }

    void Update()
    {
        //PAUSE
        pausePressed = input.Interface.Pause.triggered;

		//if (pauseState.isGamePaused == false)
		//{
		    //MOUSE MOVEMENTS
		    movementClicked = input.CharacterControls.MovementsMouse.triggered;

            //INTERACT
            interactPressed = input.CharacterControls.Interact.triggered;

            //CAMERA CONTROLS
            rightRotatePressed = input.CharacterControls.CameraRotateRight.triggered;
            leftRotatePressed = input.CharacterControls.CameraRotateLeft.triggered;

            //ATTACK
            attackPressed = input.CharacterControls.Attack.triggered;

            //INVENTORY
            inventoryPressed = input.Interface.Inventory.triggered;
        //}
    }

    //void OnMovementPerformed(InputAction.CallbackContext value)
    //{
    //    direction = value.ReadValue<Vector2>();
    //}

    //void OnMovementCancelled(InputAction.CallbackContext value)
    //{
    //    direction = Vector2.zero;
    //}
}
