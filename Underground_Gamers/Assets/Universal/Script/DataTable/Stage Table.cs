using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageTable : DataTable
{
    List<StageInfo> stageInfoTable = new List<StageInfo>();
    List<EnemyInfo> enemyInfoTable = new List<EnemyInfo>();
    public StageTable() : base(DataType.Stage)
    {
    }

    public override void DataAdder()
    {
        stageInfoTable = new List<StageInfo>();
        enemyInfoTable = new List<EnemyInfo>();
        StageInfo a = new StageInfo();
        a.code = 100;
        a.type = 0;
        a.enemys = new List<int>();
        a.enemys.Add(0);
        a.enemys.Add(0);
        a.enemys.Add(1);
        a.enemys.Add(2);
        a.enemys.Add(3);
        a.rewards = new List<int>();
        a.rewards.Add(0);
        stageInfoTable.Add(a);

        List<Dictionary<string, string>> enemys = CSVReader.Read(Path.Combine("CSV", "monterstats"));
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
        }
    }

    public EnemyInfo GetEnemyInfo(int code)
    {
        return enemyInfoTable.Find(player => player.code == code);
    }
}
