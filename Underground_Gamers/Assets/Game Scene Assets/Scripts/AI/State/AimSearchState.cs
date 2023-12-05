using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSearchState : AIState
{
    public AimSearchState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        agent.isStopped = true;
        agent.speed = 0f;

        lastDetectTime = Time.time - aiController.detectTime;
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
        RotateToTarget();




        if (aiController.target == null)
            aiController.SetState(States.Idle);

        if (aiController.RaycastToTarget)
        {
            aiController.SetState(States.Attack);
            return;
        }

        if(aiController.DistanceToTarget > aiStatus.range)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }

        //if (lastDetectTime + aiController.detectTime < Time.time)
        //{
        //    lastDetectTime = Time.time;

        //    // Å½»ö ¹× Å¸°Ù ¼³Á¤
        //    SearchTargetInDetectionRange();
        //    SearchTargetInSector();

        //    agent.SetDestination(aiController.target.position);
        //}
    }
}
