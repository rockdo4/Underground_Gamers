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
    private PlayerTable pt;
    private StringTable st;

    private void Awake()
    {
        lobbyUIManager = GetComponent<LobbyUIManager>();
    }

    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    public void SlotChecker()
    {
        usingList = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 8; i++) 
        {
            UIPlayerSlots slot = usingSlots[i];
            if(usingList[i].code < 0)
            {
                slot.FrontPanel.gameObject.SetActive(true);
                slot.image.sprite = null;
            }
            else
            {
                Player player = usingList[i];
                slot.FrontPanel.gameObject.SetActive(false);
                slot.image.sprite =pt.playerSprites[pt.PlayerIndexSearch(player.code)];
                if (!slot.isSpare)
                {
                    slot.typeText.text = st.Get($"type{player.type}");
                    slot.levelText.text = $"Lv.{player.level}";
                    slot.nameText.text = st.Get($"playerName{player.code}");
                    slot.skillNameText.text = st.Get($"skillName{player.gearCode}");
                    slot.skillLevelText.text = $"Lv.{player.gearLevel}";
                    slot.xpGauge.value = player.xp / player.maxXp;
                }
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
            pb.SetImage(pt.playerSprites[player.code]);
            pb.GetComponent<Button>().onClick.AddListener(() => ToUse(currIndex));
            pb.GetComponent<Button>().onClick.AddListener(() => lobbyUIManager.ActivePlayerSlotSet(false));
            pb.index = index++;
            pb.playerNameCard.text = st.Get($"playerName{player.code}");
            pb.Level.text = $"Lv.{player.level}";
            olds.Add(bt);
        }

        index = 0;
    }

    public void ToUse(int index)
    {
        if (usingList[currentSlotIndex].code >= 0)
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
        if (usingList[currentSlotIndex].code >= 0)
        {
            haveList.Add(usingList[currentSlotIndex]);
            GamePlayerInfo.instance.RemoveUsePlayer(currentSlotIndex);
        }
        SlotChecker();
    }

    public bool IsFullSquad()
    {
        usingList = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 5; i++)
        {
            if (usingList[i].code == -1)
            {
                return false;
            }
        }
        return true;
    }
}
