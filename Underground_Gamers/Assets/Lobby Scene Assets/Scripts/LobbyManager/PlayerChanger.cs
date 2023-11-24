using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChanger : MonoBehaviour
{
    public GameObject playerButtons;
    
    public GameObject havePlayerSpace;

    public List<UIPlayerSlots> usingSlots;

    public List<Player> haveList;
    public List<Player> usingList;

    private int currentSlotIndex = 0;
    private LobbyUIManager lobbyUIManager;
    private List<GameObject> olds = new List<GameObject>();
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


    private void Awake()
    {
        lobbyUIManager = GetComponent<LobbyUIManager>();
    }
    public void SlotChecker()
    {
        usingList = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 8; i++) 
        {
            if(usingList[i].code < 0)
            {
                usingSlots[i].FrontPanel.gameObject.SetActive(true);
                usingSlots[i].image.sprite = null;
            }
            else
            {
                usingSlots[i].FrontPanel.gameObject.SetActive(false);
                usingSlots[i].image.sprite = PlayerLoadManager.instance.playerSprites[PlayerLoadManager.instance.PlayerIndexSearch(usingList[i].code)];
            }
            
        }

    }
    public void StartChange(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        GamePlayerInfo.instance.SortPlayersWithGrade();
        haveList = GamePlayerInfo.instance.havePlayers;
        usingList = GamePlayerInfo.instance.usingPlayers;

        foreach (var old in olds)
        {
            Destroy(old.gameObject);
        }
        olds.Clear();


        int index = 0;
        foreach (var player in haveList)
        {
            int currIndex = index;
            var bt = Instantiate(playerButtons, havePlayerSpace.transform);
            var pb = bt.GetComponent<PlayerButtons>();
            pb.SetImage(PlayerLoadManager.instance.playerSprites[player.code]);
            pb.GetComponent<Button>().onClick.AddListener(() => ToUse(currIndex));
            pb.GetComponent<Button>().onClick.AddListener(() => lobbyUIManager.ActivePlayerSlotSet(false));
            Debug.Log(index);
            pb.index = index++;
            olds.Add(bt);
        }

        index = 0;
    }

    public void ToUse(int index)
    {
        if (usingList[currentSlotIndex].code > 0)
        {
            haveList.Add(usingList[currentSlotIndex]);
            GamePlayerInfo.instance.RemoveUsePlayer(currentSlotIndex);
        }
        usingList[currentSlotIndex] = haveList[index];
        haveList.Remove(haveList[index]);
        SlotChecker();
    }

    public void ToHave()
    {
        if (usingList[currentSlotIndex].code > 0)
        {
            haveList.Add(usingList[currentSlotIndex]);
            GamePlayerInfo.instance.RemoveUsePlayer(currentSlotIndex);
        }
        SlotChecker();
    }
}
