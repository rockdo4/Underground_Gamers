using UnityEngine;


[CreateAssetMenu(fileName = "Chain.Asset", menuName = "DeBuffSkill/Chain")]
public class Chain : BuffSkill
{
    [Header("디버프 / 음수")]
    public float decreaseSpeedRateLevel1;
    public float decreaseSpeedRateLevel2;
    public float decreaseSpeedRateLevel3;

    public float decreaseAttackSpeedRateLevel1;
    public float decreaseAttackSpeedRateLevel2;
    public float decreaseAttackSpeedRateLevel3;


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
            1 => decreaseSpeedRateLevel1,
            2 => decreaseSpeedRateLevel2,
            3 => decreaseSpeedRateLevel3,
            _ => decreaseSpeedRateLevel1,
        };


        AttackSpeedBuff attackSpeedBuff = new AttackSpeedBuff();
        attackSpeedBuff.duration = duration + castDuration;
        attackSpeedBuff.increasedAttackSpeedRate = skillLevel switch
        {
            1 => decreaseAttackSpeedRateLevel1,
            2 => decreaseAttackSpeedRateLevel2,
            3 => decreaseAttackSpeedRateLevel3,
            _ => decreaseAttackSpeedRateLevel1,
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
