using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MoveSkill.Asset", menuName = "MoveSkill/MoveSkill")]
public class MoveSkill : AttackDefinition
{
    [Header("이동 스킬")]
    public float damageRateLevel1;
    public float damageRateLevel2;
    public float damageRateLevel3;

    public float moveTime;

    public bool afterAttack = true;
    public bool lookTaget = true;
    public bool isPull = false;

    [Header("공격기")]
    public Attack attack;
    public float[] attackTiming = new float[1];
    public float colDisableTime = 0.01f;

    [Header("이펙트")]
    public CreateEffectSkill effectSkillPrefab;

    [Header("타격 이펙트")]
    public float durationHitEffect;
    public float offsetHitEffect;
    public float scaleHitEffect = 1f;

    [Header("트레일 이펙트")]
    public DurationEffect trailEffectPrefab;
    public float offsetTrailEffect;
    public float scaleTrailEffect = 1f;

    [Header("리지드 바디")]
    public float devideForce;

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

        attack.Damage = Mathf.RoundToInt(damage);

        if (aController != null)
        {
            DurationEffect trailEffect = Instantiate(trailEffectPrefab, attacker.transform);
            trailEffect.SetOffsetNScale(offsetTrailEffect, scaleTrailEffect);
            Destroy(trailEffect, moveTime);

            aController.UseMoveSkill(aController, moveTime, afterAttack, lookTaget, isPull, attack,
                attackTiming, colDisableTime, defender.transform.position, attacker.transform.position, effectSkillPrefab, devideForce);
        }
    }
}
