using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHole : BuffSkill
{
    [Header("디버프 / 음수")]
    public float decreasedSpeedRateLevel1;
    public float decreasedSpeedRateLevel2;
    public float decreasedSpeedRateLevel3;

    public int decreaseAttackSpeedRateLevel1;
    public int decreaseAttackSpeedRateLevel2;
    public int decreaseAttackSpeedRateLevel3;
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


    }
}
