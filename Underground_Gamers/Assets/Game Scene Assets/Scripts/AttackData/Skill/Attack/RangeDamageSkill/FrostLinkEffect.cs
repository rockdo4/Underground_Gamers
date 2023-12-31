using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostLinkEffect : CreateEffectSkill
{
    [Header("프로스트 링크")]
    public Collider[] frostLinkCol = new Collider[3];

    protected override void Update()
    {
        if (hitCount < timing.Length)
        {
            if (timing[hitCount] + timer < Time.time && !frostLinkCol[hitCount].enabled)
            {
                frostLinkCol[hitCount].enabled = true;
                delayTimer = Time.time;
            }

            if (delay + delayTimer < Time.time && frostLinkCol[hitCount].enabled)
            {
                frostLinkCol[hitCount].enabled = false;
                hitCount++;
            }
        }
        else
        {
            frostLinkCol[timing.Length - 1].enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        CharacterStatus aStatus = controller.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
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

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }
}
