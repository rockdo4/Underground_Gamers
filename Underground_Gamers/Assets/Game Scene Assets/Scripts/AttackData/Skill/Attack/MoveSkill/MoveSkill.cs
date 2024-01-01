using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MoveSkill.Asset", menuName = "MoveSkill/MoveSkill")]
public class MoveSkill : AttackDefinition
{
    [Header("이동 스킬")]
    public float damageRateLevel1;
    public float damageRateLevel2;
    public float damageRateLevel3;

    public float moveTime;
    public float moveSpeed;

    public bool afterAttack = true;

    [Header("공격기")]
    public Attack attack;
    public float[] attackTiming = new float[1];
    public float colDisableTime = 0.01f;

    [Header("이펙트")]
    public CreateEffectSkill effectSkillPrefab;
    public float durationEffect;
    public float offsetEffect;
    public float scaleEffect = 1f;

    [Header("타격 이펙트")]
    public float durationHitEffect;
    public float offsetHitEffect;
    public float scaleHitEffect = 1f;

    [Header("트레일 이펙트")]
    public DurationEffect trailEffectPrefab;
    public float offsetTrailEffect;
    public float scaleTrailEffect = 1f;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController aController = attacker.GetComponent<AIController>();
        AIController dController = defender.GetComponent<AIController>();

        Vector3 dirToTarget = (defender.transform.position - attacker.transform.position).normalized;
        NavMeshAgent aNav = aController.GetComponent<NavMeshAgent>();
        if (aController != null && aNav != null)
        {
            aNav.enabled = false;
            DurationEffect trailEffect= Instantiate(trailEffectPrefab, attacker.transform);
            trailEffect.SetOffsetNScale(offsetTrailEffect, scaleTrailEffect);

            Destroy(trailEffect, moveTime);

            aController.UseMoveSkill(aController, moveTime, moveSpeed, afterAttack, attack, attackTiming, colDisableTime,
                defender.transform.position, 
                effectSkillPrefab, durationEffect, offsetEffect, scaleEffect,
                durationHitEffect, offsetHitEffect, scaleHitEffect);
        }
    }
}
