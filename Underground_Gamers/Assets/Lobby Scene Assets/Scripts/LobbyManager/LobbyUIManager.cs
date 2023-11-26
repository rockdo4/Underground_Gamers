using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    [Header("UIGroup : Lobby")]
    public GameObject lobby;
    public List<GameObject> lobbyNoMoney;
    public GameObject money;
    public GameObject playerList;
    public GameObject playerSlotSet;
    public GameObject playerSlotLackWarning;

    [Space(10f)]
    [Header("UIGroup : Schedule")]
    public GameObject schedule;
    public GameObject stage;
    public GameObject playerCountLackWarning;
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

    [Header("LobbyUI")]
    [SerializeField]
    private List<TMP_Text> MoneyList;

    private void Start()
    {
        UpdateMoneyInfo();
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
        if (GamePlayerInfo.instance.usingPlayers.Count < 5)
        {
            ActivePlayerCountLackWarning(true);
            return;
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
