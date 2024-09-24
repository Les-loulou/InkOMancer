using UnityEngine;

public class SC_LC_PlayerControls : MonoBehaviour
{
    public static SC_LC_PlayerControls instance;
    //SC_GamePauseState pauseState;

    [HideInInspector] public PlayerInput input;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Vector2 focusDirection;


    //public bool isDamaging = false;
    //public bool isAttacking = false;

    [Header("   ---= INPUT CHECKERS =---")]
    [Header("DEBUG")]
    public bool debugPressed;

    [Space]
    public bool createIslandPressed;
    public bool rerollIslandsPressed;

    public bool upIslandPressed;
    public bool rightIslandPressed;
    public bool downIslandPressed;
    public bool leftIslandPressed;

    public bool damageEnemyPressed;

    public bool castSpellPressed;

    public bool changePlayerHealthPressed;
    public bool changePlayerMaxHealthPressed;

    [Header("MOVEMENTS")]
    public bool movementPressed;
    public bool sprintPressed;
    public bool movementClicked;

    [Space]
    [Header("CAMERA")]
    public bool focusPressed;
    public bool rightRotatePressed;
    public bool leftRotatePressed;
    public bool changeFocusPressed;

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
        input.DebugKeys.Enable();

        //input.CharacterControls.Movement.performed += OnMovementPerformed;
        //input.CharacterControls.Movement.canceled += OnMovementCancelled;
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
        input.Interface.Disable();
        input.DebugKeys.Disable();

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
            //movementPressed = true;
        };
        input.CharacterControls.Movement.canceled += ctx =>
        {
            direction = Vector2.zero;
            //movementPressed = false;
        };

        input.CharacterControls.ChangeFocus.performed += ctx =>
        {
            focusDirection = ctx.ReadValue<Vector2>();
        };
        input.CharacterControls.ChangeFocus.canceled += ctx =>
        {
            focusDirection = Vector2.zero;
        };

        //SPRINT
        input.CharacterControls.Sprint.performed += ctx => sprintPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Sprint.canceled += ctx => sprintPressed = ctx.ReadValueAsButton();

        //CAMERA FOCUS
        input.CharacterControls.CameraFocus.performed += ctx => focusPressed = ctx.ReadValueAsButton();
        input.CharacterControls.CameraFocus.canceled += ctx => focusPressed = ctx.ReadValueAsButton();

        //DEBUG
        input.DebugKeys.Debug.performed += ctx => debugPressed = ctx.ReadValueAsButton();
        input.DebugKeys.Debug.canceled += ctx => debugPressed = ctx.ReadValueAsButton();
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
        changeFocusPressed = input.CharacterControls.ChangeFocus.triggered;

        //ATTACK
        attackPressed = input.CharacterControls.Attack.triggered;

        //INVENTORY
        inventoryPressed = input.Interface.Inventory.triggered;

        //FOCUS CAMERA
        //focusPressed = input.CharacterControls.CameraFocus.triggered;
        //}

        if (debugPressed == true)
        {
            createIslandPressed = input.DebugKeys.GenerateIsland.triggered;

            rerollIslandsPressed = input.DebugKeys.RerollIslands.triggered;

            upIslandPressed = input.DebugKeys.GenerateIslandUp.triggered;
            rightIslandPressed = input.DebugKeys.GenerateIslandRight.triggered;
            downIslandPressed = input.DebugKeys.GenerateIslandDown.triggered;
            leftIslandPressed = input.DebugKeys.GenerateIslandLeft.triggered;

            damageEnemyPressed = input.DebugKeys.DamageEnemy.triggered;

            castSpellPressed = input.DebugKeys.CastSpell.triggered;

            changePlayerHealthPressed = input.DebugKeys.ChangePlayerHealth.triggered;
            changePlayerMaxHealthPressed = input.DebugKeys.ChangePlayerMaxHealth.triggered;
        }
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
