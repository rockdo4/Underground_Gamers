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
    public float offsetY = 0.5f;

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
        if (timer > status.lifeCycle)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"COL {other.name}");
        if (ai.enemyLayer == 1 << other.gameObject.layer)
        {
            if (status.isAreaAttack)
                AreaAttack(other);

            if (!status.isAreaAttack)
                SingleAttack(other);

            if (!status.isPenetrating && !status.isAreaAttack)
            {
                Destroy(gameObject);
            }
        }

        if (ai.teamLayer != 1 << other.gameObject.layer
            && ai.enemyLayer != 1 << other.gameObject.layer
            && ai.nodeLayer != 1 << other.gameObject.layer)
        {
            if (status.isAreaAttack)
                AreaAttack(other);

            if (!status.isAreaAttack)
                Destroy(gameObject);
        }
    }

    public Attack CreateAttack(CharacterStatus attacker, CharacterStatus defender)
    {
        float damage = attacker.damage;
        //damage *= Random.Range(minDamageRate, maxDamageRate);
        damage *= Utils.GetRandomDamageByAccuracy(damage, attacker);
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

        return new Attack((int)damage, isCritical, false);
    }
    public void SingleAttack(Collider other)
    {
        Vector3 effectPos = other.transform.position;
        effectPos.y += offsetY;

        GameObject colEffect = Instantiate(colEffectPrefab, effectPos, Quaternion.identity);
        Destroy(colEffect, 2f);

        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
        var attackables = other.GetComponents<IAttackable>();
        Attack attack = CreateAttack(status, dStatus);
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(ai.gameObject, attack);
        }
    }

    public void AreaAttack(Collider other)
    {
        Vector3 effectPos = other.transform.position;
        effectPos.y += offsetY;

        GameObject colEffect = Instantiate(colEffectPrefab, effectPos, Quaternion.identity);
        Destroy(colEffect, 2f);

        var cols = Physics.OverlapSphere(transform.position, status.explosionRange, ai.enemyLayer);

        foreach (var col in cols)
        {
            GameObject explosionEffect = Instantiate(colEffectPrefab, col.transform.position, Quaternion.identity);
            Destroy(colEffect, 2f);

            CharacterStatus dStatus = col.GetComponent<CharacterStatus>();
            var attackables = col.GetComponents<IAttackable>();
            Attack attack = CreateAttack(status, dStatus);
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(ai.gameObject, attack);
            }
        }

        Destroy(gameObject);
    }
}
