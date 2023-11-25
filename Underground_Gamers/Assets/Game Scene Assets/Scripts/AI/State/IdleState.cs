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
        aiController.RefreshDebugAIStatus(this.ToString());

        //agent.isStopped = true;
        //agent.angularSpeed = 0;
        //agent.speed = 0f;
        // Idle애니메이션 실행

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        //Debug.Log("Idle State");
        if (!aiStatus.IsLive)
        {
            return;
        }
        if (aiController.target == null)
        {
            aiController.target = aiController.point;
            return;
        }

        if(aiController.target != null)
        {
            aiController.SetState(States.MissionExecution);
        }
    }
}