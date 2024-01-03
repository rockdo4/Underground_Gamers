using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunMoveSkill.Asset", menuName = "MoveSkill/StunMoveSkill")]
public class StunMoveSkill : MoveSkill
{
    [Header("Ω∫≈œ")]
    public float stunRateLevel1;
    public float stunRateLevel2;
    public float stunRateLevel3;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController aController = attacker.GetComponent<AIController>();
        AIController dController = defender.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();

        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        float damage = skillLevel switch
        {
            1 => aStatus.damage * damageRateLevel1,
            2 => aStatus.damage * damageRateLevel2,
            3 => aStatus.damage * damageRateLevel3,
            _ => aStatus.damage * damageRateLevel1
        };

        float stuntime = skillLevel switch
        {
            1 => stunRateLevel1,
            2 => stunRateLevel2,
            3 => stunRateLevel3,
            _ => stunRateLevel1
        };

        attack.Damage = Mathf.RoundToInt(damage);

        if (aController != null)
        {
            DurationEffect trailEffect = Instantiate(trailEffectPrefab, attacker.transform);
            trailEffect.SetOffsetNScale(offsetTrailEffect, scaleTrailEffect);
            Destroy(trailEffect, moveTime);

            aController.UseMoveSkill(aController, moveTime, afterAttack, lookTaget, isPull, attack,
                attackTiming, colDisableTime, defender.transform.position, attacker.transform.position, effectSkillPrefab, devideForce, stuntime);
        }
    }

}
