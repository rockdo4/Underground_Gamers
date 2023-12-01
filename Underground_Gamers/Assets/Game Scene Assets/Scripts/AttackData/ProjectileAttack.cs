using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttack.Asset", menuName = "ProjectileAttack/ProjectileAttack")]
public class ProjectileAttack : AttackDefinition
{
    [Header("≈ıªÁ√º")]
    public Projectile projectilePrefab;
    public GameObject colEffectPrefab;
    public float damageRate;
    public float speed;
    public float lifeCycle;
    public bool isPenetrating;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController ai = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        Transform firePos = null;
        if (ai != null)
        {
            firePos = ai.firePos;
            Projectile projectile = Instantiate(projectilePrefab, firePos.position, ai.transform.rotation);
            ProjectileStatus projectileStatus = projectile.GetComponent<ProjectileStatus>();
            projectile.ai = ai;
            if (aStatus != null)
                projectileStatus.damage = aStatus.damage * damageRate;
            projectileStatus.speed = speed;
            projectileStatus.lifeCycle = lifeCycle;
            projectileStatus.isPenetrating = isPenetrating;
            projectile.colEffectPrefab = colEffectPrefab;
        }
    }
}
