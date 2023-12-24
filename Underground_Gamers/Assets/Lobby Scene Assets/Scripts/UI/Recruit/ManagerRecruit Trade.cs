using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerRecruitTrade : ManagerRecruit
{
    [SerializeField]
    private List<TMP_Text> moneyListText;
    [SerializeField]
    private int resetCost = 50;

    [SerializeField]
    private int defaultRecruitCode = 0;
    [SerializeField]
    private GameObject recruitCheckWindow;
    [SerializeField]
    private TMP_Text recruitCheckWindowMoney;
    [SerializeField]
    private TMP_Text recruitCheckWindowMoneyCurr;
    [SerializeField]
    private GameObject spaceLackWarning;
    [SerializeField]
    private GameObject moneyWarning;
    [SerializeField]
    private TMP_Text moneyWarningWindowMoney;
    [SerializeField]
    private TMP_Text moneyWarningWindowMoneyCurr;

    [SerializeField]
    private List<RecruitTradeCard> recruitTradeCards;
    [SerializeField]
    private TMP_Text storeTimer;

    //[SerializeField]
    //private GameObject popupCompleteRecruit;
    //[SerializeField]
    //private List<TMP_Text> popupCompleteTexts;
    //[SerializeField]
    //private List<Image> popupCompleteImages;
    [SerializeField]
    private GameObject popupResetList;
    [SerializeField]
    private TMP_Text popupResetListMoney;
    [SerializeField]
    private TMP_Text popupResetListMoneyCurr;
    [SerializeField]
    private GameObject recruitCardEffetRare;
    [SerializeField]
    private GameObject recruitCardEffetUnique;
    [SerializeField]
    private GameObject recruitCardBoard;
    [SerializeField]
    private GameObject recruitEffect;
    [SerializeField]
    private Image recruitEffrctCharImage;
    [SerializeField]
    private Image recruitEffrctTypeImage;
    [SerializeField]
    private Image recruitEffrctStars;
    [SerializeField]
    private TMP_Text recruitEffrctName;
    [SerializeField]
    private Transform recruitCardPos;
    [SerializeField]
    private GameObject recruitCardPrefab;


    private List<GameObject> oldRecruitCards;
    private RecruitTable rt;
    private PlayerTable pt;
    private StringTable st;

    private TimeSpan timer;
    private DateTime lastResetTime;

    private int currIndex = 0;
    private int currcost = 0;

    private void Start()
    {
        oldRecruitCards = new List<GameObject>();
    }
    public override void OnEnter()
    {
        if (rt == null)
        {
            rt = DataTableManager.instance.Get<RecruitTable>(DataType.Recruit);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        UpdateMoneyInfo();
        MakeCards();
        gameObject.SetActive(true);

    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
        LobbyUIManager.instance.UpdateMoneyInfo();
    }


    private void MakeCards()
    {
        if (GamePlayerInfo.instance.lastRecruitTime.AddDays(1) 
            < DateTime.Now)
        {
            ShuffleCards();
            DateTime now = DateTime.Now;
            GamePlayerInfo.instance.lastRecruitTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
        }

        lastResetTime = GamePlayerInfo.instance.lastRecruitTime;
        
        int count = 0;
        foreach (var card in recruitTradeCards)
        {
            if (GamePlayerInfo.instance.tradeCenter.Count-1 < count )
            {
                break;
            }
            int code = GamePlayerInfo.instance.tradeCenter[count];
            if (code >= 0)
            {
                card.SetCards(code);
            }
            else
            {
                card.CloseCards();
            }
            count++;
        }

        GamePlayerInfo.instance.SaveFile();
    }

    public void ShuffleCards()
    {
        var outPut = rt.RecruitRandomNoDuples(defaultRecruitCode, recruitTradeCards.Count);
        GamePlayerInfo.instance.SetTradeCenter(outPut);
    }

    public void TryResetList()
    {
        if (!GamePlayerInfo.instance.CheckMoney(0, resetCost, 0))
        {
            string messege = "";
            string submessege = st.Get("recruitMoneyLackMessegeCurr");

            messege += $" {st.Get("crystal")} {resetCost - GamePlayerInfo.instance.crystal}{st.Get("count")}";
            submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}";

            messege += st.Get("recruitMoneyLackMessege");
            moneyWarningWindowMoney.text = messege;
            moneyWarningWindowMoneyCurr.text = submessege;
            moneyWarning.SetActive(true);
            return;
        }
        else
        {
            string messege = "";
            string submessege = st.Get("recruitCheckCurrMessege");

            messege += $" {st.Get("crystal")} {resetCost}{st.Get("count")}";
            submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}"; ;

            messege += st.Get("reset_list_check");
            popupResetListMoney.text = messege;
            popupResetListMoneyCurr.text = submessege;
            popupResetList.SetActive(true);
            return;
        }
    }

    public void DoReset()
    {
        if (!GamePlayerInfo.instance.UseMoney(0, resetCost, 0))
        {
            return;
        }
        UpdateMoneyInfo();
        ShuffleCards();
        MakeCards();
    }

    public void TryTrade(int index)
    {
        int usingList = 0;
        List<Player> used = GamePlayerInfo.instance.usingPlayers;
        foreach (var item in used)
        {
            if (item.code < 0)
            {
                usingList++;
            }
        }
        int cost = recruitTradeCards[index].cost;


        if (!GamePlayerInfo.instance.CheckMoney(0, 0, cost))
        {
            string messege = "";
            string submessege = st.Get("recruitMoneyLackMessegeCurr");

            messege += $" {st.Get("ticket")} {cost - GamePlayerInfo.instance.contractTicket}{st.Get("count")}";
            submessege += $" {st.Get("ticket")} {GamePlayerInfo.instance.contractTicket}{st.Get("count")}";

            messege += st.Get("recruitMoneyLackMessege");
            moneyWarningWindowMoney.text = messege;
            moneyWarningWindowMoneyCurr.text = submessege;
            moneyWarning.SetActive(true);
            return;
        }
        else if (GamePlayerInfo.instance.havePlayers.Count + 1 + usingList > 200)
        {
            spaceLackWarning.SetActive(true);
            return;
        }
        else
        {
            currcost = cost;
            currIndex = index;
            string messege = "";
            string submessege = st.Get("recruitCheckCurrMessege");
            messege += $" {st.Get("ticket")} {cost}{st.Get("count")}";
            submessege += $" {st.Get("ticket")} {GamePlayerInfo.instance.contractTicket}{st.Get("count")}";
            messege += st.Get("recruitCheckMessege");
            recruitCheckWindowMoney.text = messege;
            recruitCheckWindowMoneyCurr.text = submessege;
            recruitCheckWindow.SetActive(true);
            return;
        }
    }

    public void DoTrade()
    {
        if (!GamePlayerInfo.instance.UseMoney(0,0, currcost))
        {
            return;
        }

        foreach (var item in oldRecruitCards)
        {
            Destroy(item);
        }
        oldRecruitCards.Clear();

        int playerCode = GamePlayerInfo.instance.tradeCenter[currIndex]; 

        var card = Instantiate(recruitCardPrefab, recruitCardPos);
        card.GetComponent<RecruitCards>().image.sprite = pt.GetPlayerSprite(playerCode);
        var rc = card.GetComponent<RecruitCards>();
        rc.image.sprite = pt.GetPlayerSprite(playerCode);
        PlayerInfo playerInfo = pt.playerDatabase[playerCode];
        rc.stars.sprite = playerInfo.grade switch
        {
            3 => pt.starsSprites[0],
            4 => pt.starsSprites[1],
            5 => pt.starsSprites[2],
            _ => pt.starsSprites[0],
        };
        int grade = pt.GetPlayerInfo(playerCode).grade;
        if (grade >= 5)
        {
            var effect = Instantiate(recruitCardEffetUnique, card.transform);
            effect.transform.SetSiblingIndex(0);
        }
        else if (grade >= 4)
        {
            var effect = Instantiate(recruitCardEffetRare, card.transform);
            effect.transform.SetSiblingIndex(0);
        }
        oldRecruitCards.Add(card);


        RecruitEffrctNextPlayer();

        recruitCardBoard.SetActive(true);
        recruitEffect.SetActive(true);

        GamePlayerInfo.instance.AddPlayer(GamePlayerInfo.instance.tradeCenter[currIndex]);
        var info = pt.GetPlayerInfo(GamePlayerInfo.instance.tradeCenter[currIndex]);
        GamePlayerInfo.instance.tradeCenter[currIndex] = -1;

        recruitCheckWindow.SetActive(false);
        //popupCompleteTexts[0].text = info.name;
        //popupCompleteTexts[1].text = $"{info.name}{st.Get("with1")} {currcost}{st.Get("recruit_with_tradepoint")}";
        //popupCompleteImages[0].sprite = pt.GetPlayerSprite(info.code);
        //popupCompleteImages[1].sprite = info.grade switch
        //{
        //    3 => pt.starsSprites[0],
        //    4 => pt.starsSprites[1],
        //    5 => pt.starsSprites[2],
        //    _ => pt.starsSprites[0],
        //};


        MakeCards();
        UpdateMoneyInfo();
        //popupCompleteRecruit.SetActive(true);
    }

    public void RecruitEffrctNextPlayer()
    {
        int currMakeCode = GamePlayerInfo.instance.tradeCenter[currIndex];
        PlayerInfo pi = pt.GetPlayerInfo(currMakeCode);
        recruitEffrctCharImage.sprite = pt.GetPlayerFullSprite(currMakeCode);
        recruitEffrctTypeImage.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", pi.type.ToString()));
        recruitEffrctName.text = pi.name;
        recruitEffrctStars.sprite = pi.grade switch
        {
            3 => pt.starsSprites[0],
            4 => pt.starsSprites[1],
            5 => pt.starsSprites[2],
            _ => pt.starsSprites[0],
        };
    }

    public void UpdateMoneyInfo()
    {
        moneyListText[0].text = "C : " + GamePlayerInfo.instance.crystal.ToString();
        moneyListText[1].text = "M : " + GamePlayerInfo.instance.contractTicket.ToString();
        LobbyUIManager.instance.UpdateMoneyInfo();
    }

    public void Update()
    {
        timer = lastResetTime.AddDays(1) - DateTime.Now;
        if (timer < TimeSpan.Zero)
        {
            DateTime now = DateTime.Now;
            GamePlayerInfo.instance.lastRecruitTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            lastResetTime = GamePlayerInfo.instance.lastRecruitTime;
            timer = lastResetTime.AddDays(1) - DateTime.Now;
            ShuffleCards();
            MakeCards();
        }
        storeTimer.text = $"{timer.Hours} : {timer.Minutes} : {timer.Seconds}";
    }

}