using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDamageEffect : CreateEffectSkill
{
    public DurationEffect hitEffectPrefab;
    private float durationHitEffect;
    private float offsetHitEffect;
    private float scaleHitEffect;

    private void OnTriggerEnter(Collider other)
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

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }

    public void SetHitEffect(float duration, float offset, float scale)
    {
        durationHitEffect = duration;
        offsetHitEffect = offset;
        scaleHitEffect = scale;
    }
}
