using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitScanAttack.Asset", menuName = "AttackData/HitScanAttack")]
public class HitScanAttack : AttackDefinition
{
    public GameObject hitScanLine;
    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        GameObject line = Instantiate(hitScanLine);
        LineRenderer lineRen = line.GetComponent<LineRenderer>();
        AIController attackAI = attacker.GetComponent<AIController>();
        lineRen.positionCount = 2;

        var attackPos = attackAI.firePos.position;
        //attackPos.y += 0.6f;

        lineRen.SetPosition(0, attackPos);
        lineRen.SetPosition(1, attackAI.hitInfoPos);

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
