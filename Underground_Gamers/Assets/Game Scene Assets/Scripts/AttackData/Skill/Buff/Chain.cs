using UnityEngine;


[CreateAssetMenu(fileName = "Chain.Asset", menuName = "DeBuffSkill/Chain")]
public class Chain : BuffSkill
{
    [Header("디버프 / 음수")]
    public float decreasedSpeedRateLevel1;
    public float decreasedSpeedRateLevel2;
    public float decreasedSpeedRateLevel3;

    public float decreasedAttackSpeedRateLevel1;
    public float decreasedAttackSpeedRateLevel2;
    public float decreasedAttackSpeedRateLevel3;


    public override void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        int skillLevel = 1;
        if (attacker.GetComponent<AIController>().playerInfo != null)
            skillLevel = attacker.GetComponent<AIController>().playerInfo.skillLevel;

        AIController buffAi = type switch
        {
            BuffType.Self => attacker.GetComponent<AIController>(),
            BuffType.Other => defender.GetComponent<AIController>(),
            _ => attacker.GetComponent<AIController>()
        };

        if (buffAi == null)
            return;

        SpeedBuff speedBuff = new SpeedBuff();
        speedBuff.duration = duration + castDuration;
        speedBuff.increasedSpeedRate = skillLevel switch
        {
            1 => decreasedSpeedRateLevel1,
            2 => decreasedSpeedRateLevel2,
            3 => decreasedSpeedRateLevel3,
            _ => decreasedSpeedRateLevel1,
        };


        AttackSpeedBuff attackSpeedBuff = new AttackSpeedBuff();
        attackSpeedBuff.duration = duration + castDuration;
        attackSpeedBuff.increasedAttackSpeedRate = skillLevel switch
        {
            1 => decreasedAttackSpeedRateLevel1,
            2 => decreasedAttackSpeedRateLevel2,
            3 => decreasedAttackSpeedRateLevel3,
            _ => decreasedAttackSpeedRateLevel1,
        };

        speedBuff.ApplyBuff(buffAi);
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
