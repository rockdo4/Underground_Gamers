using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExecutionState : AIState
{
    private float reloadTime;
    private float reloadCoolTime = 3f;

    public MissionExecutionState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        if (!aiStatus.IsLive)
        {
            return;
        }
        aiController.RefreshDebugAIStatus(this.ToString());

        //if(aiController.point != null && aiController.missionTarget == null)
        //    aiController.SetMissionTarget(aiController.point);

        aiController.isBattle = false;

        aiController.SetMissionTarget(aiController.point);

        lastDetectTime = Time.time - aiController.detectTime;
        reloadTime = Time.time;
        agent.isStopped = false;
        agent.speed = aiStatus.speed;
        agent.angularSpeed = aiStatus.reactionSpeed;

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

        if (aiController.missionTarget == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        if(aiController.battleTarget != null)
        {
            aiController.SetBattleTarget(aiController.battleTarget);
            aiController.SetState(States.Trace);
            return;
        }

        // 전투 중이 아닌, 작전 수행 중 총알이 모자르다면 장전
        if(reloadTime + reloadCoolTime < Time.time && aiController.currentAmmo < aiController.maxAmmo)
        {
            reloadTime = Time.time;
            aiController.Reload();
        }

        // 수정 필요, 포인트 변경점 필요 / 넥서스, 타워 변경
        if(Vector3.Distance(aiTr.position, aiController.missionTarget.position) < 2f)
        {
            // currentPoint로, 현재 포인트 저장. List<Transform>을 이용하고, EventBus로 current지점 변경
            // 주의 사항, 탑라인 바텀라인 구분
            aiController.SetMissionTarget(aiController.point);
        }


        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            // 탐색 및 타겟 설정
            SearchTargetInDetectionRange();
            SearchTargetInSector();

            aiController.SetDestination(aiController.missionTarget);
        }
    }
}