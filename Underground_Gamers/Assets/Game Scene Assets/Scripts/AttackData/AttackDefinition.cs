
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDefinition.Asset", menuName = "AttackData/AttackDefinition")]
public class AttackDefinition : ScriptableObject
{
    [Tooltip("������")]
    public float damage;
    public float maxDamageRate;
    public float minDamageRate;

    public float cooldown;
    public float reloadCooldown;

    [Tooltip("���� ��")]
    public float chargeCount;
    [Tooltip("���߷�")]
    public float accuracyRate;
    [Tooltip("��ź��")]
    public float spreadRate;


    [Range(0f, 1f)]
    [Tooltip("ũ��Ƽ�� Ȯ�� 0 ~ 1 ���� ��")]
    public float criticalRate;
    public float criticalMultiplier;

    public SkillTypes skillType;

    public Attack CreateAttack(CharacterStatus attacker, CharacterStatus defender)
    {
        float damage = this.damage + attacker.damage;
        damage *= Random.Range(minDamageRate, maxDamageRate);

        bool isCritical = Random.value < criticalRate;

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