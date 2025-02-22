using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SC_LC_PlayerControls : MonoBehaviour
{
    public static SC_LC_PlayerControls instance;

    [HideInInspector] public PlayerInput input;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Vector2 focusDirection;
    [HideInInspector] public Vector2 newIslandDirection;

    [Header("   ---= INPUT CHECKERS =---")]
    [Header("DEBUG")]
    public bool debugPressed;

    [Space]
    public bool generateRandomIslandPressed;
    public bool generateIslandPressed;
    public bool rerollIslandsPressed;

    public bool damageEnemyPressed;

    public bool castSpellPressed;

    public bool changePlayerHealthPressed;
    public bool changePlayerMaxHealthPressed;

    [Header("MOVEMENTS")]
    public bool sprintPressed;
    public bool movementClicked;

    [Space]
    [Header("CAMERA")]
    public bool focusPressed;
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

        //InputVector2(input.CharacterControls.Movement, direction);
        //InputVector2(input.CharacterControls.ChangeFocus, focusDirection);
        //InputVector2(input.DebugKeys.GenerateIsland, newIslandDirection);
        
        //MOVE
        input.CharacterControls.Movement.performed += ctx => direction = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.canceled += ctx => direction = Vector2.zero;

        //SPRINT
        input.CharacterControls.Sprint.performed += ctx => sprintPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Sprint.canceled += ctx => sprintPressed = ctx.ReadValueAsButton();
        
        //FOCUS ENEMIES
        input.CharacterControls.ChangeFocus.performed += ctx => focusDirection = ctx.ReadValue<Vector2>();
		input.CharacterControls.ChangeFocus.canceled += ctx => focusDirection = Vector2.zero;

        //DEBUG GENERAL
        input.DebugKeys.Debug.performed += ctx => debugPressed = ctx.ReadValueAsButton();
        input.DebugKeys.Debug.canceled += ctx => debugPressed = ctx.ReadValueAsButton();

        //DEBUG GENERATE ISLANDS
		input.DebugKeys.GenerateIsland.performed += ctx => newIslandDirection = ctx.ReadValue<Vector2>();
        input.DebugKeys.GenerateIsland.canceled += ctx => newIslandDirection = Vector2.zero;
    }

 //   void InputVector2(InputAction _input, Vector2 _direction)
 //   {
	//	_input.performed += ctx => _direction = ctx.ReadValue<Vector2>();
	//	_input.canceled += ctx => _direction = Vector2.zero;
	//}

    void Update()
    {
        //PAUSE
        pausePressed = input.Interface.Pause.triggered;

		//MOUSE MOVEMENTS
		movementClicked = input.CharacterControls.MovementsMouse.triggered;

        //INTERACT
        interactPressed = input.CharacterControls.Interact.triggered;

        //CAMERA CONTROLS
        changeFocusPressed = input.CharacterControls.ChangeFocus.triggered;

        //ATTACK
        attackPressed = input.CharacterControls.Attack.triggered;

        //INVENTORY
        inventoryPressed = input.Interface.Inventory.triggered;

        if (debugPressed == true)
        {
            generateRandomIslandPressed = input.DebugKeys.GenerateRandomIsland.triggered;

			generateIslandPressed = input.DebugKeys.GenerateIsland.triggered;

			rerollIslandsPressed = input.DebugKeys.RerollIslands.triggered;
            
            damageEnemyPressed = input.DebugKeys.DamageEnemy.triggered;

            castSpellPressed = input.DebugKeys.CastSpell.triggered;

            changePlayerHealthPressed = input.DebugKeys.ChangePlayerHealth.triggered;
            changePlayerMaxHealthPressed = input.DebugKeys.ChangePlayerMaxHealth.triggered;
        }
    }
}
