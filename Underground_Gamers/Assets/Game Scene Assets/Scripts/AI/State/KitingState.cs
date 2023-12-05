using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitingState : AIState
{
    private float kitingCoolTime = 0.5f;
    private float lastKitingTime;

    public KitingState(AIController aiController) : base(aiController)
    {
        //kitingCoolTime = aiController.kitingInfo.kitingCoolTime;

        agent.speed = aiController.kitingInfo.kitingSpeed;
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        lastKitingTime = Time.time - kitingCoolTime;
        agent.isStopped = false;
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
        if (aiController.target == null)
        {
            aiController.SetState(States.Idle);
        }

        if(aiController.isOnCoolBaseAttack 
            || aiController.isOnCoolOriginalSkill 
            || aiController.isOnCoolGeneralSkill)
        {
            aiController.SetState(States.AimSearch);
            return;
        }


        if (lastKitingTime + kitingCoolTime < Time.time/* && !aiController.isKiting*/)
        {
            //aiController.isKiting = true;
            lastKitingTime = Time.time;
            aiController.UpdateKiting();
        }

        //float dis = Vector3.Distance(aiController.kitingPos, aiTr.position);
        //if (Vector3.Distance(aiController.kitingPos, aiTr.position) < 0.1f)
        //{
        //    //aiController.isKiting = false;
        //    if (DistanceToTarget < aiStatus.range)
        //        aiController.SetState(States.AimSearch);
        //    else
        //        aiController.SetState(States.MissionExecution);

        //    return;
        //}


        //if (aiController.lastAttackTime + aiController.attackCoolTime < Time.time)
        //{
        //    aiController.lastAttackTime = Time.time;
        //    //aiController.SetState(States.Trace);
        //    return;
        //}
    }
}
