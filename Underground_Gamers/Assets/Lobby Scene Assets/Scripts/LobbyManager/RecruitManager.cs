using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject moneyList;
    [SerializeField]
    private List<TMP_Text> moneyListText;
    [Space(5f)]
    [Header("Recruit")]
    [SerializeField]
    private GameObject recruitList;
    private List<Toggle> recruitListToggles = new List<Toggle>();
    public Image recruitImage;
    public TMP_Text recruitInfo;
    [SerializeField]
    private int defaultRecruitCode = 0;
    private int currCode = 0;
    private int currCount = 0;
    private int maxCount = 1;
    private List<int> outPut;
    [SerializeField]
    private TMP_Text MoneyText1;
    [SerializeField]
    private TMP_Text MoneyText10;
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

    [Space(5f)]
    [Header("RecruitEffect")]
    [SerializeField]
    private GameObject recruitCardBoard;
    [SerializeField]
    private GameObject recruitEffect;
    public GameObject recruitEffrctPrefabNomal;
    public GameObject recruitEffrctPrefabRare;
    [SerializeField]
    private Image recruitEffrctCharImage;
    [SerializeField]
    private Image recruitEffrctTypeImage;
    [SerializeField]
    private Transform recruitEffrctPos;
    [SerializeField]
    public GameObject recruitEffrctWindow;
    private Image recruitEffrctStars;
    [SerializeField]
    private TMP_Text recruitEffrctName;
    [SerializeField]
    private Transform recruitCardPos;
    public GameObject recruitCardPrefab;

    private List<GameObject> oldRecruitCards;
    [SerializeField]
    private GameObject recruitCardEffetRare;
    [SerializeField]
    private GameObject recruitCardEffetUnique;

    private int recruitCount = 0;
    private RecruitTable rt;
    private PlayerTable pt;
    private StringTable st;
    private void Awake()
    {
        var toggles = recruitList.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            recruitListToggles.Add(toggle);
        }
        oldRecruitCards = new List<GameObject>();
    }

    public void ResetIndex()
    {
        currCode = defaultRecruitCode;
        ShowIndex(defaultRecruitCode);
        recruitListToggles[0].isOn = true;
        UpdateMoneyInfo();
    }

    public void ShowIndex(int code)
    {
        currCode = code;
        if (rt == null || pt == null || st == null)
        {
            rt = DataTableManager.instance.Get<RecruitTable>(DataType.Recruit);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());

        recruitImage.sprite = Resources.Load<Sprite>(Path.Combine("RecruitSprite", currCode.ToString()));
        recruitInfo.text = info.info;
        MoneyText1.text = $"{info.money} {info.crystal} {info.contractTicket}";
        MoneyText10.text = $"{info.money * 10} {info.crystal * 10} {info.contractTicket * 10}";
    }

    public void TryRecruit(int count)
    {
        recruitCount = count;
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());
        int needMoney = info.money * recruitCount;
        int needCrystal = info.crystal * recruitCount;
        int needTicket = info.contractTicket * recruitCount;

        if (!GamePlayerInfo.instance.CheckMoney(needMoney, needCrystal, needTicket))
        {
            string messege = "";
            string submessege = st.Get("recruitMoneyLackMessegeCurr");
            if (needMoney - GamePlayerInfo.instance.money> 0)
            {
                messege += $" {st.Get("money")} {needMoney - GamePlayerInfo.instance.money}{st.Get("count")}" ;
                submessege += $"{st.Get("money")} {GamePlayerInfo.instance.money}{st.Get("count")} ";
            }
            if (needCrystal - GamePlayerInfo.instance.crystal > 0)
            {
                messege += $" {st.Get("crystal")} {needCrystal - GamePlayerInfo.instance.crystal}{st.Get("count")}";
                submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}"; ;
            }
            if (needTicket - GamePlayerInfo.instance.contractTicket > 0)
            {
                messege += $" {st.Get("ticket")} {needTicket - GamePlayerInfo.instance.contractTicket}{st.Get("count")}";
                submessege += $" {st.Get("ticket")} {GamePlayerInfo.instance.contractTicket}{st.Get("count")}";
            }
            messege += st.Get("recruitMoneyLackMessege");
            moneyWarningWindowMoney.text = messege;
            moneyWarningWindowMoneyCurr.text = submessege;
            moneyWarning.SetActive(true);
            return;
        }
        else if (GamePlayerInfo.instance.havePlayers.Count + count > 200)
        {
            spaceLackWarning.SetActive(true);
            return;
        }
        else
        {
            string messege = "";
            string submessege = st.Get("recruitCheckCurrMessege");
            if (needMoney > 0)
            {
                messege += $" {st.Get("money")} {needMoney}{st.Get("count")}";
                submessege += $"{st.Get("money")} {GamePlayerInfo.instance.money}{st.Get("count")} ";
            }
            if (needCrystal> 0)
            {
                messege += $" {st.Get("crystal")} {needCrystal}{st.Get("count")}";
                submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}"; ;
            }
            if (needTicket > 0)
            {
                messege += $" {st.Get("ticket")} {needTicket}{st.Get("count")}";
                submessege += $" {st.Get("ticket")} {GamePlayerInfo.instance.contractTicket}{st.Get("count")}";
            }
            messege += st.Get("recruitCheckMessege");
            recruitCheckWindowMoney.text = messege;
            recruitCheckWindowMoneyCurr.text = submessege;
            recruitCheckWindow.SetActive(true);
            return;
        }
    }

    public void StartRecruit()
    {
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());
        if (!GamePlayerInfo.instance.UseMoney(info.money * recruitCount, info.crystal * recruitCount, info.contractTicket * recruitCount))
        {
            return;
        }
        foreach (var card in oldRecruitCards)
        {
            Destroy(card.gameObject);
        }
        oldRecruitCards.Clear();

        

        outPut = rt.RecruitRandom(currCode, recruitCount);
        currCount = recruitCount;
        bool isHaveUnique = false;
        foreach (int i in outPut) 
        {
            GamePlayerInfo.instance.AddPlayer(i);

            var card = Instantiate(recruitCardPrefab, recruitCardPos);
            card.GetComponent<RecruitCards>().image.sprite = pt.GetPlayerSprite(i);
            int grade = pt.GetPlayerInfo(i).grade;
            if (grade >= 5)
            {
                var effect = Instantiate(recruitCardEffetUnique, card.transform);
                effect.transform.SetSiblingIndex(0);
                isHaveUnique = true;
            }
            else if (grade >= 4)
            {
                var effect = Instantiate(recruitCardEffetRare, card.transform);
                effect.transform.SetSiblingIndex(0);
            }
            oldRecruitCards.Add(card);
        }
        //레어 이펙트시 여기서 조건문 처리
        if (isHaveUnique)
        {
            Instantiate(recruitEffrctPrefabRare, recruitEffrctPos);
        }
        else
        {
            Instantiate(recruitEffrctPrefabNomal, recruitEffrctPos);
        }
       
        RecruitEffrctNextPlayer();

        recruitCardBoard.SetActive(true);
        recruitEffect.SetActive(true);
        moneyList.SetActive(false);
        UpdateMoneyInfo();
        LobbyUIManager.instance.UpdateMoneyInfo();
    }

    public void RecruitEffrctNextPlayer()
    {
        if (currCount == 0)
        {
            recruitEffrctWindow.SetActive(false);
            return;
        }
        currCount--;
        int currMakeCode = outPut[currCount];
        PlayerInfo pi = pt.GetPlayerInfo(currMakeCode);
        recruitEffrctCharImage.sprite = pt.GetPlayerFullSprite(currMakeCode);
        recruitEffrctTypeImage.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", pi.type.ToString()));
        recruitEffrctName.text = pi.name;
    }

    public void UpdateMoneyInfo()
    {
        if (moneyListText.Count >= 3)
        {
            moneyListText[0].text = GamePlayerInfo.instance.money.ToString();
            moneyListText[1].text = GamePlayerInfo.instance.crystal.ToString();
            moneyListText[2].text = GamePlayerInfo.instance.contractTicket.ToString();
        }
    }

}
