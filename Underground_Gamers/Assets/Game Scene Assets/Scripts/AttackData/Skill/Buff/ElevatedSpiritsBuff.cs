using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ElevatedSpiritsBuff.Asset", menuName = "BuffSkill/ElevatedSpiritsBuff")]
public class ElevatedSpiritsBuff : BuffSkill
{
    public float increasedSpeedRateLevel1;
    public float increasedSpeedRateLevel2;
    public float increasedSpeedRateLevel3;

    public int invalidAttackCountLevel1;
    public int invalidAttackCountLevel2;
    public int invalidAttackCountLevel3;

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
        text.text = "Spirit UP!";
        text.color = new Color(0.7f, 1, 0);


        SpeedBuff speedBuff = new SpeedBuff();
        speedBuff.duration = duration;
        speedBuff.increasedSpeedRate = skillLevel switch
        {
            1 => increasedSpeedRateLevel1,
            2 => increasedSpeedRateLevel2,
            3 => increasedSpeedRateLevel3,
            _ => increasedSpeedRateLevel1,
        };

        InvalidAttackBuff invalidAttackBuff = new InvalidAttackBuff();
        invalidAttackBuff.duration = duration;
        invalidAttackBuff.invalidAttackCount = skillLevel switch
        {
            1 => invalidAttackCountLevel1,
            2 => invalidAttackCountLevel2,
            3 => invalidAttackCountLevel3,
            _ => invalidAttackCountLevel1,
        };

        speedBuff.ApplyBuff(buffAi);
        invalidAttackBuff.ApplyBuff(buffAi);

        GameObject buffEffect = Instantiate(durationEffectPrefab, buffAi.transform);
        Destroy(buffEffect, duration);
    }
}
