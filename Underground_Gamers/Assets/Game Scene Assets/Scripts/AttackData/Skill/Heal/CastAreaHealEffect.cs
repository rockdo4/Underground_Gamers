using Demo_Project;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class CastAreaHealEffect : MonoBehaviour
{
    public float timer;
    public float colOntime;
    public Collider explosionCol;
    public DurationEffect healEffectPrefab;

    public AIController attacker;
    public Attack healAmount;
    public float healDuration;

    public float offsetDurationEffect = 0f;
    public float scaleDurationEffect = 1f;

    private void OnDisable()
    {
        explosionCol.enabled = false;
    }

    private void Update()
    {
        if (timer + colOntime < Time.time)
        {
            explosionCol.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var ai = other.GetComponent<AIController>();
        if (ai == null)
            return;

        if (other.gameObject.layer == this.attacker.gameObject.layer)
        {
            DurationEffect healPrefab = Instantiate(healEffectPrefab, other.gameObject.transform);
            healPrefab.SetOffsetNScale(offsetDurationEffect, scaleDurationEffect);
            Destroy(healPrefab.gameObject, 1f);

            var attackables = other.GetComponents<IAttackable>();
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(attacker.gameObject, healAmount);
            }
        }
    }


    public void SetAreaHealEffect(AIController ai, DurationEffect healEffect, Attack healAmount, float duration, float colOnTime)
    {
        attacker = ai;
        healEffectPrefab = healEffect;
        this.healAmount = healAmount;
        healDuration = duration;
        this.colOntime = colOnTime;
    }

    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }
}
