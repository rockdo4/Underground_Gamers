using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceState : AIState
{
    public TraceState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());

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
        if(aiController.target == null)
        {
            aiController.SetState(States.MissionExecution);
        }

        if(lastDetectTime + aiController.detectTime < Time.time)
        {
            aiController.SetDestination(aiController.target.position);
        }


        if(DistanceToTarget < aiStatus.range)
        {
            aiController.SetState(States.AimSearch);
            return;
        }

        //if (lastDetectTime + aiController.detectTime < Time.time)
        //{
        //    lastDetectTime = Time.time;

        //    SearchTargetInDetectionRange();
        //    SearchTargetInSector();

        //    agent.SetDestination(aiController.target.position);
        //}

        //if(DistanceToTarget > aiStatus.range)
        //{
        //    aiController.SetDestination(aiController.target.position);
        //}
    }
}
