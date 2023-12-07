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
        if(aiController.battleTarget != null)
        {
            if (!aiController.battleTarget.GetComponent<TeamIdentifier>().isBuilding)
                aiController.isBattle = true;
            else
                aiController.isBattle = false;
        }

        lastDetectTime = Time.time - aiController.detectTime;

        agent.isStopped = false;
        agent.angularSpeed = aiStatus.reactionSpeed;
        agent.speed = aiController.kitingInfo.kitingSpeed;
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



        if (aiController.battleTarget == null)
        {
            aiController.SetState(States.MissionExecution);
        }

        if(lastDetectTime + aiController.detectTime < Time.time && aiController.battleTarget != null)
        {
            aiController.SetDestination(aiController.battleTarget.position);
        }

        if(aiController.DistanceToBattleTarget < aiStatus.range)
        {
            aiController.SetState(States.AimSearch);
            return;
        }

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            // Å½»ö ¹× Å¸°Ù ¼³Á¤
            SearchTargetInDetectionRange();
            SearchTargetInSector();

            agent.SetDestination(aiController.battleTarget.position);
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
