
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
    [HideInInspector]
    public float fixedCooldown;
    public float reloadCooldown;

    [Tooltip("장전 수")]
    public int chargeCount;
    [Tooltip("명중률")]
    public float accuracyRate;
    [Tooltip("집탄율")]
    public float spreadRate;


    [Range(0f, 1f)]
    [Tooltip("크리티컬 확률 0 ~ 1 사이 값")]
    public float criticalRate;
    public float criticalMultiplier;

    public SkillMode skillMode;
    public SkillType skillType;

    public Attack CreateAttack(CharacterStatus attacker, CharacterStatus defender)
    {
        float damage = this.damage + attacker.damage;
        damage *= Random.Range(minDamageRate, maxDamageRate);
        // 캐릭터의 공속 + 무기의 공속(스킬, 평타)
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

        return new Attack((int)damage, isCritical, false);
    }

    public virtual void ExecuteAttack(GameObject attacker, GameObject defender)
    {

    }
}
