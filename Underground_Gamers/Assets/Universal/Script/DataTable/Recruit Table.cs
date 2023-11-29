using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecruitTable : DataTable
{
    List<RecruitInfo> info;
    List<Dictionary<string, string>> gatchaData;

    public RecruitTable() : base(DataType.Recruit)
    {
    }

    public override void DataAdder()
    {
        List<Dictionary<string, string>> data = CSVReader.Read(Path.Combine("CSV", "RecruitInfo"));
        info = new List<RecruitInfo>();
        gatchaData = CSVReader.Read(Path.Combine("CSV", "Gatcha_Table"));
        foreach (var item in data)
        {
            RecruitInfo newInfo = new RecruitInfo();
            newInfo.code = item["Code"];
            newInfo.info = item["Info"];
            newInfo.money = int.Parse(item["Money"]);
            newInfo.crystal = int.Parse(item["Crystal"]);
            newInfo.contractTicket = int.Parse(item["Ticket"]);
            info.Add(newInfo);
        }
    }

    public RecruitInfo GetRecruitInfo(string code)
    {
        return info.Find(info => info.code == code);
    }

    public List<int> RecruitRandom(int code,int count)
    {
        List<int> result = new List<int>();
        Dictionary<string, string> data = gatchaData.Find(data => data["ID"] == code.ToString());
        
        for (int i = 0; i < count; i++)
        {
            //da
        }
        return result;
    }

}
