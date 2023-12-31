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

    public bool isDirectional = false;

    public RangeDamageEffect rangeDamageEffectPrefab;
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


        float damage = skillLevel switch
        {
            1 => aStatus.damage * damageRateLevel1,
            2 => aStatus.damage * damageRateLevel2,
            3 => aStatus.damage * damageRateLevel3,
            _ => aStatus.damage * damageRateLevel1
        };

        Attack attack = CreateAttack(aStatus, dStatus);
        attack.Damage = Mathf.RoundToInt(damage);
        if (attack.IsCritical)
        {
            attack.IsCritical = false;
        }
        RangeDamageEffect rangeEffect;
        if (isDirectional)
            rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform.position, attacker.transform.rotation);
        else
            rangeEffect = Instantiate(rangeDamageEffectPrefab, attacker.transform.position, rangeDamageEffectPrefab.transform.rotation);

        rangeEffect.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time);
        rangeEffect.SetHitEffect(durationHitEffect, offsetHitEffect, scaleHitEffect);
        rangeEffect.SetOffsetNScale(offsetRangeEffect, scaleRangeEffect);
        Destroy(rangeEffect, durationRangeEffect);

    }
}
