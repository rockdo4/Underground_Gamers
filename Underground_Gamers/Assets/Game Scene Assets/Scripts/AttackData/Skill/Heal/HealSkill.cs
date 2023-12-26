using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSkill.Asset", menuName = "HealSkill/HealSkill")]
public class HealSkill : AttackDefinition
{
    [Header("Èú")]
    public GameObject effectPrefab;
    public float healRate = 0.25f;
    public float delayTime;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController controller = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        TeamIdentifier identity = attacker.GetComponent<TeamIdentifier>();

        Collider[] cols = Physics.OverlapSphere(attacker.transform.position, aStatus.range, identity.teamLayer);
        CharacterStatus select = null;
        float minHpRate = float.MaxValue;
        foreach (var col in cols)
        {
            var colIdentity = col.GetComponent<TeamIdentifier>();

            if (colIdentity == null)
                continue;

            if (colIdentity.isBuilding)
                continue;


            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            float colHpRate = colStatus.Hp / colStatus.maxHp;
            if(colHpRate < minHpRate)
            {
                minHpRate = colHpRate;
                select = colStatus;
            }
        }


        if (select == null)
        {
            controller.isOnCoolOriginalSkill = true;
            return;
        }

        Attack heal = CreateHeal(aStatus, select);

        GameObject healPrefab = Instantiate(effectPrefab, select.transform);
        Destroy(healPrefab, 1f);


        var attackables = select.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, heal);
        }
    }

    public Attack CreateHeal(CharacterStatus attacker, CharacterStatus defender)
    {
        damage = -(defender.maxHp * healRate);
        return new Attack((int)damage, false, true);
    }
}
