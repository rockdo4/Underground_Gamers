using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayEffectGraveOfSword : RangeDamageEffect
{
    [Header("ÈÄ¼ÓÅ¸")]
    public float delayDamageRateLevel1;
    public float delayDamageRateLevel2;
    public float delayDamageRateLevel3;

    public Attack delayAttack;

    protected override void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        CharacterStatus aStatus = controller.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();

        int skillLevel = 1;
        if (controller.playerInfo != null)
            skillLevel = controller.playerInfo.skillLevel;

        float damage = skillLevel switch
        {
            1 => aStatus.damage * delayDamageRateLevel1,
            2 => aStatus.damage * delayDamageRateLevel2,
            3 => aStatus.damage * delayDamageRateLevel3,
            _ => aStatus.damage * delayDamageRateLevel1
        };

        if (attack.IsCritical)
        {
            attack.IsCritical = false;
        }

        if (identity == null)
            return;
        if (other.gameObject.layer == controller.gameObject.layer)
            return;

        if (hitEffectPrefab != null)
        {
            DurationEffect hitEffect = Instantiate(hitEffectPrefab, other.transform.position, hitEffectPrefab.transform.rotation);
            hitEffect.SetOffsetNScale(offsetHitEffect, scaleHitEffect);
            Destroy(hitEffect, durationHitEffect);
        }

        var attackables = other.GetComponentsInChildren<IAttackable>();

        damage = Utils.GetRandomDamageByAccuracy(damage, aStatus);
        attack.Damage = Mathf.RoundToInt(damage);

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }
}
