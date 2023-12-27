using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidAttackBuff : Buff
{
    public int invalidAttackCount;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        ai.isInvalid = true;
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.removedBuffs.Add(this);
        ai.isInvalid = false;
    }
}
