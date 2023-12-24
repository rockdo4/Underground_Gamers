using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : AIState
{
    private float timer;
    private float time = 1f;
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
        aiController.battleTarget = null;
        timer = Time.time - time;
        //Transform defendTarget = aiController.buildingManager.GetDefendPoint(aiController.currentLine, aiController.teamIdentity.teamType).GetComponent<Building>().defendPoint;
        //aiController.SetMissionTarget(defendTarget);
        aiController.SetMissionTarget(aiController.missionTarget);
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
        //if (timer + time > Time.time)
        //{
        //    timer = Time.time;
        //    if (aiController.isDefend)
        //    {
        //        aiController.RefreshBuilding();
        //        aiController.SetMissionTarget(aiController.missionTarget);
        //    }
        //}

        if(aiController.DistanceToMissionTarget < 3f)
        {
            aiController.SetState(States.Patrol);
            return;
        }
    }
}
