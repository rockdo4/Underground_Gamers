using System;
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
    public int levelUpCount = 0;
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

    public int index;
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
    public string mapCode;
    public string name;
    public int type;
    public List<int> enemys;
    public List<int> rewards;
    public int xp;
    public string teamName;
}

public struct EnemyInfo
{
    public int code;
    public string name;
    public int grade;
    public int uniqueSkill;
    public int type;
    public string info;
    public int weaponType;
    public int atkType;
    public int kitingType;
    public int mag;
    public float reload;
    public int hp;
    public int atk;
    public float atkRate;
    public float moveSpeed;
    public float sight;
    public float range;
    public float critical;
    public float accuracy;
    public float reaction;
    public float detection;
}

public class SaveData
{
    public int representativePlayer = -1;
    public List<Player> havePlayers = new List<Player>();
    public List<Player> usingPlayers = new List<Player>();

    public int cleardStage = 0;
    public string playername = "이감독";
    public int money = 1000;
    public int crystal = 1000;
    public int contractTicket = 0;
    public int stamina = 0;
    public List<int> XpItem = new List<int>(4);
    public int IDcode = 0;
    public int PresetCode = 0;
    public List<List<float>> Presets;
    public List<int> tradeCenter = new List<int>();
    public DateTime lastRecruitTime = DateTime.MinValue;
    public bool isInit = true;

    public List<float> officialPlayers = new List<float>();
    public string teamName = "드림팀";
    public int playSpeed = 1;

    public Queue<int> storyQueue = new Queue<int>();
    public bool isInfoOn = false;
    public bool isAutoBattle = false;

    //정규전
    public bool isOnOfficial = false;
    public int officialLevel = -1;
    public List<EnemyInfo>[] enemyTeams = new List<EnemyInfo>[7];
    public OfficialTeamData[] officialTeamDatas = new OfficialTeamData[8];
    public OfficialPlayerData[] officialPlayerDatas = new OfficialPlayerData[8];
    public int officialWeekNum = 0;
    public int[,] officialMatchResult = new int[7, 8];
    public string[,] officialFinalMatchResultName = new string[3, 2];
    public int[,] officialFinalMatchResult = new int[3, 2];
    public bool endScrimmage = false;

    //스크리밍
    public DateTime lastScrimmageTime = DateTime.MinValue;
    public int scrimmageCount = 3;

    //옵션
    public int quality = 2;
    public int resolution = 2;
    public int textureQuality = 2;
    public bool isPost_fx = true;
    public bool isShadowOn = true;
    public bool is60FPS = true;
    public int language = 0;

    public bool isSkillIllust = true;
    public bool isSkillEffect = true;

    public bool muteMaster = false;
    public bool muteBackground = false;
    public bool muteEffect = false;

    public float volumeMaster = 1f;
    public float volumeBackground = 1f;
    public float volumeEffect = 1f;
}

public struct OfficialTeamData
{
    public string name;
    public int index;
    public int win;
    public int lose;
    public int setWin;
    public int setLose;
    public bool isPlayer;
}

public struct OfficialPlayerData
{
    public int playCount;
    public int kill;
    public int death;
    public int totalDamage;
}

public struct SkillInfo
{
    public int ID;
    public int name;
    public int toolTip;
    public Sprite icon;
}

[System.Serializable]
public class StorySetter
{
    public int code;
    public int startCode;
    public int endCode;
}

public struct StoryDatas
{
    public int ID;
    public int charName;
    public int storyScript;
}


[System.Serializable]
public class SpecialStoryObjects
{
    public int ID;
    public GameObject prefab;
}