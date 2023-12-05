using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTable : DataTable
{
    public List<PlayerInfo> playerDatabase = null;
    public List<Sprite> playerSprites;
    public List<Sprite> playerFullSprites;

    public PlayerTable() : base(DataType.Player)
    {
    }

    public override void DataAdder()
    {
        if (playerDatabase != null) { return; }

        List<Dictionary<string, string>> players = CSVReader.Read(Path.Combine("CSV", "PlayerStats"));
        playerDatabase = new List<PlayerInfo>();
        playerSprites = new List<Sprite>();
        playerFullSprites = new List<Sprite>();
        foreach (var player in players)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.code = int.Parse(player["Code"]);
            playerInfo.name = player["Name"];
            playerInfo.grade = int.Parse(player["Grade"]);
            playerInfo.type = int.Parse(player["Type"]);
            playerInfo.UniqueSkillCode = int.Parse(player["UniqueSkill"]);
            playerInfo.Potential = int.Parse(player["Type"]);
            playerInfo.info = player["Info"];
            playerInfo.cost = int.Parse(player["Cost"]);
            playerInfo.weaponType = int.Parse(player["WeaponType"]);
            playerInfo.hp = int.Parse(player["minHP"]);
            playerInfo.atk = int.Parse(player["minAtk"]);
            playerInfo.atkRate = float.Parse(player["minAtkRate"]);
            playerInfo.moveSpeed = float.Parse(player["minSpeed"]);
            playerInfo.sight = float.Parse(player["minSight"]);
            playerInfo.range = float.Parse(player["minRange"]);
            playerInfo.criticalChance = float.Parse(player["minCritical"]);
            playerInfo.magazine = int.Parse(player["Mag"]);
            playerInfo.reloadingSpeed = float.Parse(player["Reload"]);
            playerInfo.Accuracy = float.Parse(player["minAccuracy"]);
            playerInfo.reactionSpeed = float.Parse(player["minReaction"]);
            playerInfo.detectionRange = float.Parse(player["minDetection"]);
            playerInfo.atkType = int.Parse(player["AtkType"]);
            playerInfo.kitingType = int.Parse(player["KitingType"]);

            playerDatabase.Add(playerInfo);
            playerSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerSprite", playerInfo.code.ToString())));
            playerFullSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerFullSprite", playerInfo.code.ToString())));
        }
    }
    
    public int PlayerIndexSearch(int code)
    {
        return playerDatabase.FindIndex(player => player.code == code);
    }

    public PlayerInfo GetPlayerInfo(int code)
    {
        return playerDatabase.Find(player => player.code == code);
    }

    public Sprite GetPlayerSprite(int code)
    {
        return playerSprites[PlayerIndexSearch(code)];
    }
    public Sprite GetPlayerFullSprite(int code)
    {
        return playerFullSprites[PlayerIndexSearch(code)];
    }
}
