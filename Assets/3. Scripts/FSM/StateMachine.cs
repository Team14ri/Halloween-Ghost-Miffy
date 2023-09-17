using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void ExecuteCurrentState()
    {
        CurrentState?.Execute();
    }
}