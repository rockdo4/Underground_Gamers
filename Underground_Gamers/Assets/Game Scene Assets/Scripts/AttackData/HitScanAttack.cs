using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "HitScanAttack.Asset", menuName = "AttackData/HitScanAttack")]
public class HitScanAttack : AttackDefinition
{
    public GameObject muzzlePrefab;
    public GameObject hitScanLine;
    public float lineWidth;
    public float offsetFirePos = 0.8f;
    private Vector3 cameraRight;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        GameObject line = Instantiate(hitScanLine);
        LineRenderer lineRen = line.GetComponent<LineRenderer>();
        AIController attackAI = attacker.GetComponent<AIController>();
        lineRen.positionCount = 2;
        lineRen.startWidth = lineWidth;
        lineRen.endWidth = lineWidth;

        var attackPos = attackAI.firePos.position;
        var hitPos = attackAI.hitInfoPos;
        hitPos.y = attackPos.y;
        cameraRight = Camera.main.transform.right;
        float dot = Vector3.Dot(attacker.transform.forward, cameraRight);
        Vector3 muzzlePos = attackPos;
        Vector3 euler = new Vector3(0f, 180f, 0f);
        if (dot > 0)
        {
            euler = Vector3.zero;
            attackPos.z += offsetFirePos;
            muzzlePos.z += offsetFirePos;
        }
        else
        {
            attackPos.z -= offsetFirePos;
            muzzlePos.z -= offsetFirePos;
        }

        lineRen.SetPosition(0, attackPos);
        lineRen.SetPosition(1, hitPos);

        Quaternion rotation = Quaternion.Euler(euler);

        GameObject muzzleEffect = Instantiate(muzzlePrefab, muzzlePos, rotation);
        Destroy(muzzleEffect, 1f);

        var attackStatus = attacker.GetComponent<CharacterStatus>();
        var defendStatus = defender.GetComponent<CharacterStatus>();

        var attack = CreateAttack(attackStatus, defendStatus);

        var attackables = defender.GetComponents<IAttackable>();
        foreach( var attackable in attackables )
        {
            attackable.OnAttack(attacker, attack);
        }
        Destroy(line, 0.3f);
    }
}
