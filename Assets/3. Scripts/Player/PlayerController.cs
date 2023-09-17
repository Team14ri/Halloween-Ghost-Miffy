using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private StateMachine stateMachine = new();

    public Transform tr;
    public Rigidbody rb;
    
    public Vector2 movementInput;

    private void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        stateMachine.ChangeState(new PlayerIdleState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.ExecuteCurrentState();
    }

    #region UpdateData

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) { }

        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }

    #endregion
}