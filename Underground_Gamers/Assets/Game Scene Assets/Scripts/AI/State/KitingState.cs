using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitingState : AIState
{
    private float kitingCoolTime = 0.5f;
    private float lastKitingTime;

    public KitingState(AIController aiController) : base(aiController)
    {
        kitingCoolTime = aiController.kitingInfo.kitingCoolTime;

        //agent.speed = aiController.kitingInfo.kitingSpeed;
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        lastKitingTime = Time.time - kitingCoolTime;
        agent.isStopped = false;
        agent.speed = aiController.status.speed;
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
        if (aiController.battleTarget == null)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }

        if (aiController.isOnCoolBaseAttack
            || aiController.isOnCoolOriginalSkill
            || aiController.isOnCoolGeneralSkill)
        {
            aiController.SetState(States.AimSearch);
            return;
        }


        if (lastKitingTime + kitingCoolTime < Time.time)
        {
            lastKitingTime = Time.time;
            aiController.UpdateKiting();
        }
    }
}
