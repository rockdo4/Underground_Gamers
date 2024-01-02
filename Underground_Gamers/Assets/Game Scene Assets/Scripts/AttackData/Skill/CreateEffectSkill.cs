using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffectSkill : MonoBehaviour
{
    public AIController controller;

    public Collider col;
    public Attack attack;

    protected float delayTimer;
    protected float delay;

    protected float timer;
    public float[] timing;
    protected int hitCount = 0;

    [Header("¿Ã∆Â∆Æ")]
    public float durationEffect;
    public float offsetEffect;
    public float scaleEffect = 1f;

    public Vector3 rotateAxis = Vector3.zero;

    [Header("««∞› ¿Ã∆Â∆Æ")]
    public DurationEffect hitEffectPrefab;
    public float durationHitEffect;
    public float offsetHitEffect;
    public float scaleHitEffect = 1f;

    private void OnEnable()
    {
        SetOffsetNScale(offsetEffect, scaleEffect);
        Quaternion newRotation = RotateAxis(transform.rotation.eulerAngles, rotateAxis);
        transform.rotation = newRotation;
    }

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

    protected Quaternion RotateAxis(Vector3 eulerAngles, Vector3 offset)
    {
        Vector3 rotation = eulerAngles;
        rotation.y += offset.y;
        if (rotation.y < 0)
            rotation.y += 360f;

        rotation.y %= 360f;

        rotation.x += offset.x;

        if (rotation.x < 0)
            rotation.x += 360f;

        rotation.x %= 360f;
        Quaternion newRotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, rotation.z));
        return newRotation;
    }

}
