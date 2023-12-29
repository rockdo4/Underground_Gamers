using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackDamageBuff.Asset", menuName = "BuffSkill/IncreaseAttackDamageBuff")]
public class IncreaseAttackDamageBuff : BuffSkill
{
    [Header("버프 / 양수")]
    public float increaseAttackDamageRateLevel1;
    public float increaseAttackDamageRateLevel2;
    public float increaseAttackDamageRateLevel3;

    public float increaseReactionSpeedRateLevel1;
    public float increaseReactionSpeedRateLevel2;
    public float increaseReactionSpeedRateLevel3;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        AIController buffAi = type switch
        {
            BuffType.Self => attacker.GetComponent<AIController>(),
            BuffType.Other => defender.GetComponent<AIController>(),
            _ => attacker.GetComponent<AIController>()
        };

        if (buffAi == null)
            return;

        Vector3 textPos = attacker.transform.position;
        textPos.y += offsetText;
        TextMeshPro text = Instantiate(scrollingBuffText, textPos, Quaternion.identity);
        text.text = "ATK UP!";
        text.color = new Color(1, 0.6f, 0);

        AttackBuff attackBuff = new AttackBuff();
        attackBuff.duration = duration;

        attackBuff.increasedDamageRate = skillLevel switch
        {
            1 => increaseAttackDamageRateLevel1,
            2 => increaseAttackDamageRateLevel2,
            3 => increaseAttackDamageRateLevel3,
            _ => increaseAttackDamageRateLevel1,
        };

        ReactionSpeedBuff reactionSpeedBuff = new ReactionSpeedBuff();
        reactionSpeedBuff.duration = duration;
        reactionSpeedBuff.increasedReactionSpeedRate = skillLevel switch
        {
            1 => increaseReactionSpeedRateLevel1,
            2 => increaseReactionSpeedRateLevel2,
            3 => increaseReactionSpeedRateLevel3,
            _ => increaseReactionSpeedRateLevel1,
        };

        attackBuff.ApplyBuff(buffAi);
        reactionSpeedBuff.ApplyBuff(buffAi);

        DurationEffect buffEffect = Instantiate(durationEffectPrefab, buffAi.transform);
        Destroy(buffEffect.gameObject, duration);
    }
}
