using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PatrolState : AIState
{
    private float patrolRadius = 4f;
    private float patrolTimer;
    private float patrolTime;

    private float detectTimer;
    private float detectTime;

    private float reloadTime;
    private float reloadCoolTime = 2f;

    public PatrolState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());

        agent.isStopped = false;
        agent.speed = aiController.status.speed;
        aiController.battleTarget = null;
        patrolTime = 2f;
        patrolTimer = Time.time - patrolTime;

        detectTime = 1f;
        detectTimer = Time.time - detectTimer;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (aiController.isStun)
            return;

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

        if (patrolTime + patrolTimer < Time.time)
        {
            patrolTimer = Time.time;
            RandomPatrol();
        }

        if (detectTime + detectTimer < Time.time && !aiController.isReloading)
        {
            detectTimer = Time.time;
            SearchTargetInPatrol();
        }
    }

    private void RandomPatrol()
    {
        patrolTimer = Time.time;
        // 타겟과의 Y좌표 통일하여, XZ평면상에서의 계산
        Vector3 targetPos = aiController.missionTarget.position;
        targetPos.y = aiController.transform.position.y;
        Vector3 pointInNavMesh = Vector3.zero;

        if (Utils.RandomPointInNav(targetPos, patrolRadius, 30, out pointInNavMesh))
        {
            agent.SetDestination(pointInNavMesh);
        }
    }
}
