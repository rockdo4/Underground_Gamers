using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Executor.Asset", menuName = "BuffSkill/Executor")]
public class Executor : BuffSkill
{
    public float increaseDamageRateLevel1;
    public float increaseDamageRateLevel2;
    public float increaseDamageRateLevel3;
    public AttackExecutor attackExecutor;


    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();

        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        AIController buffAi = type switch
        {
            BuffType.Self => attacker.GetComponent<AIController>(),
            BuffType.Other => defender.GetComponent<AIController>(),
            _ => attacker.GetComponent<AIController>()
        };

        if (buffAi == null)
            return;

        float damage = skillLevel switch
        {
            1 => aStatus.damage * increaseDamageRateLevel1,
            2 => aStatus.damage * increaseDamageRateLevel2,
            3 => aStatus.damage * increaseDamageRateLevel3,
            _ => aStatus.damage * increaseDamageRateLevel1
        };

        Attack attack = CreateAttack(aStatus, dStatus);
        attack.Damage = Mathf.RoundToInt(damage);
        if (attack.IsCritical)
        {
            attack.IsCritical = false;
        }
        ExecutorBuff executorBuff = new ExecutorBuff();
        executorBuff.duration = duration;
        executorBuff.attackExecutor = attackExecutor;
        executorBuff.attackExecutor.SetAttack(attack);
        executorBuff.ApplyBuff(buffAi);

        DurationEffect buffEffect = Instantiate(durationEffectPrefab, buffAi.transform);
        Destroy(buffEffect.gameObject, duration);
    }
}
