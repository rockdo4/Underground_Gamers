using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitScanAttack.Asset", menuName = "AttackData/HitScanAttack")]
public class HitScanAttack : AttackDefinition
{
    public GameObject muzzlePrefab;
    public GameObject hitScanLine;
    public float lineWidth;
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

        lineRen.SetPosition(0, attackPos);
        lineRen.SetPosition(1, hitPos);
        lineRen.GetComponent<HitScan>().SetHitScan(attackPos, hitPos);
        GameObject muzzleEffect = Instantiate(muzzlePrefab, attackPos, attackAI.firePos.rotation);
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
