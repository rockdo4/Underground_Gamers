using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExecutionState : AIState
{
    private float reloadTime;
    private float reloadCoolTime = 2f;

    private float executeTime = 0.5f;
    private float executeTimer;

    public MissionExecutionState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        if (!aiStatus.IsLive && aiController.isStun)
        {
            return;
        }
        aiController.RefreshDebugAIStatus(this.ToString());

        aiController.isBattle = false;
        if(agent.enabled)
        {
            Debug.Log($"{aiStatus.name} Enabled");
        }
        else
        {
            Debug.Log($"{aiStatus.name} Disabled");
        }
        agent.isStopped = false;
        aiController.SetMissionTarget(aiController.missionTarget);

        lastDetectTime = Time.time - aiController.detectTime;
        reloadTime = Time.time;
        agent.angularSpeed = aiStatus.reactionSpeed;
        agent.speed = aiStatus.speed;
        //executeTimer = Time.time;
    }

    public override void Exit()
    {
        // 추가
        aiController.isReloading = false;
    }

    public override void Update()
    {
        if (!aiStatus.IsLive)
        {
            return;
        }

        if(aiController.isDefend)
        {
            aiController.SetState(States.Retreat);
            return;
        }

        if (aiController.missionTarget == null)
        {
            // 수정
            BuildingManager buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
            TeamIdentifier identity = aiController.teamIdentity;
            aiController.missionTarget = buildingManager.GetAttackPoint(aiController.currentLine, identity.teamType);
            aiController.SetMissionTarget(aiController.missionTarget);
            //aiController.SetState(States.Idle);
            return;
        }

        if(aiController.battleTarget != null)
        {
            //aiController.SetBattleTarget(aiController.battleTarget);
            aiController.SetState(States.Trace);
            return;
        }

        // 전투 중이 아닌, 작전 수행 중 총알이 모자르다면 장전
        if(reloadTime + reloadCoolTime < Time.time && aiController.currentAmmo < aiController.maxAmmo && !aiController.isReloading)
        {
            reloadTime = Time.time;
            aiController.isReloading = true;
            aiController.lastReloadTime = Time.time;
            aiController.TryReloading();
            //aiController.Reload();
        }

        if (lastDetectTime + aiController.detectTime < Time.time && !aiController.isReloading)
        {
            lastDetectTime = Time.time;

            // 탐색 및 타겟 설정
            SearchTargetInDetectionRange();
            SearchTargetInSector();
        }

        //if (aiController.agent.velocity.magnitude <= 0.01f && executeTime + executeTimer < Time.time)
        //{
        //    executeTimer = Time.time;
        //    aiController.SetMissionTarget(aiController.missionTarget);
        //    return;
        //}
    }
}