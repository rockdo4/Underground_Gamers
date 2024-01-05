using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : AIState
{
    private float reloadTime;
    private float reloadCoolTime = 2f;

    private float reTagetTimer;
    private float reTargetTime = 0.5f;
    private bool isDisabledNav = false;
    public RetreatState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        if (!aiController.status.IsLive && aiController.isStun)
            return;
        aiController.RefreshDebugAIStatus(this.ToString());

        agent.isStopped = false;
        agent.speed = aiController.status.speed;
        aiController.battleTarget = null;
        //Transform defendTarget = aiController.buildingManager.GetDefendPoint(aiController.currentLine, aiController.teamIdentity.teamType).GetComponent<Building>().defendPoint;
        //aiController.SetMissionTarget(defendTarget);
        isDisabledNav = false;
        if (!agent.enabled)
        {
            isDisabledNav = true;
            reTagetTimer = Time.time;
        }
        aiController.SetMissionTarget(aiController.missionTarget);
        reloadTime = Time.time;
    }

    public override void Exit()
    {
        aiController.StopMove();
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
        aiController.stunTime = 0f;
    }

    public override void Update()
    {
        if (aiController.isStun)
            return;

        if(reTagetTimer + reTargetTime < Time.time && isDisabledNav)
        {
            isDisabledNav = false;
            aiController.SetMissionTarget(aiController.missionTarget);
        }

        if (aiController.isAttack)
        {
            aiController.SetMissionTarget(aiController.buildingManager.GetAttackPoint(aiController.currentLine, aiController.teamIdentity.teamType));
            aiController.SetState(States.MissionExecution);
            return;
        }

        if (reloadTime + reloadCoolTime < Time.time && aiController.currentAmmo < aiController.maxAmmo && !aiController.isReloading)
        {
            reloadTime = Time.time;
            aiController.isReloading = true;
            aiController.lastReloadTime = Time.time;
            aiController.TryReloading();
            //aiController.Reload();
        }

        if (aiController.DistanceToMissionTarget < 3f && !aiController.isReloading)
        {
            aiController.SetState(States.Patrol);
            return;
        }
    }
}
