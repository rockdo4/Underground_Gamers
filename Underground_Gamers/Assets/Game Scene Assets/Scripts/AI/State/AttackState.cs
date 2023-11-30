using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    public AttackState(AIController aiController) : base(aiController)
    {
    }

    public override void Enter()
    {
        aiController.RefreshDebugAIStatus(this.ToString());
        agent.isStopped = true;
        agent.speed = 0;
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
            return;
        }

        if (DistanceToTarget > aiStatus.range)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }
        if (!aiController.RaycastToTarget)
        {
            RotateToTarget();
        }

        if (aiController.attackInfos[(int)SkillTypes.Base] != null 
            && aiController.target != null && aiController != null 
            && aiController.isOnCoolBaseAttack && aiController.RaycastToTarget)
        {
            aiController.isOnCoolBaseAttack = false;
            aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);
            aiController.SetState(States.Kiting);
            return;
        }
        //Debug.Log("Attack State");
        //RotateToTarget();

        //if (aiController.RaycastToTarget)
        //{
        //    if (aiController.attackInfos[(int)SkillTypes.Base] != null)
        //    {
        //        aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);
        //        aiController.SetState(States.Kiting);
        //    }

        //    //if (aiController.lastAttackTime + aiController.attackCoolTime < Time.time)
        //    //{
        //    //    aiController.lastAttackTime = Time.time;
        //    //    if (aiController.attackInfos[(int)SkillTypes.Base] != null)
        //    //        aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);

        //    //}
        //}
    }
}
