using System.Collections;
using Cinemachine;
using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public readonly StateMachine StateMachine = new();

    public PlayerData data;
    public GameObject model;
    public ParticleSystem movingTrail;
    
    public Rigidbody Rb { get; private set; }
    public PlayerInteraction Interaction { get; private set; }

    #region PlayerInput

    public PlayerInput PlayerInput { get; private set; }

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
        PlayerInput = GetComponent<PlayerInput>();
        Interaction = GetComponentInChildren<PlayerInteraction>();
        StateMachine.ChangeState(new PlayerIdleState(this, StateMachine));

        initModelScaleX = model.transform.localScale.x;
    }

    private void Update()
    {
        StateMachine.ExecuteCurrentState();
    }

    #endregion

    #region ManageInput

    private void ToggleInput(string actionName, bool toggleEnabled)
    {
        if (toggleEnabled)
        {
            PlayerInput.actions[actionName].Enable();
        }
        else
        {
            PlayerInput.actions[actionName].Disable();
        }
    }
    
    private IEnumerator _StopInteractionInputUntil(float delay)
    {
        StopInteractionInput = true;
        yield return new WaitForSeconds(delay);
        StopInteractionInput = false;
    }
    
    public void StopInteractionInputUntil(float delay)
    {
        StartCoroutine(_StopInteractionInputUntil(delay));
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

    #region ETC
    
    private int lastFacing = 1;
    private float initModelScaleX;
    private Coroutine _scaleRoutine;

    public void ChangePlayerFacing(int dir)
    {
        if (lastFacing == dir)
            return;

        lastFacing = dir;
        this.EnsureCoroutineStopped(ref _scaleRoutine);
        _scaleRoutine = StartCoroutine(SmoothReverseScaleX(dir));
    }
    
    public void ChangePlayerFacingImmediately(int dir)
    {
        lastFacing = dir;
        float targetScaleX = initModelScaleX * dir;
        model.transform.localScale = new Vector3(
            targetScaleX,
            model.transform.localScale.y,
            model.transform.localScale.z);
    }

    private IEnumerator SmoothReverseScaleX(int dir)
    {
        float startTime = Time.time;
        float targetScaleX = initModelScaleX * dir;
        float elapsed = 0f;
        
        while (elapsed < data.ChangeFacingDuration)
        {
            elapsed = Time.time - startTime;
            float progress = elapsed / data.ChangeFacingDuration;
            float smoothScaleX = Mathf.Lerp(model.transform.localScale.x, targetScaleX, progress);
            
            model.transform.localScale = new Vector3(
                smoothScaleX,
                model.transform.localScale.y,
                model.transform.localScale.z);

            yield return null;
        }
    }

    #endregion
}