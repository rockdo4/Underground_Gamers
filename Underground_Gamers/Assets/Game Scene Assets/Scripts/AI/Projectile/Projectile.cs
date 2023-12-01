using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public AIController ai;
    private Rigidbody rb;

    private float timer;
    public float criticalMultiplier;
    public float minDamageRate;
    public float maxDamageRate;

    public GameObject colEffectPrefab;

    public ProjectileStatus status;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.AddForce(transform.forward * status.speed, ForceMode.Impulse);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > status.lifeCycle)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject colEffect = Instantiate(colEffectPrefab, other.transform.position, Quaternion.identity);
        Destroy(colEffect, 1f);

        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        if(identity != null)
        {
            if (ai.enemyLayer == identity.teamLayer)
            {
                CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
                var attackables = other.GetComponents<IAttackable>();
                Attack attack = CreateAttack(status, dStatus);
                foreach (var attackable in attackables)
                {
                    attackable.OnAttack(gameObject, attack);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Attack CreateAttack(CharacterStatus attacker, CharacterStatus defender)
    {
        float damage = attacker.damage;
        damage *= Random.Range(minDamageRate, maxDamageRate);

        bool isCritical = Random.value < attacker.critical;

        if (isCritical)
        {
            damage *= criticalMultiplier;
        }

        if (defender != null)
        {
            damage -= defender.armor;
            damage = Mathf.Max(0, damage);
        }

        return new Attack((int)damage, isCritical);
    }
}
