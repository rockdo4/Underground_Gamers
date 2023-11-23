using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GamePlayerInfo : MonoBehaviour
{
    public static GamePlayerInfo instance
    {
        get
        {
            if (gamePlayerInfo == null)
            {
                gamePlayerInfo = FindObjectOfType<GamePlayerInfo>();
            }
            return gamePlayerInfo;
        }
    }

    private static GamePlayerInfo gamePlayerInfo;

    public List<Player> havePlayers = new List<Player>();
    public List<Player> usingPlayers = new List<Player>();

    public List<Gear> haveGears = new List<Gear>();
    public List<Gear> usingGears = new List<Gear>();

    [HideInInspector]
    public int cleardStage = 0;

    [Space(10f)]
    [Header("Resource")]
    public int money = 1000;
    public int crystal = 100;
    public int contractTicket = 0; 
    public int stamina = 0;

    public void SortPlayersWithGrade()
    {
        var sortedPeople = havePlayers.OrderBy(p => p.grade).ThenBy(p => p.code);
        havePlayers = sortedPeople.ToList();
    }
}
