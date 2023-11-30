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
    private RecruitTable rt;
    private PlayerTable pt;
    private List<int> outPut;
    [SerializeField]
    private TMP_Text MoneyText1;
    [SerializeField]
    private TMP_Text MoneyText10;
    [SerializeField]
    private GameObject moneyWarning;

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
        if (rt == null || pt == null)
        {
            rt = DataTableManager.instance.Get<RecruitTable>(DataType.Recruit);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());

        recruitImage.sprite = Resources.Load<Sprite>(Path.Combine("RecruitSprite", currCode.ToString()));
        recruitInfo.text = info.info;
        MoneyText1.text = $"{info.money} {info.crystal} {info.contractTicket}";
        MoneyText10.text = $"{info.money * 10} {info.crystal * 10} {info.contractTicket * 10}";
    }

    public void StartRecruit(int count)
    {
        RecruitInfo info = rt.GetRecruitInfo(currCode.ToString());

        if (!GamePlayerInfo.instance.UseMoney(info.money * count,info.crystal * count, info.contractTicket * count))
        {
            moneyWarning.SetActive(true);
            return;
        }

        foreach (var card in oldRecruitCards)
        {
            Destroy(card.gameObject);
        }
        oldRecruitCards.Clear();

        

        outPut = rt.RecruitRandom(currCode, count);
        currCount = count;
        foreach (int i in outPut) 
        {
            GamePlayerInfo.instance.AddPlayer(i);

            var card = Instantiate(recruitCardPrefab, recruitCardPos);
            card.GetComponent<RecruitCards>().image.sprite = pt.GetPlayerSprite(i);
            oldRecruitCards.Add(card);
        }

        //레어 이펙트시 여기서 조건문 처리
        Instantiate(recruitEffrctPrefabNomal, recruitEffrctPos);
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
        recruitEffrctCharImage.sprite = pt.GetPlayerSprite(currMakeCode);
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
