using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalBuff : Buff
{
    public float increasedCriticalRate;
    private float prevCritical;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        prevCritical = ai.status.critical;
        ai.status.critical += (prevCritical * increasedCriticalRate);
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.critical -= (prevCritical * increasedCriticalRate);
        ai.removedBuffs.Add(this);
    }
}
