using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSearchState : AIState
{
    private float traceTimer;
    private float traceTime;
    public AimSearchState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        agent.isStopped = true;
        agent.speed = 0f;

        lastDetectTime = Time.time - aiController.detectTime;
        traceTime = 1f;
        traceTimer = Time.time - traceTime;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (!aiStatus.IsLive && aiController.isStun)
        {
            return;
        }
        RotateToTarget();

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            // Å½»ö ¹× Å¸°Ù ¼³Á¤
            SearchTargetInDetectionRange();
            SearchTargetInSector();
        }
    
        if (aiController.battleTarget == null)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }

        if (aiController.RaycastToTarget)
        {
            aiController.SetState(States.Attack);
            return;
        }

        if(aiController.DistanceToBattleTarget < aiStatus.range && traceTimer + traceTime < Time.time)
        {
            aiController.SetDestination(aiController.battleTarget);
        }

        if(aiController.DistanceToBattleTarget > aiStatus.range)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }
    }
}
