using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;


public class GearTable : DataTable
{
    public List<GearInfo> itemDatabase = null;
    public List<Sprite> playerSprites;

    public GearTable() : base(DataType.Gear)
    {
    }

    public override void DataAdder()
    {
        if (itemDatabase != null) { return; }

        List<Dictionary<string, string>> items = CSVReader.Read(Path.Combine("CSV", "ItemStats"));
        itemDatabase = new List<GearInfo>();
        playerSprites = new List<Sprite>();
        foreach (var item in items)
        {
            GearInfo itemInfo = new GearInfo();
            itemInfo.code = int.Parse(item["Code"]);
            itemInfo.name = item["Name"];
            itemDatabase.Add(itemInfo);
            playerSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerSprite", itemInfo.code.ToString())));
        }
    }
}
