using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeDamageEffect : CreateEffectSkill
{
    [Header("Ω∫≈œ ¿Ã∆Â∆Æ")]
    public float stunTime = 0f;
    public DurationEffect stunEffectPrefab;
    public float offsetStunEffect;
    public float scaleStunEffect = 1f;

    [Header("ø¨º‚ ¿Ã∆Â∆Æ")]
    public CreateEffectSkill chainEffectPrefab;
    public Attack chainAttack;
    public float chainDelay;
    public float[] chainTiming;

    public float durationChainEffect = 1f;

    private void OnDisable()
    {
        CreateEffectSkill chainEffect = Instantiate(chainEffectPrefab);
        chainEffect.SetEffect(controller, chainAttack, chainTiming, chainDelay, Time.time);
        Destroy(chainEffect, durationChainEffect);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        CharacterStatus aStatus = controller.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
        AIController dController = other.GetComponent<AIController>();
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

        float damage = attack.Damage;
        damage = Utils.GetRandomDamageByAccuracy(damage, aStatus);
        attack.Damage = Mathf.RoundToInt(damage);

        if (stunTime > 0f)
        {
            dController.Stun(false, stunTime);
            DurationEffect stunEffect = Instantiate(stunEffectPrefab, dController.transform);
            stunEffect.SetOffsetNScale(offsetStunEffect, scaleStunEffect);
            Destroy(stunEffect.gameObject, stunTime);
        }

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }
}
