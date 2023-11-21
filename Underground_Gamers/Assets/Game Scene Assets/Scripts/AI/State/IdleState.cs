using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIState
{
    public IdleState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        agent.isStopped = true;
        agent.speed = 0f;
        // Idle애니메이션 실행

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (target == null)
        {
            target = aiController.point;
            return;
        }

        if(target != null)
        {
            aiController.SetState(States.Trace);
        }
    }
}