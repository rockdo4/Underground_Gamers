using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitingState : AIState
{
    private float kitingCoolTime;
    private float lastKitingTime;

    public KitingState(AIController aiController) : base(aiController)
    {
        kitingCoolTime = aiController.kitingInfo.kitingCoolTime;
        lastKitingTime = Time.time - kitingCoolTime;
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        agent.isStopped = false;
        agent.speed = aiController.kitingInfo.kitingSpeed;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if(Vector3.Distance(aiController.kitingPos, aiTr.position) < 0.1f && aiController.isKiting)
        {
            aiController.isKiting = false;
            aiController.SetState(States.Attack);
            return;
        }

        if (lastKitingTime + kitingCoolTime < Time.time)
        {
            lastKitingTime = Time.time;
            aiController.isKiting = true;
            aiController.UpdateKiting();
        }



        //if (aiController.lastAttackTime + aiController.attackCoolTime < Time.time)
        //{
        //    aiController.lastAttackTime = Time.time;
        //    //aiController.SetState(States.Trace);
        //    return;
        //}
    }
}
