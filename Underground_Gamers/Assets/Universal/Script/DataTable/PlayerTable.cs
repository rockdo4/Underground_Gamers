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
            playerInfo.type = (int)player["Grade"];
            playerInfo.UniqueSkillCode = (int)player["UniqueSkill"];
            playerInfo.Potential = (int)player["Type"];
            playerInfo.info = (string)player["Info"];
            playerInfo.cost = (int)player["Cost"];
            playerInfo.weaponType = (int)player["WeaponType"];
            playerInfo.hp = (int)player["Hp"];
            playerInfo.atk = (int)player["Atk"];
            playerInfo.atkRate = (float)player["AtkRate"];
            playerInfo.moveSpeed = (float)player["Speed"];
            playerInfo.sight = (float)player["Sight"];
            playerInfo.range = (float)player["Range"];
            playerInfo.criticalChance = (float)player["Critical"];
            playerInfo.magazine = (int)player["Mag"];
            playerInfo.reloadingSpeed = (int)player["Reload"];
            playerInfo.Accuracy = (int)player["Accuracy"];
            playerInfo.reactionSpeed = (int)player["Reaction"];
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
