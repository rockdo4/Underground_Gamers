using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExecutionState : AIState
{
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

        if(aiController.point != null && aiController.target == null)
            aiController.SetTarget(aiController.point);

        lastDetectTime = Time.time - aiController.detectTime;
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

        if (aiController.target == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        // 수정 필요, 포인트 변경점 필요
        if(Vector3.Distance(aiTr.position, aiController.target.position) < 2f)
        {
            aiController.SetTarget(aiController.point);
        }

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            // 탐색 및 타겟 설정
            SearchTargetInDetectionRange();
            SearchTargetInSector();

            agent.SetDestination(aiController.target.position);
        }
    }
}