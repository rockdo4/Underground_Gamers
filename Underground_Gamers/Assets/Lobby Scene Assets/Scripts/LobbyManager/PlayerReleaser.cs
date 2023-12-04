using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReleaser : MonoBehaviour
{
    public static PlayerReleaser instance
    {
        get
        {
            if (playerReleaser == null)
            {
                playerReleaser = FindObjectOfType<PlayerReleaser>(true);
            }
            return playerReleaser;
        }
    }

    private static PlayerReleaser playerReleaser;

    [SerializeField]
    private TMP_Text selectCountText;
    [SerializeField]
    private TMP_Text releaseRewardText;
    [SerializeField]
    private Transform relesaseCheckCardsPos;
    [SerializeField]
    private GameObject popupReleaseInfo;
    [SerializeField]
    private GameObject popupReleaseCheck;
    [SerializeField]
    private Transform relesaseOriginCardsPos;
    [SerializeField]
    private Button relesaseAllButton;
    [SerializeField]
    private Button relesaseStartButton;
    [SerializeField]
    private PlayersFilter filter;

    [HideInInspector]
    public bool isReleaseMod = false;
    private List<PlayerButtons> btList = new List<PlayerButtons>();
    private List<GameObject> buttonCopys = new List<GameObject>();
    private List<Player> usingReleasePlayers = new List<Player>();
    private List<Player> releasePlayers = new List<Player>();

    private void Awake()
    {
        isReleaseMod = false;
        btList = new List<PlayerButtons>();
        buttonCopys = new List<GameObject>();
        usingReleasePlayers = new List<Player>();
        releasePlayers = new List<Player>();
    }
    public void StartReleaseMod()
    {
        isReleaseMod = true;
    }
    public void EndReleaseMod()
    {
        isReleaseMod = false;
        CancelSelect();
    }
    public void SelectForReleaseFunc(PlayerButtons playerButtons)
    {
        if (!isReleaseMod || btList.Count >= 10) { return; }
        playerButtons.SetReleaseSelect();
        if (playerButtons.willRelease) 
        {
            btList.Add(playerButtons);
        }
        else 
        {
            btList.Remove(playerButtons);
        }
        ButtonChecker();
    }

    public void ButtonChecker()
    {
        bool isAvailableButtons = btList.Count > 0;
        relesaseAllButton.interactable = isAvailableButtons;
        relesaseStartButton.interactable = isAvailableButtons;
    }

    public void SelectAllForRelease()
    {
        PlayerButtons[] buttons = relesaseOriginCardsPos.GetComponentsInChildren<PlayerButtons>(false);
        foreach (var item in buttons)
        {
            if (btList.Count>=10)
            {
                return;
            }
            if (item.willRelease)
            {
                continue;
            }
            item.SetReleaseSelect(true);
            btList.Add(item);
        }
        ButtonChecker();
    }

    public void CancelSelect()
    {
        foreach (var item in btList)
        {
            item.SetReleaseSelect(false);
        }
        btList.Clear();
        relesaseAllButton.interactable = false;
        relesaseStartButton.interactable= false;
    }

    public void MakeReleaseCheckCards(bool on)
    {
        if (on)
        {
            foreach (var item in btList)
            {
                var copyB = Instantiate(item, relesaseCheckCardsPos);
                copyB.GetComponent<Button>().onClick.RemoveAllListeners();
                copyB.gameObject.SetActive(true);
                buttonCopys.Add(copyB.gameObject);
            }
        }
        else
        {
            foreach (var item in buttonCopys)
            {
                Destroy(item);
            }
            buttonCopys.Clear();
        }
    }

    public void TryRelease()
    {
        bool isHighLevel = false;
        var pl1 = from a in GamePlayerInfo.instance.havePlayers
                  join b in btList on a.ID equals b.ID
                 select a;
        var pl2 = from a in GamePlayerInfo.instance.usingPlayers
                  join b in btList on a.ID equals b.ID
                  select a;
        releasePlayers = pl1.ToList();
        usingReleasePlayers = pl2.ToList();

        foreach (var player in releasePlayers)
        {
            if (player.grade > 3 || player.level > 1)
            {
                isHighLevel = true;
            }
        }
        foreach (var player in usingReleasePlayers)
        {
            if (player.grade > 3 || player.level > 1)
            {
                isHighLevel = true;
            }
        }

        if (isHighLevel)
        {
            popupReleaseCheck.SetActive(true);
        }
        else
        {
            popupReleaseInfo.SetActive(false);
            ReleasePlayers();
        }
    }

    public void ReleasePlayers()
    {
        foreach (var player in usingReleasePlayers)
        {
            GamePlayerInfo.instance.RemoveUsePlayer(GamePlayerInfo.instance.usingPlayers.IndexOf(player));
        }
        foreach (var player in releasePlayers)
        {
            GamePlayerInfo.instance.havePlayers.Remove(player);
        }
        PlayerChanger.instance.OpenPlayers();
        filter.Filtering();

        btList.Clear();
        foreach (var item in buttonCopys)
        {
            Destroy(item);
        }
        buttonCopys.Clear();
        releasePlayers.Clear();
        ButtonChecker();
    }
}