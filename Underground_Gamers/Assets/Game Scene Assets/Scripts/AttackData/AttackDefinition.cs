
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDefinition.Asset", menuName = "AttackData/AttackDefinition")]
public class AttackDefinition : ScriptableObject
{
    [Header("어택")]
    [Tooltip("데미지")]
    public float damage;
    public float maxDamageRate;
    public float minDamageRate;

    public float cooldown;
    public float reloadCooldown;

    [Tooltip("장전 수")]
    public float chargeCount;
    [Tooltip("명중률")]
    public float accuracyRate;
    [Tooltip("집탄율")]
    public float spreadRate;


    [Range(0f, 1f)]
    [Tooltip("크리티컬 확률 0 ~ 1 사이 값")]
    public float criticalRate;
    public float criticalMultiplier;

    public SkillTypes skillType;

    public Attack CreateAttack(CharacterStatus attacker, CharacterStatus defender)
    {
        float damage = this.damage + attacker.damage;
        damage *= Random.Range(minDamageRate, maxDamageRate);

        bool isCritical = Random.value < attacker.critical;

        if(isCritical)
        {
            damage *= criticalMultiplier;
        }

        if(defender != null)
        {
            damage -= defender.armor;
            damage = Mathf.Max(0, damage);
        }

        return new Attack((int)damage, isCritical);
    }

    public virtual void ExecuteAttack(GameObject attacker, GameObject defender)
    {

    }
}
