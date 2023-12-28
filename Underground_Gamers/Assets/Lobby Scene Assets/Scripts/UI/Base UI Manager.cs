using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BaseUIManager : LobbySceneSubscriber
{
    [Header("PlayerCard")]
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private Sprite nullPortrait;
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text teamName;

    [Space(10f)]
    [Header("Back")]
    [SerializeField]
    private Image charImage;
    [SerializeField]
    private Button[] sideBts = new Button[2];

    [Space(10f)]
    [SerializeField]
    private GameObject popupOfficialEnd;

    private List<Player> players = new List<Player>();
    private int currIndex = 0;
    private PlayerTable pt;
    private StringTable st;
    private MainUIAnimator UIanimator;

    [SerializeField]
    private MenuMover menuMover;
    [SerializeField]
    private PopupRepresentPlayers popupRepresentPlayers;
    public override void OnEnter()
    {
        base.OnEnter();
        LobbyUIManager.instance.ActiveLobby(true);
        lobbyTopMenu.ActiveTop(false);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    void Start()
    {
        InitBaseUI();
        UIanimator = GetComponent<MainUIAnimator>();
        if (GamePlayerInfo.instance.endScrimmage)
        {
            if (GamePlayerInfo.instance.officialWeekNum < 7)
            {
                GamePlayerInfo.instance.CalculateOfficialPlayer(false, 0, 2);
                if (GamePlayerInfo.instance.endScrimmage)
                {
                    GamePlayerInfo.instance.isOnOfficial = false;
                    GamePlayerInfo.instance.endScrimmage = false;
                    popupOfficialEnd.SetActive(true);
                    GamePlayerInfo.instance.SaveFile();
                }
            }
            else
            {
                GamePlayerInfo.instance.isOnOfficial = false;
                GamePlayerInfo.instance.endScrimmage = false;
                popupOfficialEnd.SetActive(true);
                GamePlayerInfo.instance.SaveFile();
            }

        }
        menuMover.StartMenuMove();
    }

    public void InitBaseUIWithMotion()
    {
        if (UIanimator == null)
        {
            UIanimator = GetComponent<MainUIAnimator>();
        }
        UIanimator.DoOpen();
        InitBaseUI();
    }
    public void InitBaseUI()
    {
        currIndex = 0;
        if (pt == null)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        players = GamePlayerInfo.instance.GetUsingPlayers();
        if (players.Count > 0)
        {
            charImage.gameObject.SetActive(true);
            charImage.sprite = pt.GetPlayerStandingSprite(players[0].code);
            bool enables = players.Count != 1;
            sideBts[0].gameObject.SetActive(enables);
            sideBts[1].gameObject.SetActive(enables);

        }
        else
        {
            charImage.gameObject.SetActive(false);
            sideBts[0].gameObject.SetActive(false);
            sideBts[1].gameObject.SetActive(false);
        }
        UpdateProfile();
    }

    public void LobbyCharLeft()
    {
        currIndex = (currIndex - 1 + players.Count) % players.Count;
        charImage.sprite = pt.GetPlayerStandingSprite(players[currIndex].code);
    }
    public void LobbyCharRight()
    {
        currIndex = (currIndex + 1) % players.Count;
        charImage.sprite = pt.GetPlayerStandingSprite(players[currIndex].code);
    }

    public void UpdateProfile()
    {
        if (GamePlayerInfo.instance.representativePlayer >= 0)
        {
            portrait.sprite = pt.GetPlayerSprite(GamePlayerInfo.instance.representativePlayer);
        }
        else
        {
            portrait.sprite = popupRepresentPlayers.nullSprite;
        }
        playerName.text = $"{GamePlayerInfo.instance.playername}";
        teamName.text = $"{GamePlayerInfo.instance.teamName}";
    }

    public void OpenOption()
    {
        OptionUI.instance.OpenOption();
    }
}