using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutorEffect : CreateEffectSkill
{
    public DurationEffect durationEffectPrefab;
    
    private float hitDuration;

    private int originDamage;

    public float firstDamage = 0.7f;
    public float secondDamage = 0.8f;

    private float hitScale;
    private float hitOffset;

    private void OnDisable()
    {
        col.enabled = false;
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

        DurationEffect durationEffect = Instantiate(durationEffectPrefab, other.transform.position, durationEffectPrefab.transform.rotation);
        durationEffect.SetOffsetNScale(hitOffset, hitScale);
        Destroy(durationEffect, hitDuration);


        attack.Damage = hitCount switch
        {
            1 => Mathf.RoundToInt(attack.Damage * firstDamage),
            2 => Mathf.RoundToInt(attack.Damage * secondDamage),
            3 => originDamage,
            _ => Mathf.RoundToInt(attack.Damage * firstDamage)
        };

        var attackables = other.GetComponentsInChildren<IAttackable>();

        foreach( var attackable in attackables )
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }

    public void SetEffect(AIController ai, Attack attack, float[] timing, float delay, float timer, float hitDuration, float offset, float scale)
    {
        this.controller = ai;
        this.attack = attack;
        this.timing = timing;
        this.delay = delay;
        this.timer = timer;
        this.hitDuration = hitDuration;
        originDamage = this.attack.Damage;
        hitOffset = offset;
        hitScale = scale;
    }
}
