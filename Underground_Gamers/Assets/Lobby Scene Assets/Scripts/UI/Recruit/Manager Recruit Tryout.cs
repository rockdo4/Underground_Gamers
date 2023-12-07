using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerRecruitTryout : ManagerRecruit
{
    [SerializeField]
    private GameObject moneyList;
    [SerializeField]
    private TMP_Text moneyListText;
    [Space(5f)]
    [Header("Recruit")]
    public Image recruitImage;
    public TMP_Text recruitInfo;
    [SerializeField]
    private int defaultRecruitCode = 0;
    private List<int> outPut;
    [SerializeField]
    private TMP_Text tradePointCost;
    [SerializeField]
    private TMP_Text MoneyText5;
    [SerializeField]
    private TMP_Text ticketRewardText;
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
    private GameObject tryoutBoard;
    [SerializeField]
    private GameObject tryoutCardPrefab;
    private List<GameObject> oldtryoutCards = new List<GameObject>();
    [SerializeField]
    private Transform tryoutUpperPos;
    [SerializeField]
    private Transform tryoutSelectPos;
    [SerializeField]
    private Button tryoutStartButton;
    [SerializeField]
    private GameObject popupTryoutCheck;
    [SerializeField]
    private TMP_Text popupTryoutCheckText;
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

    private RecruitTable rt;
    private PlayerTable pt;
    private StringTable st;

    private int currCode = 0;
    private int currCount;
    private int currCost = 0;
    private int currTryout = -1;
    private int currReward = 5;

    private void Awake()
    {
        oldRecruitCards = new List<GameObject>();
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        ResetIndex();
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public void ResetIndex()
    {
        currCode = defaultRecruitCode;
        ShowIndex(defaultRecruitCode);
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
        MoneyText5.text = $"{info.crystal}";
        ticketRewardText.text = currReward.ToString();
    }

    public void TryTryout()
    {
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());
        currCost = info.crystal;

        //뽑히는 선수 수
        currCount = 4;
        ///////////////////////////////////////////////////////

        int usingList = 0;
        List<Player> used = GamePlayerInfo.instance.usingPlayers;
        foreach (var item in used)
        {
            if (item.code < 0)
            {
                usingList++;
            }
        }

        if (!GamePlayerInfo.instance.CheckMoney(0, currCost, 0))
        {
            string messege = "";
            string submessege = st.Get("recruitMoneyLackMessegeCurr");

            messege += $" {st.Get("crystal")} {currCost - GamePlayerInfo.instance.crystal}{st.Get("count")}";
            submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}"; ;

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
            string messege = "";
            string submessege = st.Get("recruitCheckCurrMessege");

            messege += $" {st.Get("crystal")} {currCost}{st.Get("count")}";
            submessege += $" {st.Get("crystal")} {GamePlayerInfo.instance.crystal}{st.Get("count")}"; ;

            messege += st.Get("recruitCheckMessege");
            recruitCheckWindowMoney.text = messege;
            recruitCheckWindowMoneyCurr.text = submessege;
            recruitCheckWindow.SetActive(true);
            return;
        }
    }

    public void StartTryout()
    {
        if (!GamePlayerInfo.instance.UseMoney(0, currCost, 0))
        {
            return;
        }

        foreach (var card in oldtryoutCards)
        {
            Destroy(card.gameObject);
        }
        oldtryoutCards.Clear();

        outPut = rt.RecruitRandomNoDuples(currCode, currCount);

        bool isHaveUnique = false;
        int counter = 0;
        foreach (int i in outPut)
        {
            var card = Instantiate(tryoutCardPrefab, tryoutUpperPos);
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
            oldtryoutCards.Add(card);

            int index = counter;
            card.GetComponent<Button>().onClick.AddListener(() => SelectTryOut(index));
            counter++;
        }

        if (isHaveUnique)
        {
            Instantiate(recruitEffrctPrefabRare, recruitEffrctPos);
        }
        else
        {
            Instantiate(recruitEffrctPrefabNomal, recruitEffrctPos);
        }

        currTryout = -1;
        tryoutStartButton.interactable = false;
        tryoutBoard.SetActive(true);
    }

    public void SelectTryOut(int index)
    {
        if (currTryout == index)
        {
            currTryout = -1;
            oldtryoutCards[index].transform.SetParent(tryoutUpperPos);
            tryoutStartButton.interactable = false;
        }
        else
        {
            if (currTryout != -1)
            {
                oldtryoutCards[currTryout].transform.SetParent(tryoutUpperPos);
            }
            currTryout = index;
            oldtryoutCards[index].transform.SetParent(tryoutSelectPos);
            tryoutStartButton.interactable = true;
        }
    }

    public void TryRecruit()
    {
        popupTryoutCheckText.text = $"{pt.GetPlayerInfo(outPut[currTryout]).name}{st.Get("check_select")}";
        popupTryoutCheck.SetActive(true);
    }
    public void StartRecruit()
    {
        foreach (var item in oldRecruitCards)
        {
            Destroy(item);
        }
        oldRecruitCards.Clear();

        int playerCode = outPut[currTryout];
        GamePlayerInfo.instance.AddPlayer(playerCode);
        GamePlayerInfo.instance.AddMoney(0,0,currReward);

       var card = Instantiate(recruitCardPrefab, recruitCardPos);
        card.GetComponent<RecruitCards>().image.sprite = pt.GetPlayerSprite(playerCode);
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
        moneyList.SetActive(false);
        UpdateMoneyInfo();
        LobbyUIManager.instance.UpdateMoneyInfo();
    }

    public void RecruitEffrctNextPlayer()
    {
        int currMakeCode = outPut[currTryout];
        PlayerInfo pi = pt.GetPlayerInfo(currMakeCode);
        recruitEffrctCharImage.sprite = pt.GetPlayerFullSprite(currMakeCode);
        recruitEffrctTypeImage.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", pi.type.ToString()));
        recruitEffrctName.text = pi.name;
    }

    public void UpdateMoneyInfo()
    {
        moneyListText.text = GamePlayerInfo.instance.crystal.ToString();
    }

}
