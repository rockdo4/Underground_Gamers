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
        agent.angularSpeed = aiStatus.reactionSpeed;

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
        if (!aiStatus.IsLive)
        {
            return;
        }

        if(aiController.battleTarget != null)
        {
            aiController.SetBattleTarget(aiController.battleTarget);
            aiController.SetState(States.Trace);
            return;
        }

        if (aiController.missionTarget == null)
        {
            aiController.SetMissionTarget(aiController.missionTarget);
            aiController.SetState(States.MissionExecution);
            return;
        }

        if(aiController.missionTarget != null)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }
    }
}