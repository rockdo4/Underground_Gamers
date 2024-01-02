using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarMoveSkill.Asset", menuName = "MoveSkill/FarMoveSkill")]
public class FarMoveSkill : MoveSkill
{
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController aController = attacker.GetComponent<AIController>();
        AIController dController = defender.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();

        var cols = Physics.OverlapSphere(attacker.transform.position, aStatus.sight);
        Vector3 targetPos = Vector3.zero;
        float distanceToTarget = float.MinValue;
        foreach (var col in cols)
        {
            var controller = col.GetComponent<AIController>();
            if (controller == null)
                continue;

            if (attacker.gameObject.layer == controller.gameObject.layer)
                continue;

            float dis = Vector3.Distance(attacker.transform.position, col.transform.position);
            if (dis > distanceToTarget)
            {
                targetPos = col.transform.position;
                distanceToTarget = dis;
            }
        }

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

        attack.Damage = Mathf.RoundToInt(damage);

        if (aController != null)
        {
            DurationEffect trailEffect = Instantiate(trailEffectPrefab, attacker.transform);
            trailEffect.SetOffsetNScale(offsetTrailEffect, scaleTrailEffect);
            Destroy(trailEffect, moveTime);

            aController.UseMoveSkill(aController, moveTime, afterAttack, lookTaget, isPull, attack, 
                attackTiming, colDisableTime, targetPos, attacker.transform.position, effectSkillPrefab, devideForce);
        }
    }
}
