using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    public float increasedSpeedRate;

    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        ai.status.damage += Mathf.RoundToInt(ai.status.damage * increasedSpeedRate);
        foreach (var attackInfo in ai.attackInfos)
        {
            if (attackInfo != null)
            {
                attackInfo.damage += Mathf.RoundToInt(attackInfo.damage * increasedSpeedRate);
            }
        }
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.damage -= Mathf.RoundToInt(ai.status.damage * increasedSpeedRate);
        foreach (var attackInfo in ai.attackInfos)
        {
            if (attackInfo != null)
            {
                attackInfo.damage -= Mathf.RoundToInt(attackInfo.damage * increasedSpeedRate);
            }
        }
        ai.removedBuffs.Add(this);
    }
}
