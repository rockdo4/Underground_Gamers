using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppleOfEden.Asset", menuName = "RangeSkill/AppleOfEden")]
public class AppleOfEden : AttackDefinition
{
    public float damageRateLevel1;
    public float damageRateLevel2;
    public float damageRateLevel3;

    public float[] attackTiming = new float[1];
    public float colDisableDelay;

    public float durationEffect;
    public float offsetEffect = 0f;
    public float scaleEffect = 1f;

    private Attack attack;

    public AppleOfEdenEffect appleOfEdenEffectPrefab;

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

        AppleOfEdenEffect appleOfEdenEffect = Instantiate(appleOfEdenEffectPrefab, attacker.transform.position, attacker.transform.rotation);
        appleOfEdenEffect.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time);
        appleOfEdenEffect.SetOffsetNScale(offsetEffect, scaleEffect);
        Destroy(appleOfEdenEffect, durationEffect);

    }
}
