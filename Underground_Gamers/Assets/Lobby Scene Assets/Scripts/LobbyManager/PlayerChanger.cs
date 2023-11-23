using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChanger : MonoBehaviour
{
    public GameObject playerButtons;
    
    public GameObject havePlayerSpace;
    public GameObject usingPlayerSpace;

    public List<Player> haveList;
    public List<Player> usingList;

    public PlayerLoadManager playerLoadManager;

    public static PlayerChanger instance
    {
        get
        {
            if (playerChanger == null)
            {
                playerChanger = FindObjectOfType<PlayerChanger>();
            }
            return playerChanger;
        }
    }

    private static PlayerChanger playerChanger;
    public void StartChange()
    {
        GamePlayerInfo.instance.SortPlayersWithGrade();
        var olds = GameObject.FindGameObjectsWithTag("PlayerButtons");
        foreach (var old in olds)
        {
            Destroy(old.gameObject);
        }
        GamePlayerInfo.instance.SortPlayersWithGrade();

        haveList = GamePlayerInfo.instance.havePlayers;
        usingList = GamePlayerInfo.instance.usingPlayers;

        int index = 0;
        foreach (var player in haveList)
        {
            int currIndex = index;
            var bt = Instantiate(playerButtons, havePlayerSpace.transform);
            var pb = bt.GetComponent<PlayerButtons>();
            pb.SetImage(playerLoadManager.playerSprites[player.code]);
            pb.GetComponent<Button>().onClick.AddListener(() => ToUse(currIndex));
            pb.index = index++;
        }

        index = 0;
        foreach (var player in usingList)
        {
            int currIndex = index;
            var bt = Instantiate(playerButtons, usingPlayerSpace.transform);
            var pb = bt.GetComponent<PlayerButtons>();
            pb.SetImage(playerLoadManager.playerSprites[player.code]);
            pb.GetComponent<Button>().onClick.AddListener(() => ToHave(currIndex));
            pb.index = index++;
        }
    }

    public void ToUse(int index)
    {
        if (usingList.Count >= 8)
        {
            return;
        }

        usingList.Add(haveList[index]);
        haveList.Remove(haveList[index]);
        Debug.Log(index);
        StartChange();
    }

    public void ToHave(int index)
    {
        haveList.Add(usingList[index]);
        usingList.RemoveAt(index);

        StartChange();
    }
}
