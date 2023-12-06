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

        if (aiController.DistanceToTarget > aiStatus.range)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }
        if (!aiController.RaycastToTarget)
        {
            RotateToTarget();
        }

        AttackByOriginSkill();
        AttackByBase();
    }
    private void AttackByOriginSkill()
    {
        if (aiController.attackInfos[(int)SkillTypes.Original] != null
            && aiController.target != null && aiController != null
            && aiController.isOnCoolOriginalSkill && aiController.RaycastToTarget)
        {
            aiController.isOnCoolOriginalSkill = false;
            aiController.attackInfos[(int)SkillTypes.Original].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);
        }
    }

    private void AttackByBase()
    {
        if (aiController.attackInfos[(int)SkillTypes.Base] != null
            && aiController.target != null && aiController != null
            && aiController.isOnCoolBaseAttack && aiController.RaycastToTarget)
        {
            aiController.isOnCoolBaseAttack = false;
            aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.target.gameObject);
            UseAmmo();
            if(aiController.isReloading)
            {
                aiController.TryReloading();
                aiController.SetState(States.Reloading);
                return;
            }
            aiController.SetState(States.Kiting);
            return;
        }
    }

    private void UseAmmo()
    {
        aiController.currentAmmo--;
        aiController.currentAmmo = Mathf.Max(0, aiController.currentAmmo);
        if (aiController.currentAmmo <= 0)
        {
            aiController.isReloading = true;
            aiController.lastReloadTime = Time.time;
        }
    }
}
