using DG.Tweening.Core.Easing;
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

        if (aiController.battleTarget == null)
        {
            aiController.SetState(States.MissionExecution);
            return;
        }

        if (aiController.DistanceToBattleTarget > aiStatus.range)
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
        // 이 곳에서 스킬 누를 수 있도록, 그리고 유효한지 확인 / 사거리 안에 적이 있는지
        if (aiController.attackInfos[(int)SkillTypes.Original] != null
            && aiController.battleTarget != null && aiController != null
            && aiController.isOnCoolOriginalSkill && aiController.RaycastToTarget
            && (aiController.gameManager.skillModeButton.IsAutoMode || (!aiController.gameManager.skillModeButton.IsAutoMode && aiController.isPrior)))
        {
            aiController.isOnCoolOriginalSkill = false;
            if (aiController.aiCommandInfo != null)
                aiController.gameManager.skillCoolTimeManager.StartSkillCooldown(aiController, Time.time);

            aiController.isPrior = false;
            aiController.gameManager.skillModeButton.SetPriorSkill(aiController.isPrior);
            aiController.attackInfos[(int)SkillTypes.Original].ExecuteAttack(aiController.gameObject, aiController.battleTarget.gameObject);
        }
    }

    private void AttackByBase()
    {
        if (aiController.attackInfos[(int)SkillTypes.Base] != null
            && aiController.battleTarget != null && aiController != null
            && aiController.isOnCoolBaseAttack && aiController.RaycastToTarget)
        {
            aiController.isOnCoolBaseAttack = false;
            aiController.attackInfos[(int)SkillTypes.Base].ExecuteAttack(aiController.gameObject, aiController.battleTarget.gameObject);
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
