using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : AIState
{
    public StunState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.speed = 0;
        }
        aiController.isStun = true;
    }

    public override void Exit()
    {
        Debug.Log("Stun Exit~~~~~~~~~~");

    }

    public override void Update()
    {
        if(aiController.stunTime + aiController.stunTimer < Time.time)
        {
            //if (!aiController.rb.isKinematic)
            //{
            //    aiController.rb.isKinematic = true;
            //}

            if (aiController.isAttack)
                aiController.SetState(States.MissionExecution);
            if (aiController.isDefend)
                aiController.SetState(States.Retreat);
        }
    }
}
