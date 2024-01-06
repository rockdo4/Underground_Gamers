using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum Language
{
    Kor,
    Eng,
    Count
}

public class StringTable : DataTable
{
    private List<Dictionary<string, string>> tables;
    private List<Dictionary<string, string>> storyTextTables;
    private List<StoryDatas> storyInfoTables;

    public StringTable() : base(DataType.String) { }
    public override void DataAdder()
    {
        tables = new List<Dictionary<string, string>>
        {
            CSVReader.ReadStringTable(Path.Combine("CSV", "StringTable_kr")),
            CSVReader.ReadStringTable(Path.Combine("CSV", "StringTable_eng"))
        };
        storyTextTables = new List<Dictionary<string, string>>
        {
            CSVReader.ReadStringTable(Path.Combine("CSV", "storystring_kr")),
            CSVReader.ReadStringTable(Path.Combine("CSV", "storystring_eng"))
        };
        storyInfoTables = new List<StoryDatas>();
        var storystable = CSVReader.Read(Path.Combine("CSV", "storystable"));
        foreach (var item in storystable)
        {
            StoryDatas newData = new StoryDatas();
            newData.ID = int.Parse(item["storyID"]);
            newData.charName = int.Parse(item["charName"]);
            newData.storyScript = int.Parse(item["storyScirpt"]);
            storyInfoTables.Add(newData);
        }
    }


    public string Get(string id)
    {
        return tables[GamePlayerInfo.instance.language][id];
    }

    public (string,string) GetStory(int id)
    {
        StoryDatas findData = storyInfoTables.Find(a => a.ID == id);
        return (storyTextTables[GamePlayerInfo.instance.language][findData.charName.ToString()], storyTextTables[GamePlayerInfo.instance.language][findData.storyScript.ToString()]);
    }
}
