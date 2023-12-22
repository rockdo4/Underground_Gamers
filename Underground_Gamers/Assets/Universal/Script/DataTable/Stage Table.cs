
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public struct OfficialRewards
{
    public int id;
    public int[] reward;
}
public struct ScreanRewards
{
    public int id;
    public int[] min;
    public int[] max;
}
public class StageTable : DataTable
{
    List<StageInfo> stageInfoTable = new List<StageInfo>();
    List<EnemyInfo> enemyInfoTable = new List<EnemyInfo>();
    List<GameObject> enemySprite = new List<GameObject>();
    List<OfficialRewards> officialRewards = new List<OfficialRewards>();
    List<ScreanRewards> screanRewards = new List<ScreanRewards>();
    List<PlayerInfo> officialPlyaers = new List<PlayerInfo>();
    List<EnemyInfo> scrimmPlyaers = new List<EnemyInfo>();
    private StringTable str;
    public StageTable() : base(DataType.Stage)
    {
    }

    public override void DataAdder()
    {
        stageInfoTable = new List<StageInfo>();
        enemyInfoTable = new List<EnemyInfo>();
        enemySprite = new List<GameObject>();
        officialRewards = new List<OfficialRewards>();
        screanRewards = new List<ScreanRewards>();
        officialPlyaers = new List<PlayerInfo>();
        scrimmPlyaers = new List<EnemyInfo>();

        List<Dictionary<string, string>> stages = CSVReader.Read(Path.Combine("CSV", "story_stage_table"));
        foreach (var stage in stages)
        {
            StageInfo newStage = new StageInfo();
            newStage.code = int.Parse(stage["StageID"]);
            newStage.name = stage["StageName"];
            newStage.type = int.Parse(stage["StageType"]);
            newStage.enemys = new List<int>();

            //추가 예정
            newStage.mapCode = stage["StageNum"];
            newStage.rewards = new List<int>
            {
                int.Parse(stage["Exp1"]),
                int.Parse(stage["Exp2"]),
                int.Parse(stage["Exp3"]),
                int.Parse(stage["Exp4"]),
                int.Parse(stage["Gold"]),
                int.Parse(stage["Juwel"])
            };
            newStage.xp = int.Parse(stage["ExpReward"]);

            for (int i = 0; i < 5; i++)
            {
                newStage.enemys.Add(int.Parse(stage[$"StageMon{i + 1}"]));
            }
            stageInfoTable.Add(newStage);
        }

        List<Dictionary<string, string>> enemys = CSVReader.Read(Path.Combine("CSV", "monter_table"));
        foreach (var enemy in enemys)
        {
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.code = int.Parse(enemy["MonID"]);
            enemyInfo.name = enemy["MonName"];
            enemyInfo.grade = int.Parse(enemy["Grade"]);
            enemyInfo.uniqueSkill = int.Parse(enemy["UniqueSkill"]);
            enemyInfo.type = int.Parse(enemy["Type"]);
            enemyInfo.info = enemy["Info"];
            enemyInfo.weaponType = int.Parse(enemy["WeaponType"]);
            enemyInfo.atkType = int.Parse(enemy["AtkType"]);
            enemyInfo.kitingType = int.Parse(enemy["KitingType"]);
            enemyInfo.mag = int.Parse(enemy["Mag"]);
            enemyInfo.reload = float.Parse(enemy["Reload"]);
            enemyInfo.hp = int.Parse(enemy["HP"]);
            enemyInfo.atk = int.Parse(enemy["Atk"]);
            enemyInfo.atkRate = float.Parse(enemy["AtkRate"]);
            enemyInfo.moveSpeed = float.Parse(enemy["Speed"]);
            enemyInfo.sight = float.Parse(enemy["Sight"]);
            enemyInfo.range = float.Parse(enemy["Range"]);
            enemyInfo.critical = float.Parse(enemy["Critical"]);
            enemyInfo.accuracy = float.Parse(enemy["Accuracy"]);
            enemyInfo.reaction = float.Parse(enemy["Reaction"]);
            enemyInfo.detection = float.Parse(enemy["Detection"]);
            enemyInfoTable.Add(enemyInfo);

            var childs = Resources.Load<GameObject>(Path.Combine("EnemySpum", $"{enemyInfo.code}")).GetComponentsInChildren<Transform>();
            foreach (var child in childs)
            {
                if (child.name == "HeadSet")
                {
                    GameObject head = child.gameObject;

                    enemySprite.Add(head);
                    break;
                }
            }
        }

        List<Dictionary<string, string>> officialRewardTables = CSVReader.Read(Path.Combine("CSV", "official_reward_table"));
        foreach (var officialRewardTable in officialRewardTables)
        {
            OfficialRewards newList = new OfficialRewards();
            newList.id = int.Parse(officialRewardTable["ID"]);
            newList.reward = new int[6] { 0, 0, 0, 0, 0, 0 };
            newList.reward[0] = int.Parse(officialRewardTable["count3"]);
            newList.reward[1] = int.Parse(officialRewardTable["count4"]);
            newList.reward[2] = int.Parse(officialRewardTable["count5"]);
            newList.reward[3] = int.Parse(officialRewardTable["count6"]);
            newList.reward[4] = int.Parse(officialRewardTable["count2"]);
            newList.reward[5] = int.Parse(officialRewardTable["count1"]);

            officialRewards.Add(newList);
        }

        List<Dictionary<string, string>> scrimmRewardTables = CSVReader.Read(Path.Combine("CSV", "scrimm_reward_table"));
        foreach (var scrimmRewardTable in scrimmRewardTables)
        {
            ScreanRewards newList = new ScreanRewards();
            newList.id = int.Parse(scrimmRewardTable["ID"]);
            newList.min = new int[5] { 0,0,0,0,0 };
            newList.max = new int[5] { 0, 0, 0, 0, 0 };

            for (int i = 0; i < 3; i++)
            {
                int rewardNwm = int.Parse(scrimmRewardTable[$"reward{i + 1}"]);
                if (rewardNwm > 0)
                {
                    newList.min[rewardNwm - 1] = int.Parse(scrimmRewardTable[$"minCount{i + 1}"]);
                    newList.max[rewardNwm - 1] = int.Parse(scrimmRewardTable[$"maxCount{i + 1}"]);
                }
                else
                {
                    break;
                }
            }
            screanRewards.Add(newList);
        }


        List<Dictionary<string, string>> officialPlayersTable = CSVReader.Read(Path.Combine("CSV", "official_enemy_table"));

        foreach (var player in officialPlayersTable)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.code = int.Parse(player["MonID"]);
            playerInfo.type = int.Parse(player["Type"]);
            playerInfo.UniqueSkillCode = int.Parse(player["UniqueSkill"]);
            playerInfo.Potential = int.Parse(player["Type"]);
            playerInfo.weaponType = int.Parse(player["WeaponType"]);
            playerInfo.hp.min = float.Parse(player["minHP"]);
            playerInfo.atk.min = float.Parse(player["minAtk"]);
            playerInfo.atkRate.min = float.Parse(player["minAtkRate"]);
            playerInfo.moveSpeed.min = float.Parse(player["minSpeed"]);
            playerInfo.sight.min = float.Parse(player["minSight"]);
            playerInfo.range.min = float.Parse(player["minRange"]);
            playerInfo.critical.min = float.Parse(player["minCritical"]);
            playerInfo.magazine = int.Parse(player["Mag"]);
            playerInfo.reloadingSpeed = float.Parse(player["Reload"]);
            playerInfo.accuracy.min = float.Parse(player["minAccuracy"]);
            playerInfo.reactionSpeed.min = float.Parse(player["minReaction"]);
            playerInfo.detectionRange.min = float.Parse(player["minDetection"]);
            playerInfo.atkType = int.Parse(player["AtkType"]);
            playerInfo.kitingType = int.Parse(player["KitingType"]);

            playerInfo.hp.max = float.Parse(player["maxHP"]);
            playerInfo.atk.max = float.Parse(player["maxAtk"]);
            playerInfo.atkRate.max = float.Parse(player["maxAtkRate"]);
            playerInfo.moveSpeed.max = float.Parse(player["maxSpeed"]);
            playerInfo.sight.max = float.Parse(player["maxSight"]);
            playerInfo.range.max = float.Parse(player["maxRange"]);
            playerInfo.critical.max = float.Parse(player["maxCritical"]);
            playerInfo.accuracy.max = float.Parse(player["maxAccuracy"]);
            playerInfo.reactionSpeed.max = float.Parse(player["maxReaction"]);
            playerInfo.detectionRange.max = float.Parse(player["maxDetection"]);
            officialPlyaers.Add(playerInfo);
        }

        List<Dictionary<string, string>> scrimmPlayersTable = CSVReader.Read(Path.Combine("CSV", "scrimm_enemy_table"));

        foreach (var enemy in scrimmPlayersTable)
        {
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.code = int.Parse(enemy["MonID"]);
            enemyInfo.grade = int.Parse(enemy["Grade"]);
            enemyInfo.uniqueSkill = int.Parse(enemy["UniqueSkill"]);
            enemyInfo.type = int.Parse(enemy["Type"]);
            enemyInfo.weaponType = int.Parse(enemy["WeaponType"]);
            enemyInfo.atkType = int.Parse(enemy["AtkType"]);
            enemyInfo.kitingType = int.Parse(enemy["KitingType"]);
            enemyInfo.mag = int.Parse(enemy["Mag"]);
            enemyInfo.reload = float.Parse(enemy["Reload"]);
            enemyInfo.hp = int.Parse(enemy["HP"]);
            enemyInfo.atk = int.Parse(enemy["Atk"]);
            enemyInfo.atkRate = float.Parse(enemy["AtkRate"]);
            enemyInfo.moveSpeed = float.Parse(enemy["Speed"]);
            enemyInfo.sight = float.Parse(enemy["Sight"]);
            enemyInfo.range = float.Parse(enemy["Range"]);
            enemyInfo.critical = float.Parse(enemy["Critical"]);
            enemyInfo.accuracy = float.Parse(enemy["Accuracy"]);
            enemyInfo.reaction = float.Parse(enemy["Reaction"]);
            enemyInfo.detection = float.Parse(enemy["Detection"]);
            scrimmPlyaers.Add(enemyInfo);
        }
    }

    public EnemyInfo GetEnemyInfo(int code)
    {
        return enemyInfoTable.Find(player => player.code == code);
    }

    public StageInfo GetStageInfo(int code)
    {
        return stageInfoTable.Find(stage => stage.code == code);
    }

    public GameObject GetHeadSet(int code)
    {
        return enemySprite[enemyInfoTable.IndexOf(enemyInfoTable.Find(stage => stage.code == code))];
    }

    public List<EnemyInfo> MakeOfficialEnemies(int level)
    {
        if (str == null)
        {
            str = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        List <EnemyInfo> newEnemies = new List<EnemyInfo>();

        List<int> codes = new List<int>();
        for (int i = 0; i < 5; i++)
        {

        }

        //var ids = GenerateRandomNumbers();
        //officialPlyaers.Find(a=>a.code == GenerateRandomNumbers())

        for (int i = 0; i < 5; i++)
        {
            EnemyInfo newPlayer = new EnemyInfo();
            newPlayer.name = str.Get($"random_player_name{UnityEngine.Random.Range(0, 99)}");
            newEnemies.Add(newPlayer);

        }
        
        return newEnemies;
    }

    static List<int> GenerateRandomNumbers(int minValue, int maxValue, int count)
    {
        if (count > maxValue - minValue + 1 || count < 0)
        {
            throw new ArgumentOutOfRangeException("count", "Count should be between 0 and the range of values (maxValue - minValue + 1).");
        }

        List<int> numbers = new List<int>();

        // 중복 없이 count 개의 무작위 숫자 생성
        for (int i = 0; i < count; i++)
        {
            int randomNumber;

            do
            {
                randomNumber = UnityEngine.Random.Range(minValue, maxValue);
            } while (numbers.Contains(randomNumber));

            numbers.Add(randomNumber);
        }

        return numbers;
    }
}
