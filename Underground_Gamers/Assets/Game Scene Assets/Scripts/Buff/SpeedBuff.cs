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
        ai.status.speed += Mathf.RoundToInt(ai.status.speed * increasedSpeedRate);
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.speed -= Mathf.RoundToInt(ai.status.speed * increasedSpeedRate);
        ai.removedBuffs.Add(this);
    }
}
