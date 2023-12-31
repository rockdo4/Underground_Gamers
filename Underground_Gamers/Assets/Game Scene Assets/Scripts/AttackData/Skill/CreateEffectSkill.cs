using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffectSkill : MonoBehaviour
{
    public AIController controller;

    public Collider col;
    protected Attack attack;

    protected float delayTimer;
    protected float delay;

    protected float timer;
    protected float[] timing;
    protected int hitCount = 0;

    [Header("««∞› ¿Ã∆Â∆Æ")]
    public DurationEffect hitEffectPrefab;
    protected float durationHitEffect;
    protected float offsetHitEffect;
    protected float scaleHitEffect;

    protected virtual void Update()
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

    public virtual void SetEffect(AIController ai, Attack attack, float[] timing, float delay, float timer)
    {
        this.controller = ai;
        this.attack = attack;
        this.timing = timing;
        this.delay = delay;
        this.timer = timer;
    }
    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }

    public void SetHitEffect(float duration, float offset, float scale)
    {
        durationHitEffect = duration;
        offsetHitEffect = offset;
        scaleHitEffect = scale;
    }
}
