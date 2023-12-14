
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StageTable : DataTable
{
    List<StageInfo> stageInfoTable = new List<StageInfo>();
    List<EnemyInfo> enemyInfoTable = new List<EnemyInfo>();
    List<GameObject> enemySprite = new List<GameObject>();
    public StageTable() : base(DataType.Stage)
    {
    }

    public override void DataAdder()
    {
        stageInfoTable = new List<StageInfo>();
        enemyInfoTable = new List<EnemyInfo>();
        enemySprite = new List<GameObject>();
        List<Dictionary<string, string>> stages = CSVReader.Read(Path.Combine("CSV", "story_stage_table"));
        foreach (var stage in stages)
        {
            StageInfo newStage = new StageInfo();
            newStage.code = int.Parse(stage["StageID"]);
            newStage.name = stage["StageName"];
            newStage.type = int.Parse(stage["StageType"]);
            newStage.enemys = new List<int>();

            //추가 예정
            newStage.mapCode = 0;
            newStage.rewards = new List<int>();
            ///////////////////////

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
                    //head.layer = LayerMask.NameToLayer("OverUI");

                    enemySprite.Add(head);
                    break;
                }
            }
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
}
