using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutorEffect : MonoBehaviour
{
    public AIController controller;
    public DurationEffect durationEffectPrefab;
    
    public Collider col;
    private Attack attack;

    private float hitDuration;

    private float timer;
    private float[] timing;
    private int hitCount = 0;

    private float delayTimer;
    private float delay;
    private int originDamage;

    public float firstDamage = 0.7f;
    public float secondDamage = 0.8f;

    private float hitScale;
    private float hitOffset;
    private void OnDisable()
    {
        col.enabled = false;
    }

    private void Update()
    {
        if (hitCount < timing.Length)
        {
            if (timing[hitCount] + timer < Time.time && !col.enabled)
            {
                hitCount++;
                col.enabled = true;
                delayTimer = Time.time;
            }

            if (delay + delayTimer < Time.time && col.enabled)
            {
                col.enabled = false;
            }
        }
        else
        {
            col.enabled = false;
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

    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }
}
