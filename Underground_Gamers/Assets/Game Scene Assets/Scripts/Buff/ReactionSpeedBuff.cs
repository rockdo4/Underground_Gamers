using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionSpeedBuff : Buff
{
    public float increaseReactionSpeedRate;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        ai.status.damage += Mathf.RoundToInt(ai.status.damage * increaseReactionSpeedRate);
        foreach (var attackInfo in ai.attackInfos)
        {
            if (attackInfo != null)
            {
                attackInfo.damage += Mathf.RoundToInt(attackInfo.damage * increaseReactionSpeedRate);
            }
        }
        ai.appliedBuffs.Add(this);
    }
    public override void RemoveBuff(AIController ai)
    {
        ai.status.damage -= Mathf.RoundToInt(ai.status.damage * increaseReactionSpeedRate);
        foreach (var attackInfo in ai.attackInfos)
        {
            if (attackInfo != null)
            {
                attackInfo.damage -= Mathf.RoundToInt(attackInfo.damage * increaseReactionSpeedRate);
            }
        }
        ai.removedBuffs.Add(this);
    }
}
