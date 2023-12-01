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
        ai.status.reactionSpeed += Mathf.RoundToInt(ai.status.reactionSpeed * increaseReactionSpeedRate);
        ai.appliedBuffs.Add(this);
    }
    public override void RemoveBuff(AIController ai)
    {
        ai.status.reactionSpeed -= Mathf.RoundToInt(ai.status.reactionSpeed * increaseReactionSpeedRate);
        ai.removedBuffs.Add(this);
    }
}
