using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerGarden.Asset", menuName = "HealSkill/FlowerGarden")]
public class FlowerGarden : AttackDefinition
{
    [Header("Èú")]
    public CastAreaHealEffect castAreaEffectPrefab;
    public DurationEffect healEffectPrefab;

    public float healRateLevel1;
    public float healRateLevel2;
    public float healRateLevel3;

    public float delayTime;
    public float castDuration;
    public float healDuration;
    public float colOnTime;

    public float offsetCastEffect = 0f;
    public float scaleCastEffct = 1f;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        AIController controller = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = defender.GetComponent<CharacterStatus>();
        TeamIdentifier identity = attacker.GetComponent<TeamIdentifier>();

        Attack heal = CreateHeal(aStatus, dStatus);

        Collider[] cols = Physics.OverlapSphere(attacker.transform.position, aStatus.range, identity.teamLayer);
        AIController healAi = null;
        float minDis = float.MaxValue;
        foreach (var col in cols)
        {
            var colIdentity = col.GetComponent<TeamIdentifier>();

            if (colIdentity == null)
                continue;

            if (colIdentity.isBuilding)
                continue;

            if (col.gameObject.layer != attacker.layer)
                continue;

            if (col.gameObject == attacker.gameObject)
                continue;

            AIController selectAi = col.GetComponent<AIController>();
            float colDis = Vector3.Distance(attacker.transform.position, col.transform.position);
            if (colDis < minDis)
            {
                minDis = colDis;
                healAi = selectAi;
            }
        }

        if (healAi == null)
        {
            healAi = controller;
        }

        CastAreaHealEffect areaHealEffect = Instantiate(castAreaEffectPrefab, healAi.transform.position, castAreaEffectPrefab.transform.rotation);
        areaHealEffect.timer = Time.time;
        areaHealEffect.SetAreaHealEffect(controller, healEffectPrefab, heal, healDuration, colOnTime);
        areaHealEffect.SetOffsetNScale(offsetCastEffect, scaleCastEffct);
        Destroy(areaHealEffect.gameObject, castDuration);
    }

    public Attack CreateHeal(CharacterStatus attacker, CharacterStatus defender)
    {
        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        float healRate = skillLevel switch
        {
            1 => healRateLevel1,
            2 => healRateLevel2,
            3 => healRateLevel3,
            _ => healRateLevel1
        };

        float healAmount = -(attacker.damage* healRate);
        return new Attack((int)healAmount, false, true);
    }
}
