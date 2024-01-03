using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeChannelingDamageSkill.Asset", menuName = "RangeSkill/RangeChannelingDamageSkill")]
public class RangeChannelingDamageSkill : RangeDamageSkill
{
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        AIController aController = attacker.GetComponent<AIController>();

        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;
        aController.isChanneling = true;

        CreateEffectSkill rangeEffect;
        if (isDirectional)
        {
            if (!isTargetPos)
            {
                if(aController.isChanneling)
                {
                    rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform);
                    Vector3 effectPos = rangeEffect.transform.localPosition;
                    effectPos.z += 0.5f;
                    rangeEffect.transform.localPosition = effectPos;
                }
                else
                {
                    Quaternion newRotation = RotateAxis(attacker.transform.rotation.eulerAngles, offsetDirectional);
                    rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform.position, newRotation);
                }
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

        if (rangeEffect is CastEffectGarveOfSword)
        {
            CastEffectGarveOfSword garveOfSwordEffect = rangeEffect as CastEffectGarveOfSword;
            garveOfSwordEffect.delayAttack = attack;
        }

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

        aController.Stun(false, durationRangeEffect);
        rangeEffect.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time);
        rangeEffect.SetHitEffect(durationHitEffect, offsetHitEffect, scaleHitEffect);
        rangeEffect.SetOffsetNScale(offsetRangeEffect, scaleRangeEffect);


        Destroy(rangeEffect.gameObject, durationRangeEffect);
    }

}
