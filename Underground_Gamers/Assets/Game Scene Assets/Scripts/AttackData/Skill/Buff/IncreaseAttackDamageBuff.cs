using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackDamageBuff.Asset", menuName = "BuffSkill/IncreaseAttackDamageBuff")]
public class IncreaseAttackDamageBuff : BuffSkill
{
    public float increaseAttackDamageRate;
    public float increaseReactionSpeedRate;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController buffAi = type switch
        {
            BuffType.Self => attacker.GetComponent<AIController>(),
            BuffType.Other => defender.GetComponent<AIController>(),
            _ => attacker.GetComponent<AIController>()
        };
        AttackBuff attackBuff = new AttackBuff();
        attackBuff.duration = duration;
        attackBuff.increaseDamageRate = increaseAttackDamageRate;

        ReactionSpeedBuff reactionSpeedBuff = new ReactionSpeedBuff();
        reactionSpeedBuff.duration = duration;
        reactionSpeedBuff.increaseReactionSpeedRate = increaseReactionSpeedRate;

        attackBuff.ApplyBuff(buffAi);
        reactionSpeedBuff.ApplyBuff(buffAi);
    }
}
