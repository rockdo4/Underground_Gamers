using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionSpeedBuff : Buff
{
    public float increasedReactionSpeedRate;
    private float prevReactionSpeed;
    public override void UpdateBuff(AIController ai)
    {
        base.UpdateBuff(ai);
    }
    public override void ApplyBuff(AIController ai)
    {
        timer = Time.time;
        prevReactionSpeed = ai.status.reactionSpeed;
        ai.status.reactionSpeed += (prevReactionSpeed * increasedReactionSpeedRate);
        ai.appliedBuffs.Add(this);
    }
    public override void RemoveBuff(AIController ai)
    {
        ai.status.reactionSpeed -= (prevReactionSpeed * increasedReactionSpeedRate);
        ai.removedBuffs.Add(this);
    }
}
