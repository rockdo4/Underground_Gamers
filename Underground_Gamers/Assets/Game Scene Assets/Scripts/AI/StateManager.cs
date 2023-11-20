using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        if (currentState == null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }
}
