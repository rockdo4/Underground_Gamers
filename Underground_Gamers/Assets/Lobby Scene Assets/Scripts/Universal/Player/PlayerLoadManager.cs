using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerLoadManager : MonoBehaviour
{
    public PlayerDatabase playerDatabase = null;

    void Start()
    {
        PlayerAdder();
        foreach (var player in playerDatabase.players)
        {
            Debug.Log(player.name);
        }
    }

    private void PlayerAdder()
    {
        if (playerDatabase != null) { return; }

        List<Dictionary<string, object>> players = CSVReader.Read(Path.Combine("CSV", "PlayerStats"));
        playerDatabase = ScriptableObject.CreateInstance<PlayerDatabase>();
        foreach (var player in players)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.playerCode = (int)player["Code"];
            playerInfo.name = (string)player["Name"];
            playerInfo.hp = (int)player["Hp"];
            playerDatabase.players.Add(playerInfo);
        }
    }
}
