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
    private TMP_Text playerInfo;

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
            if (GamePlayerInfo.instance.officialWeekNum <7)
            {
                GamePlayerInfo.instance.CalculateOfficialPlayer(false,0,2);
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
    }

    public void InitBaseUIWithMotion()
    {
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
            charImage.sprite = pt.GetPlayerSprite(players[0].code);
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
        charImage.sprite = pt.GetPlayerSprite(players[currIndex].code);
    }
    public void LobbyCharRight()
    {
        currIndex = (currIndex + 1) % players.Count;
        charImage.sprite = pt.GetPlayerSprite(players[currIndex].code);
    }

    public void UpdateProfile()
    {
        int code = GamePlayerInfo.instance.representativePlayer;
        if (code < 0)
        {
            portrait.sprite = nullPortrait;
            playerInfo.text = $"{GamePlayerInfo.instance.playername}" +
                     $"\n{st.Get("representative_player")} : X";
        }
        else
        {
            portrait.sprite = pt.GetPlayerSprite(code);
            playerInfo.text = $"{GamePlayerInfo.instance.playername}" +
                     $"\n{st.Get("representative_player")} : {pt.GetPlayerInfo(code).name}";
        }
    }

    public void OpenOption()
    {
        OptionUI.instance.OpenOption();
    }
}