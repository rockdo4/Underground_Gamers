using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackExecutor.Asset", menuName = "BuffSkill/AttackExecutor")]
public class AttackExecutor : AttackDefinition
{
    [Header("집행자 스킬")]
    public ExecutorEffect executorEffectPrefab;
    public float offsetEffect = 0f;
    public float scaleEffect = 1f;

    public float[] attackTiming = new float[3];
    public float colDisableDelay;
    private Attack attack;

    public float duration;
    public float hitDuration;
    public float hitOffset = 0f;
    public float hitScale = 1f;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController aController = attacker.GetComponent<AIController>();

        ExecutorEffect executor = Instantiate(executorEffectPrefab, attacker.transform.position, attacker.transform.rotation);
        executor.SetEffect(aController, attack, attackTiming, colDisableDelay, Time.time, hitDuration, hitOffset, hitScale);
        executor.SetOffsetNScale(offsetEffect, scaleEffect);
        Destroy(executor, duration);
    }

    public void SetAttack(Attack attack)
    {
        this.attack = attack;
    }
}
