using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeDamageSkill.Asset", menuName = "RangeSkill/RangeDamageSkill")]
public class RangeDamageSkill : AttackDefinition
{
    public float damageRateLevel1;
    public float damageRateLevel2;
    public float damageRateLevel3;

    public float[] attackTiming = new float[1];
    public float colDisableDelay;

    [Header("π¸¿ß¿Ã∆Â∆Æ")]
    public float durationRangeEffect;
    public float offsetRangeEffect = 0f;
    public float scaleRangeEffect = 1f;

    public Vector3 offsetDirectional = Vector3.zero;

    public bool isDirectional = false;
    public bool isTargetPos = false;

    public CreateEffectSkill rangeDamageEffectPrefab;
    private Attack attack;

    [Header("≈∏∞› ¿Ã∆Â∆Æ")]
    public float durationHitEffect = 1f;
    public float offsetHitEffect = 0f;
    public float scaleHitEffect = 1f;

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
        rangeEffect.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time);
        rangeEffect.SetHitEffect(durationHitEffect, offsetHitEffect, scaleHitEffect);
        rangeEffect.SetOffsetNScale(offsetRangeEffect, scaleRangeEffect);


        Destroy(rangeEffect.gameObject, durationRangeEffect);
    }

    protected Quaternion RotateAxis(Vector3 eulerAngles, Vector3 offset)
    {
        Vector3 rotation = eulerAngles;
        rotation.y += offset.y;
        if (rotation.y < 0)
            rotation.y += 360f;

        rotation.y %= 360f;

        rotation.x += offset.x;

        if (rotation.x < 0)
            rotation.x += 360f;

        rotation.x %= 360f;
        Quaternion newRotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, rotation.z));
        return newRotation;
    }
}
