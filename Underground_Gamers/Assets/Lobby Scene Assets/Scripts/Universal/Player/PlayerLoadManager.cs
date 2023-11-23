using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadManager : MonoBehaviour
{
    
    [HideInInspector]
    public List<PlayerInfo> playerDatabase = null;
    [HideInInspector]
    public List<Sprite> playerSprites;

    public static PlayerLoadManager instance
    {
        get
        {
            if (playerLoadManager == null)
            {
                playerLoadManager = FindObjectOfType<PlayerLoadManager>();
            }
            return playerLoadManager;
        }
    }

    private static PlayerLoadManager playerLoadManager;
    void Start()
    {
        PlayerAdder();
    }

    private void PlayerAdder()
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
            playerDatabase.Add(playerInfo);
            playerSprites.Add(Resources.Load<Sprite>(
                Path.Combine("PlayerSprite", playerInfo.code.ToString())));
        }
    }
    
    public int PlayerIndexSearch(int code)
    {
        return playerDatabase.FindIndex(player => player.code == code);
    }

    //private void ButtonMaker()
    //{
    //    for (int i = 0; i < playerDatabase.players.Count; i++)
    //    {
    //        var pic = Instantiate(playerCardPrefab);
    //        pic.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine("PlayerSprite", playerDatabase.players[i].playerCode.ToString()));
    //        Debug.Log(Path.Combine("PlayerSprite", playerDatabase.players[i].playerCode.ToString()));
    //    }
    //}
}
