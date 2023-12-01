using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : Buff
{
    public float increaseDamageRate;

    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }

    public override void ApplyBuff(AIController ai)
    {
        ai.status.damage += Mathf.RoundToInt(ai.status.damage * increaseDamageRate);
        ai.appliedBuffs.Add(this);
    }

    public override void RemoveBuff(AIController ai)
    {
        ai.status.damage -= Mathf.RoundToInt(ai.status.damage * increaseDamageRate);
        ai.removedBuffs.Add(this);
    }
}