using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private Buff speedBuff;
    private Buff attackSpeedBuff;
    public DurationEffect durationEffectPrefab1;
    public DurationEffect durationEffectPrefab2;

    [Header("지속 효과")]
    public float offsetDuration1;
    public float scaleDuration1;
    public float offsetDuration2;
    public float scaleDuration2;
    private float duration;

    public Collider explosionCol;
    private AIController ai;

    public float timer;
    public float colOntime;


    private void OnDisable()
    {
        explosionCol.enabled = false;
    }

    private void Update()
    {
        if(timer + colOntime > Time.time)
        {
            explosionCol.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Col Create");
        var ai = other.GetComponent<AIController>();
        if (ai == null)
            return;

        // 디버프
        if(other.gameObject.layer == this.ai.gameObject.layer)
        {
            Debug.Log("Debuff");

            speedBuff.ApplyBuff(ai);
            attackSpeedBuff.ApplyBuff(ai);

            DurationEffect durationEffect1 = Instantiate(durationEffectPrefab1, ai.transform);
            SetDurationEffect(durationEffect1, offsetDuration1, scaleDuration1);
            Destroy(durationEffect1.gameObject, speedBuff.duration);
            DurationEffect durationEffect2 = Instantiate(durationEffectPrefab2, ai.transform);
            SetDurationEffect(durationEffect2, offsetDuration2, scaleDuration2);
            Destroy(durationEffect2.gameObject, attackSpeedBuff.duration);
        }
    }

    public void SetBuff(Buff speedBuff, Buff attackSpeedBuff, AIController ai)
    {
        this.speedBuff = speedBuff;
        this.attackSpeedBuff = attackSpeedBuff;
        this.ai = ai;
    }

    private void SetDurationEffect(DurationEffect effect, float offset, float scale)
    {
        Vector3 effectPos = effect.transform.position;
        effectPos.y += offset;
        effect.transform.position = effectPos;

        effect.transform.localScale *= scale;
    }
}
