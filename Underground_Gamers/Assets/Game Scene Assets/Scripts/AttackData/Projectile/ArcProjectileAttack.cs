using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcProjectileAttack.Asset", menuName = "ProjectileAttack/ArcProjectileAttack")]
public class ArcProjectileAttack : AttackDefinition
{
    [Header("포물선 투사체")]
    public ArcProjectile arcProjectilePrefab;
    public GameObject explosionEffectPrefab;
    public float damageRate;
    public float explosionRange;
    public float speed;
    public float lifeCycle;
    public float fireAngle;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController ai = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        Transform firePos = null;
        Quaternion fireRotation = Quaternion.Euler(fireAngle, 0, 0);
        if(ai != null)
        {
            firePos = ai.firePos;
            ArcProjectile arcProjectile = Instantiate(arcProjectilePrefab, firePos.position, fireRotation);
        }

    }

}
