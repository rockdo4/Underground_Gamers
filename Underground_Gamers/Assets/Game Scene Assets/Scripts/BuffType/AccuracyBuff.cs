using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyBuff : Buff
{
    public float increasedAccuracyRate;
    private float prevAccuracy;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        prevAccuracy = ai.status.accuracyRate;
        ai.status.accuracyRate += (prevAccuracy * increasedAccuracyRate);
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.accuracyRate -= (prevAccuracy * increasedAccuracyRate);
        ai.removedBuffs.Add(this);
    }
}
