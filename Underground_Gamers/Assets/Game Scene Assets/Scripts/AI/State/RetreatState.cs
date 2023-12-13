using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : AIState
{
    public RetreatState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        if (!aiController.status.IsLive)
            return;
        aiController.RefreshDebugAIStatus(this.ToString());

        agent.isStopped = false;
        agent.speed = aiController.status.speed;
        Transform defendTarget = aiController.buildingManager.GetDefendPoint(aiController.currentLine, aiController.teamIdentity.teamType);
        aiController.battleTarget = null;
        aiController.SetMissionTarget(defendTarget);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if(aiController.isAttack)
        {
            aiController.SetMissionTarget(aiController.buildingManager.GetAttackPoint(aiController.currentLine, aiController.teamIdentity.teamType));
            aiController.SetState(States.MissionExecution);
            return;
        }

        if(aiController.DistanceToMissionTarget < 3f)
        {
            aiController.SetState(States.Patrol);
            return;
        }
    }
}
