using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();

    private void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.ExecuteCurrentState();
    }
}