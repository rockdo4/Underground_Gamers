using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        Vector3 textPos = attacker.transform.position;
        textPos.y += offsetText;
        TextMeshPro text = Instantiate(scrollingBuffText, textPos, Quaternion.identity);
        text.text = "ATK UP!";
        text.color = new Color(1, 0.6f, 0);

        AttackBuff attackBuff = new AttackBuff();
        attackBuff.duration = duration;
        attackBuff.increaseDamageRate = increaseAttackDamageRate;

        ReactionSpeedBuff reactionSpeedBuff = new ReactionSpeedBuff();
        reactionSpeedBuff.duration = duration;
        reactionSpeedBuff.increaseReactionSpeedRate = increaseReactionSpeedRate;

        attackBuff.ApplyBuff(buffAi);
        reactionSpeedBuff.ApplyBuff(buffAi);

        GameObject buffEffect = Instantiate(effectPrefab, buffAi.transform);
        Destroy(buffEffect, duration);
    }
}
