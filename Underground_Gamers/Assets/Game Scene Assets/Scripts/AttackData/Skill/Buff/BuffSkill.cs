using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Self,
    Other
}

[CreateAssetMenu(fileName = "BuffSkill.Asset", menuName = "BuffSkill/BuffSkill")]
public class BuffSkill : AttackDefinition
{
    [Header("น๖วม")]
    public float coolTime;
    public float duration;
    public BuffType type;
    public GameObject effectPrefab;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {

    }
}
