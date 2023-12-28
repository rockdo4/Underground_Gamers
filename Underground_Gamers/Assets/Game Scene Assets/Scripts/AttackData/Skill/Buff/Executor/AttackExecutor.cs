using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackExecutor.Asset", menuName = "BuffSkill/AttackExecutor")]
public class AttackExecutor : AttackDefinition
{
    [Header("집행자 스킬")]
    public ExecutorEffect executorEffectPrefab;
    public float offsetEffect;
    public float scaleEffect;
    public float delayEffect;
    public float onColTime;

    public float duration;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController aController = attacker.GetComponent<AIController>();

        ExecutorEffect executor = Instantiate(executorEffectPrefab);
        executor.SetEffect(aController, delayEffect, onColTime, Time.time);
        executor.SetOffsetNScale(offsetEffect, scaleEffect);
        Destroy(executor, duration);
    }
}
