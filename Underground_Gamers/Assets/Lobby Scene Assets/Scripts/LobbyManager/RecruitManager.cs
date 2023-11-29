using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManager : MonoBehaviour
{
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
    private GameObject moneyWarning;

    [Space(5f)]
    [Header("RecruitEffect")]
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
    }

    public void ShowIndex(int code)
    {
        currCode = code;
        if (rt == null || pt == null)
        {
            rt = DataTableManager.instance.Get<RecruitTable>(DataType.Recruit);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }

        recruitImage.sprite = Resources.Load<Sprite>(Path.Combine("RecruitSprite", currCode.ToString()));
        recruitInfo.text = rt.GetRecruitInfo(code.ToString()).info;
    }

    public void StartRecruit(int count)
    {
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
}
