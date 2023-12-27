using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBuff : Buff
{
    public float increasedAttackSpeedRate;
    private float prevAttackSpeed;

    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        // 공속이 줄어야 버프
        timer = Time.time;
        prevAttackSpeed = ai.status.cooldown;
        ai.originalSkillCoolTime -= (prevAttackSpeed * increasedAttackSpeedRate); 
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.originalSkillCoolTime += (prevAttackSpeed * increasedAttackSpeedRate);
        ai.removedBuffs.Add(this);
    }
}
