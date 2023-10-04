using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public StateMachine stateMachine = new();
    
    public Rigidbody Rb { get; private set; }

    #region PlayerInput

    private PlayerInput playerInput { get; set; }

    public Vector2 MovementInput { get; private set; }

    private bool _stopMovementInput;
    public bool StopMovementInput
    {
        get => _stopMovementInput;
        set
        {
            _stopMovementInput = value;
            ToggleInput("Move", !value);
        }
    }

    public bool InteractionInput { get; set; }

    private bool _stopInteractionInput;
    public bool StopInteractionInput
    {
        get => _stopInteractionInput;
        set
        {
            _stopInteractionInput = value;
            ToggleInput("Interaction", !value);
        }
    }

    #endregion

    #region Unity.Event
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        stateMachine.ChangeState(new PlayerIdleState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.ExecuteCurrentState();
    }

    #endregion

    #region ManageInput

    private void ToggleInput(string actionName, bool toggleEnabled)
    {
        if (toggleEnabled)
        {
            playerInput.actions[actionName].Enable();
        }
        else
        {
            playerInput.actions[actionName].Disable();
        }
    }

    #endregion

    #region UpdateData

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) { }

        if (context.performed)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
    }
    
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractionInput = true;
        }

        if (context.performed) { }

        if (context.canceled)
        {
            InteractionInput = false;
        }
    }

    #endregion
}