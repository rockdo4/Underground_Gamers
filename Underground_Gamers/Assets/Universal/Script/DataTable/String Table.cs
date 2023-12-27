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

    public StringTable() : base(DataType.String) { }
    public override void DataAdder()
    {
        if (GamePlayerInfo.instance.language == (int)Language.Kor)
        {
            tables = new List<Dictionary<string, string>>
        { CSVReader.ReadStringTable(Path.Combine("CSV", "StringTable_kr")) };
        }
        else if (GamePlayerInfo.instance.language == (int)Language.Eng)
        {
            tables = new List<Dictionary<string, string>>
        { CSVReader.ReadStringTable(Path.Combine("CSV", "StringTable_eng")) };
        }
    }


    public string Get(string id)
    {
        return tables[(int)language][id];
    }

}
