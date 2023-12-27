using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeHole.Asset", menuName = "DeBuffSkill/TimeHole")]
public class TimeHole : BuffSkill
{
    public TimeHoleCastEffect timeHoleCastEffectPrefab;
    //public TimeHoleExplosionEffect timeHoleExplodeEffectPrefab;

    [Header("타임 홀 위치 조정")]
    public float offsetTimeHoleCastEffct;

    [Header("타임 홀 크기 조정")]
    public float scaleTimeHoleCastEffct = 1f;    
    public float durationTimeHoleCastEffect = 0.8f;

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
        speedBuff.duration = duration;
        speedBuff.increasedSpeedRate = skillLevel switch
        {
            1 => decreasedSpeedRateLevel1,
            2 => decreasedSpeedRateLevel2,
            3 => decreasedSpeedRateLevel3,
            _ => decreasedSpeedRateLevel1,
        };


        AttackSpeedBuff attackSpeedBuff = new AttackSpeedBuff();
        attackSpeedBuff.duration = duration;
        attackSpeedBuff.increasedAttackSpeedRate = skillLevel switch
        {
            1 => decreasedAttackSpeedRateLevel1,
            2 => decreasedAttackSpeedRateLevel2,
            3 => decreasedAttackSpeedRateLevel3,
            _ => decreasedAttackSpeedRateLevel1,
        };

        TimeHoleCastEffect timeHoleCastEffect = Instantiate(timeHoleCastEffectPrefab, defender.transform.position, timeHoleCastEffectPrefab.transform.rotation);
        timeHoleCastEffect.SetBuff(speedBuff, attackSpeedBuff, buffAi);
        Vector3 castPos = timeHoleCastEffect.transform.position;
        castPos.y += offsetTimeHoleCastEffct;
        timeHoleCastEffect.transform.position = castPos;
        timeHoleCastEffect.transform.localScale *= scaleCastEffct;
        Destroy(timeHoleCastEffect.gameObject, durationTimeHoleCastEffect);
    }
}
