using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    private float lastAttackTime;
    private float attackCoolTime;
    public AttackState(AIController aiController) : base(aiController)
    {
        attackCoolTime = aiController.attackInfos[(int)SkillTypes.Base].cooldown;
    }

    public override void Enter()
    {
        agent.speed = 0;
        lastAttackTime -= attackCoolTime;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        //Debug.Log("Attack State");
        RotateToTarget();
        if (aiController.target == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        var dirToTarget = aiController.target.position - aiTr.position;
        dirToTarget.Normalize();

        if (aiController.RaycastToTarget)
        {
            if (lastAttackTime + attackCoolTime < Time.time)
            {
                lastAttackTime = Time.time;
                if (aiController.attackInfos[(int)SkillTypes.Base] != null)
                    aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);

            }
        }
    }
}
