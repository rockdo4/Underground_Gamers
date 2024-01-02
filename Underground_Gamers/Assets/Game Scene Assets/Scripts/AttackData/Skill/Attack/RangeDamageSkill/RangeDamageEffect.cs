using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeDamageEffect : CreateEffectSkill
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        CharacterStatus aStatus = controller.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
        if (identity == null)
            return;
        if (other.gameObject.layer == controller.gameObject.layer)
            return;

        if(hitEffectPrefab != null)
        {
            DurationEffect hitEffect = Instantiate(hitEffectPrefab, other.transform.position, hitEffectPrefab.transform.rotation);
            hitEffect.SetOffsetNScale(offsetHitEffect, scaleHitEffect);
            Destroy(hitEffect, durationHitEffect);
        }

        var attackables = other.GetComponentsInChildren<IAttackable>();

        float damage = attack.Damage;
        damage = Utils.GetRandomDamageByAccuracy(damage, aStatus);
        attack.Damage = Mathf.RoundToInt(damage);

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }
}
