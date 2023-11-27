using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTable : DataTable
{
    public List<PlayerInfo> playerDatabase = null;
    public List<Sprite> playerSprites;

    public PlayerTable() : base(DataType.Player)
    {
    }

    public override void DataAdder()
    {
        if (playerDatabase != null) { return; }

        List<Dictionary<string, object>> players = CSVReader.Read(Path.Combine("CSV", "PlayerStats"));
        playerDatabase = new List<PlayerInfo>();
        playerSprites = new List<Sprite>();
        foreach (var player in players)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.code = (int)player["Code"];
            playerInfo.name = (string)player["Name"];
            playerInfo.hp = (int)player["Hp"];
            playerInfo.type = (int)player["Type"];
            playerDatabase.Add(playerInfo);
            playerSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerSprite", playerInfo.code.ToString())));
        }
    }
    
    public int PlayerIndexSearch(int code)
    {
        return playerDatabase.FindIndex(player => player.code == code);
    }
}
