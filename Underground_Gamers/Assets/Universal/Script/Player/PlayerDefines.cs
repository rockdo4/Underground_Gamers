using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

enum PlayerClass
{
    Attacker,
    Defender,
    Supporter,
    Sniper
}
public class Player
{
    public float ID = -1f;

    public string name = string.Empty;
    public int code = -1;
    public int type = -1;
    public int grade = 3;
    public int level = 1;
    public int maxLevel = 35;
    public int breakthrough = 0;

    public int skillLevel = 1;
    public int gearCode = -1;
    public int gearLevel = 0;
    public float xp = 0;
    public float maxXp = 10;
    public int condition = 0;
    public int cost = 0;
    public int potential = 10;
    public List<int> training = new List<int>();
    //ability??
}

public struct PlayerInfo
{
    public int code;
    public int UniqueSkillCode;
    public int Potential;
    public int type;
    public string name;
    public string info;    //선수 설명
    public int grade;
    public int cost;
    public int weaponType;

    public GrowableStats hp;
    public GrowableStats atk;
    public GrowableStats atkRate;
    public GrowableStats moveSpeed;
    public GrowableStats sight;
    public GrowableStats range;
    public GrowableStats critical;
    public GrowableStats accuracy;
    public GrowableStats reactionSpeed;
    public GrowableStats detectionRange;

    public int magazine;
    public float reloadingSpeed;

    public float avoidRate;        //회피율
    public float collectingRate;   //집탄율

    public int atkType;
    public int kitingType;
}

public struct GrowableStats
{
    public float min;
    public float max;
    public float gap 
    { 
        get 
        {
            return (max - min) / 49;
        }
    }
}

public struct TrainingInfo
{
    public int id;
    public TrainingType type;
    public int level;
    public float value;
    public int needPotential;

    public void AddStats(PlayerInfo playerStats)
    {
        switch (type)
        {
            case TrainingType.MoveSpeed:
                playerStats.moveSpeed.min += value;
                break;
            case TrainingType.Sight:
                playerStats.sight.min += value;
                break;
            case TrainingType.Range:
                playerStats.range.min += value;
                break;
            case TrainingType.DetectionRange:
                playerStats.detectionRange.min += value;
                break;
            case TrainingType.Accuracy:
                playerStats.accuracy.min += value;
                break;
            case TrainingType.ReactionSpeed:
                playerStats.reactionSpeed.min += value;
                break;
            case TrainingType.AtkRate:
                playerStats.atkRate.min += value;
                break;
            case TrainingType.Critical:
                playerStats.critical.min += value;
                break;
            case TrainingType.Atk:
                playerStats.atk.min += value;
                break;
            case TrainingType.Hp:
                playerStats.hp.min += value;
                break;
            default:
                break;
        }
    }
}

public enum TrainingType
{
    MoveSpeed,
    Sight,
    Range,
    DetectionRange,
    Accuracy,
    ReactionSpeed,
    AtkRate,
    Critical,
    Atk,
    Hp,
    Count
}

public struct RecruitInfo
{
    public string code;
    public string info;
    public int money;
    public int crystal;
    public int contractTicket;
}

public struct LevelUpCost
{
    public int level;
    public int xp;
    public int cost;
}

[System.Serializable]
public class AttackDefinitionData
{
    public int code;
    public AttackDefinition value;
}

[System.Serializable]
public class KitingDefinitionData
{ 
    public int code;
    public KitingData value;
}

[System.Serializable]
public class TargetPriorityDefinitionData
{
    public int code;
    public TargetPriority value;
}

public static class ToggleExtensions
{
    public static void SetSingleListener(this Toggle toggle, UnityAction<bool> listener)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(listener);
    }
}

public struct StageInfo
{
    public int code;
    public int type;
    public List<int> enemys;
    public List<int> rewards;
}