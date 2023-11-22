using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLoadManager : MonoBehaviour
{
    [HideInInspector]
    public List<GearInfo> itemDatabase = null;
    [HideInInspector]
    public List<Sprite> playerSprites;
    void Start()
    {
        ItemAdder();
        //foreach (var item in itemDatabase) { Debug.Log(item.name); }
    }

    private void ItemAdder()
    {
        if (itemDatabase != null) { return; }

        List<Dictionary<string, object>> items = CSVReader.Read(Path.Combine("CSV", "ItemStats"));
        itemDatabase = new List<GearInfo>();
        playerSprites = new List<Sprite>();
        foreach (var item in items)
        {
            GearInfo itemInfo = new GearInfo();
            itemInfo.code = (int)item["Code"];
            itemInfo.name = (string)item["Name"];
            itemDatabase.Add(itemInfo);
            playerSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerSprite", itemInfo.code.ToString())));
        }
    }

}
