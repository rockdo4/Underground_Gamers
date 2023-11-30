using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [Header("UIGroup : Lobby")]
    public GameObject lobby;
    public List<GameObject> lobbyNoMoney;
    public GameObject money;
    public GameObject playerList;
    public GameObject playerSlotSet;
    public GameObject playerSlotSortWindow;
    public GameObject playerSlotLackWarning;
    public GameObject playerInfo;
    public List<Toggle> presets;
    public List<Toggle> playerlistLeftToggles;

    [Space(10f)]
    [Header("UIGroup : Schedule")]
    public GameObject schedule;
    public GameObject stage;
    public GameObject playerCountLackWarning;

    [Header("LobbyUI")]
    [SerializeField]
    private List<TMP_Text> MoneyList;

    private EntryFilter filter;

    [HideInInspector]
    public int PlayerInfoIndex = 0;
    [HideInInspector]
    public bool isUsingPlayer = false;
    public static LobbyUIManager instance
    {
        get
        {
            if (lobbyUIManager == null)
            {
                lobbyUIManager = FindObjectOfType<LobbyUIManager>();
            }
            return lobbyUIManager;
        }
    }

    private static LobbyUIManager lobbyUIManager;
  

    private void Start()
    {
        UpdateMoneyInfo();
        filter = GetComponent<EntryFilter>();
        GameInfo.instance.DeletePlayers();
    }
    public void UpdateMoneyInfo()
    {
        if (MoneyList.Count >= 3)
        {
            MoneyList[0].text = GamePlayerInfo.instance.money.ToString();
            MoneyList[1].text = GamePlayerInfo.instance.crystal.ToString();
            MoneyList[2].text = GamePlayerInfo.instance.contractTicket.ToString();
        }
    }

    public void ActiveLobby(bool on)
    {
        lobby.SetActive(on);
    }

    public void ActiveLobyWithoutMoney(bool on)
    {
        foreach (GameObject obj in lobbyNoMoney)
        {
            obj.SetActive(on);
        }
    }

    public void ActiveMoney(bool on)
    {
        money.SetActive(on);
    }

    public void ActivePlayerList(bool on)
    {
        if (on)
        {
            presets[GamePlayerInfo.instance.PresetCode].isOn = true;
            PlayerChanger.instance.SlotChecker();
        }
        else if(!PlayerChanger.instance.IsFullSquad())
        {
            playerSlotLackWarning.SetActive(true);
            return;
        }
        playerList.SetActive(on);
    }

    public void ActivePlayerListAnyway(bool on)
    {
        playerList.SetActive(on);
    }

    public void ActivePlayerSlotSet(bool on)
    {
        playerSlotSet.SetActive(on);
        if (!on)
        {
            filter.ResetSortStandard();
            filter.ResetToggleList();
        }
    }
    public void ActivePlayerSortWindow(bool on)
    {
        playerSlotSortWindow.SetActive(on);
    }

    public void ActivePlayerInfo(bool on, int index, bool isUsing)
    {
        PlayerInfoIndex = index;
        isUsingPlayer = isUsing;
        playerInfo.SetActive(on);
    }

    public void PresetChange()
    {
        for (int i = 0; i < presets.Count; i++)
        {
            if (presets[i].isOn)
            {
                GamePlayerInfo.instance.LoadPreset(i);
                return;
            }
        }
    }


    public void OpenPlayerReleaseImmediate()
    {
        ActivePlayerList(true);
        playerlistLeftToggles[1].isOn = true;
    }

    //------------------------------------------------//

    public void ActiveSchedule(bool on)
    {
        schedule.SetActive(on);
    }

    public void ActiveStage(bool on)
    {
        stage.SetActive(on);
    }

    public void StartGame()
    {
        List<Player> list = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 5; i++)
        {
            if (list[i].code == -1)
            {
                ActivePlayerCountLackWarning(true);
                return;
            }
        }
        GameInfo.instance.RegistPlayers();
        SceneManager.LoadScene("Game Scene");
    }

    public void ActivePlayerCountLackWarning(bool on)
    {
        playerCountLackWarning.SetActive(on);
    }

    public void ActivePlayerSlotLackWarning(bool on)
    {
        playerSlotLackWarning.SetActive(on);
    }
}
