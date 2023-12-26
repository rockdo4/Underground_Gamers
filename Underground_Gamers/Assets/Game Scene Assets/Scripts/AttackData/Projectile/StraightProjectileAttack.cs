using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StraightProjectileAttack.Asset", menuName = "ProjectileAttack/StraightProjectileAttack")]
public class StraightProjectileAttack : ProjectileAttack
{
    [Header("직선 투사체")]
    public float damageRate;
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
            projectile.gameObject.layer = ai.gameObject.layer;
            Debug.Log("Create Projectile");

            if (aStatus != null)
                projectileStatus.damage = aStatus.damage * damageRate;
            projectileStatus.speed = speed;
            projectileStatus.lifeCycle = lifeCycle;
            projectileStatus.isPenetrating = isPenetrating;
            projectileStatus.isAreaAttack = isAreaAttack;
            projectileStatus.explosionRange = explosionRange;

            projectile.colEffectPrefab = colEffectPrefab;
        }
    }
}
