using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChainDamageSkill : RangeDamageSkill
{
    [Header("ø¨º‚ ¿Ã∆Â∆Æ")]
    public CreateEffectSkill chainEffectPrefab;

    public float chainDamageRateLevel1;
    public float chainDamageRateLevel2;
    public float chainDamageRateLevel3;

    public float[] chainTiming = new float[1];
    public float chainColDisableDelay;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        AIController aController = attacker.GetComponent<AIController>();

        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        CreateEffectSkill rangeEffect;
        if (isDirectional)
        {
            if (!isTargetPos)
            {
                Quaternion newRotation = RotateAxis(attacker.transform.rotation.eulerAngles, offsetDirectional);
                rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform.position, newRotation);
            }
            else
            {
                Quaternion newRotation = RotateAxis(attacker.transform.rotation.eulerAngles, offsetDirectional);
                rangeEffect = Instantiate(rangeDamageEffectPrefab, defender.transform.position, newRotation);
            }
        }
        else
        {
            if (!isTargetPos)
            {
                rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform.position, rangeDamageEffectPrefab.transform.rotation);
            }
            else
            {
                rangeEffect = Instantiate(rangeDamageEffectPrefab, defender.transform.position, rangeDamageEffectPrefab.transform.rotation);
            }
        }

        Attack attack = CreateAttack(aStatus, dStatus);
        Attack chainAttack = CreateAttack(aStatus, dStatus);

        float damage = skillLevel switch
        {
            1 => aStatus.damage * damageRateLevel1,
            2 => aStatus.damage * damageRateLevel2,
            3 => aStatus.damage * damageRateLevel3,
            _ => aStatus.damage * damageRateLevel1
        };
        attack.Damage = Mathf.RoundToInt(damage);
        if (attack.IsCritical)
        {
            attack.IsCritical = false;
        }        
        
        float chainDamage = skillLevel switch
        {
            1 => aStatus.damage * chainDamageRateLevel1,
            2 => aStatus.damage * chainDamageRateLevel2,
            3 => aStatus.damage * chainDamageRateLevel3,
            _ => aStatus.damage * chainDamageRateLevel1
        };
        chainAttack.Damage = Mathf.RoundToInt(chainDamage);
        if (chainAttack.IsCritical)
        {
            chainAttack.IsCritical = false;
        }

        rangeEffect.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time);
        rangeEffect.SetHitEffect(durationHitEffect, offsetHitEffect, scaleHitEffect);
        rangeEffect.SetOffsetNScale(offsetRangeEffect, scaleRangeEffect);

        if(rangeEffect is PullRangeDamageEffect)
        {
            PullRangeDamageEffect pullRangeDamageEffect = rangeEffect as PullRangeDamageEffect;
            pullRangeDamageEffect.chainAttack = chainAttack;
            pullRangeDamageEffect.chainDelay = chainColDisableDelay;
            pullRangeDamageEffect.chainTiming = chainTiming;
        }
        Destroy(rangeEffect.gameObject, durationRangeEffect);
    }
}
