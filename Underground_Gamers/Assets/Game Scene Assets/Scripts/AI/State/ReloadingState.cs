using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingState : AIState
{
    private float kitingCoolTime = 0.5f;
    private float lastKitingTime;
    public ReloadingState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        agent.isStopped = false;
        agent.speed = aiController.reloadingKitingInfo.kitingSpeed;
        kitingCoolTime = aiController.reloadingKitingInfo.kitingCoolTime;
        lastDetectTime = Time.time - kitingCoolTime;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if(!aiController.isReloading)
        {
            aiController.SetState(States.AimSearch);
            return;
        }

        if (lastDetectTime + kitingCoolTime < Time.time)
        {
            lastDetectTime = Time.time;
            aiController.UpdateReloadKiting();
        }
    }
}
