
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private List<int> releaseCost;
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
    private GameObject popupCant5Release;
    [SerializeField]
    private PopupDeleteOfficial popupDeleteOfficial;
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

    private StringTable st;

    private int rewardSum = 0;
    private void Awake()
    {
        isReleaseMod = false;
        btList = new List<PlayerButtons>();
        buttonCopys = new List<GameObject>();
        usingReleasePlayers = new List<Player>();
        releasePlayers = new List<Player>();
    }

    private void Start()
    {
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    public void StartReleaseMod()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        isReleaseMod = true;
        selectCountText.text = $"<size=24>{st.Get("curr_selected_player")} : </size><color=\"yellow\">0/10</color>";
    }
    public void EndReleaseMod()
    {
        isReleaseMod = false;
        PlayerChanger.instance.SlotChecker();
        CancelSelect();
    }
    public void SelectForReleaseFunc(PlayerButtons playerButtons)
    {
        if (!isReleaseMod || (btList.Count >= 10 && !playerButtons.willRelease)) { return; }
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
        selectCountText.text = $"<size=24>{st.Get("curr_selected_player")} : </size><color=\"yellow\">{btList.Count}/10</color>";
    }

    public void SelectAllForRelease()
    {
        PlayerButtons[] buttons = relesaseOriginCardsPos.GetComponentsInChildren<PlayerButtons>(false);
        foreach (var item in buttons)
        {
            if (btList.Count >= 10)
            {
                ButtonChecker();
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
        relesaseStartButton.interactable = false;
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        selectCountText.text = $"<size=24>{st.Get("curr_selected_player")} : </size><color=\"yellow\">0/10</color>";
    }

    public void MakeReleaseCheckCards(bool on)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (on)
        {
            rewardSum = 0;
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
                rewardSum += player.grade switch
                {
                    3 => releaseCost[0],
                    4 => releaseCost[1],
                    5 => releaseCost[2],
                    _ => releaseCost[1],
                };
            }
            foreach (var player in usingReleasePlayers)
            {
                rewardSum += player.grade switch
                {
                    3 => releaseCost[0],
                    4 => releaseCost[1],
                    5 => releaseCost[2],
                    _ => releaseCost[1],
                };
            }
            foreach (var item in btList)
            {
                var copyB = Instantiate(item, relesaseCheckCardsPos);
                copyB.GetComponent<Button>().onClick.RemoveAllListeners();
                copyB.gameObject.SetActive(true);
                buttonCopys.Add(copyB.gameObject);
            }
            releaseRewardText.text = $"{st.Get("when_release")} <sprite=2> {rewardSum} {st.Get("get")}";
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

        int playerCount = GamePlayerInfo.instance.havePlayers.Count;
        foreach (var item in GamePlayerInfo.instance.usingPlayers)
        {
            if (item.code != -1)
            {
                playerCount++;
            }
        }
        playerCount = playerCount - (releasePlayers.Count + usingReleasePlayers.Count);

        if (playerCount < 5)
        {
            popupCant5Release.SetActive(true);
            return;
        }

        List<string> names = new List<string>();
        foreach (var player in releasePlayers)
        {
            if (GamePlayerInfo.instance.officialPlayers.Contains(player.ID))
            {
                names.Add(player.name);
            }
        }
        foreach (var player in usingReleasePlayers)
        {
            if (GamePlayerInfo.instance.officialPlayers.Contains(player.ID))
            {
                names.Add(player.name);
            }
        }


        if (names.Count > 0)
        {
            popupDeleteOfficial.ActiveWarning(names);
        }
        else if (isHighLevel)
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
        GamePlayerInfo.instance.AddMoney(0, 0, rewardSum);
        LobbyUIManager.instance.UpdateMoneyInfo();
        ButtonChecker();
        GamePlayerInfo.instance.CheckRepresentPlayers();
    }
}
