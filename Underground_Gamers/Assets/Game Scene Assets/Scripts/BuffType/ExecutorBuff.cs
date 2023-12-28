using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutorBuff : Buff
{
    public AttackExecutor attackExecutor;
    private AttackDefinition prevAttack;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        prevAttack = ai.attackInfos[(int)SkillMode.Base];
        ai.attackInfos[(int)SkillMode.Base] = attackExecutor;
        ai.baseAttackCoolTime = attackExecutor.cooldown;
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.attackInfos[(int)SkillMode.Base] = prevAttack;
        ai.baseAttackCoolTime = prevAttack.cooldown;
        ai.removedBuffs.Add(this);
    }
}
