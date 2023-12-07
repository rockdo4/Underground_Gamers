using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttack.Asset", menuName = "ProjectileAttack/ProjectileAttack")]
public class ProjectileAttack : AttackDefinition
{
    [Header("≈ıªÁ√º")]
    public Projectile projectilePrefab;
    public GameObject colEffectPrefab;
    public float speed;
    public float lifeCycle;
    public float explosionRange;

    public bool isAreaAttack;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {

    }
}
