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
        aiController.RefreshDebugAIStatus(this.ToString());

        if(aiController.point != null)
            aiController.SetTarget(aiController.point);

        lastDetectTime -= aiController.detectTime;
        agent.isStopped = false;
        agent.angularSpeed = aiStatus.reactionSpeed;
        agent.speed = aiStatus.speed;

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        //Debug.Log("Trace State");
        if (!aiStatus.IsLive)
        {
            return;
        }

        if (aiController.target == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            SearchTargetInDetectionRange();
            SearchTargetInSector();

            agent.SetDestination(aiController.target.position);
        }
    }
}