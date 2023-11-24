using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSearchState : AIState
{
    public AimSearchState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());

        agent.isStopped = true;
        agent.speed = 0f;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        RotateToTarget();
        if (aiController.RaycastToTarget)
        {
            aiController.SetState(States.Attack);
            return;
        }

        if(DistanceToTarget > aiStatus.range)
        {
            aiController.SetState(States.Trace);
            return;
        }
    }
}
