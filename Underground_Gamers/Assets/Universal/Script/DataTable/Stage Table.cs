using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTable : DataTable
{
    List<StageInfo> stageInfoTable = new List<StageInfo>();   
    public StageTable() : base(DataType.Stage)
    {
    }

    public override void DataAdder()
    {
        stageInfoTable = new List<StageInfo>();
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
    }
}
