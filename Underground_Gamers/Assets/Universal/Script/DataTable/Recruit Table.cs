using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

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
            result.Add(DoGatcha(data));
        }
        return result;
    }

    public int DoGatcha(Dictionary<string, string> data)
    {
        int count = 1;
        int ratioVal = 0;
        List<(int, int)> results = new List<(int, int)>();
        while (data[$"reward{count}"] != "")
        {
            int ratio = int.Parse(data[$"ratio{count}"]);
            results.Add((int.Parse(data[$"reward{count}"]), ratio));
            ratioVal += ratio;
            count++;
        }
        ratioVal = Random.Range(0, ratioVal);

        int probability = 0;
        int result = -1;
        foreach (var item in results)
        {
            probability += item.Item2;
            if (probability > ratioVal)
            {
                result = item.Item1;
                break;
            }
        }

        if (result == -1 ) 
        {
            Debug.Log("GatchaError!");
            return -1;
        }
        else if (int.Parse(data["end"]) == 0)
        {
            return DoGatcha(gatchaData.Find(data => data["ID"] == result.ToString()));
        }
        else
        {
            return result;
        }
    }

}
