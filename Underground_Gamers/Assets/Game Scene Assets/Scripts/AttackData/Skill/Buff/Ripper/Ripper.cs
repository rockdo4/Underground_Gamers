using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ripper.Asset", menuName = "BuffSkill/Ripper")]
public class Ripper : BuffSkill
{
    public float increaseCriticalRateLevel1;
    public float increaseCriticalRateLevel2;
    public float increaseCriticalRateLevel3;

    public float increaseAccuracyRateLevel1;
    public float increaseAccuracyRateLevel2;
    public float increaseAccuracyRateLevel3;

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        //AIController buffAi = type switch
        //{
        //    BuffType.Self => attacker.GetComponent<AIController>(),
        //    BuffType.Other => defender.GetComponent<AIController>(),
        //    _ => attacker.GetComponent<AIController>()
        //};


        AIController controller = attacker.GetComponent<AIController>();
        CharacterStatus aStatus = attacker.GetComponent<CharacterStatus>();
        TeamIdentifier identity = attacker.GetComponent<TeamIdentifier>();



        Collider[] cols = Physics.OverlapSphere(attacker.transform.position, aStatus.range, identity.teamLayer);
        AIController buffAi = null;
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
                buffAi = selectAi;
            }
        }

        if (buffAi == null)
        {
            buffAi = controller;
        }

        CriticalBuff criticalBuff = new CriticalBuff();
        criticalBuff.duration = duration + castDuration;
        criticalBuff.increasedCriticalRate = skillLevel switch
        {
            1 => increaseCriticalRateLevel1,
            2 => increaseCriticalRateLevel2,
            3 => increaseCriticalRateLevel3,
            _ => increaseCriticalRateLevel1,
        };


        AccuracyBuff attackSpeedBuff = new AccuracyBuff();
        attackSpeedBuff.duration = duration + castDuration;
        attackSpeedBuff.increasedAccuracyRate = skillLevel switch
        {
            1 => increaseAccuracyRateLevel1,
            2 => increaseAccuracyRateLevel2,
            3 => increaseAccuracyRateLevel3,
            _ => increaseAccuracyRateLevel1,
        };

        criticalBuff.ApplyBuff(buffAi);
        attackSpeedBuff.ApplyBuff(buffAi);

        CastEffect castEffect = Instantiate(castEffectPrefab, buffAi.transform);
        castEffect.SetDurationEffect(durationEffectPrefab, buffAi.transform, duration + castDuration, offsetDurationEffct, scaleDurationEffct);
        Vector3 pos = castEffect.transform.position;
        pos.y += offsetCastEffct;
        castEffect.transform.position = pos;
        castEffect.transform.localScale *= scaleCastEffct;
        Destroy(castEffect, castDuration);
    }

}
