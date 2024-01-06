using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastEffectGarveOfSword : RangeDamageEffect
{
    [Header("후속타")]
    public float[] delayAttacktiming;
    public Attack delayAttack;

    public CreateEffectSkill delayRangeEffect;
    public float durationDelayEffect;
    public float offsetDelayEffect;
    public float scaleDelayEffect;

    protected override void OnDisable()
    {
        // 후속타 검 꽂히는거
        CreateEffectSkill effect = Instantiate(delayRangeEffect, transform.position, delayRangeEffect.transform.rotation);
        effect.SetEffect(controller, delayAttack, delayAttacktiming, delay, Time.time);
        effect.SetOffsetNScale(offsetDelayEffect, scaleDelayEffect);
        Destroy(effect.gameObject, durationDelayEffect);
    }
}
