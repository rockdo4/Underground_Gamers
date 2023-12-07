using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    public float increasedSpeedRate;
    private float prevSpeed;

    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        prevSpeed = ai.status.speed;
        ai.status.speed += (prevSpeed * increasedSpeedRate);
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.speed -= (prevSpeed * increasedSpeedRate);
        ai.removedBuffs.Add(this);
    }
}
