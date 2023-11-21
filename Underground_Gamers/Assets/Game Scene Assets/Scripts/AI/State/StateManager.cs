using UnityEngine;

public class StateManager
{
    private BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
