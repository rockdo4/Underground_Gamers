using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[CreateAssetMenu(fileName = "ArcProjectileAttack.Asset", menuName = "ProjectileAttack/ArcProjectileAttack")]
public class ArcProjectileAttack : ProjectileAttack
{
    [Header("포물선 투사체")]
    public float damageRate;
    public float fireAngle;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController ai = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        Transform firePos = null;
        Quaternion fireRotation = Quaternion.Euler(-fireAngle, 0, 0);
        if(ai != null)
        {
            firePos = ai.firePos;
            Projectile arcrojectile = Instantiate(projectilePrefab, firePos.position, fireRotation);
            ProjectileStatus projectileStatus = arcrojectile.GetComponent<ProjectileStatus>();
            arcrojectile.ai = ai;
            arcrojectile.gameObject.layer = ai.gameObject.layer;

            if (aStatus != null)
                projectileStatus.damage = aStatus.damage * damageRate;
            projectileStatus.speed = speed;
            projectileStatus.lifeCycle = lifeCycle;
            //projectileStatus.isPenetrating = isPenetrating;
            projectileStatus.isAreaAttack = isAreaAttack;
            projectileStatus.explosionRange = explosionRange;

            arcrojectile.colEffectPrefab = colEffectPrefab;
        }

    }

}
