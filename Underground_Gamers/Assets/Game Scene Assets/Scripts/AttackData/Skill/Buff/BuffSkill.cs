using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float castDuration = 1f;
    public float duration;
    public BuffType type;
    public CastEffect castEffectPrefab;
    public GameObject durationEffectPrefab;
    public TextMeshPro scrollingBuffText;
    public float offsetText;
    public float offsetDurationEffct;
    public float offsetCastEffct;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {

    }
}
