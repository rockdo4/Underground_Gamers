//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTrainingAnalyze : ManagerTraining
{
    [SerializeField]
    private Transform upperUITransform;
    [SerializeField]
    private SwipeDetector swipeDetector;
    [SerializeField]
    private GameObject popupMoneyWarning;
    [SerializeField]
    private List<TMP_Text> popupMoneyWarningTexts;

    [Space(10f)]
    [Header("Left")]
    [SerializeField]
    private TMP_Text slotType;
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private TMP_Text slotName;
    [SerializeField]
    private GameObject slotCover;

    [Space(10f)]
    [Header("Right")]
    [SerializeField]
    private Transform charCardTransform;
    [SerializeField]
    private GameObject charCardPrefab;
    [SerializeField]
    private GameObject growInfoArea;
    [SerializeField]
    private List<TMP_Text> growInfoDatas = new List<TMP_Text>();
    [SerializeField]
    private Slider xpBar;
    [SerializeField]
    private List<Button> growItems = new List<Button>();
    [SerializeField]
    private List<TMP_Text> growItemCounts = new List<TMP_Text>();
    [SerializeField]
    private List<GameObject> growItemUseCounter = new List<GameObject>();
    [SerializeField]
    private List<TMP_Text> growItemUseCounterText = new List<TMP_Text>();
    [SerializeField]
    private TMP_Text growItemUseMoneyText;
    [SerializeField]
    private GameObject popupPlayerInfo;
    [SerializeField]
    private List<TMP_Text> playerInfoDatas = new List<TMP_Text>();
    [SerializeField]
    private Button analyzeStartB;
    [SerializeField]
    private GameObject popupFullLevel;

    private PlayerTable pt;
    private StringTable st;
    private List<GameObject> MadeBList = new List<GameObject>();
    private List<Player> sortedPlayerList = new List<Player>();
    private List<int> xpItems;

    private int currIndex = 0;
    private PlayerInfo currPlayerInfo;
    private Player currPlayer;

    private float currXp = 0;
    private float currMaxXp = 100;
    private int currlevel = 0;
    private int currCost = 0;
    private List<int> currXpItemUse = new List<int>();
    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
        swipeDetector.OnSwipeLeft += SwipeLeftCard;
        swipeDetector.OnSwipeRight += SwipeRightCard;
        swipeDetector.isSwipeable = false;
        for (int i = 0; i < 4; i++)
        {
            currXpItemUse.Add(0);
        }
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        TrainingUIManager.instance.lobbyTopMenu.transform.SetParent(upperUITransform);
        LoadPlayers();
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    private void LoadPlayers()
    {
        if (pt == null || st == null)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        foreach (var item in MadeBList)
        {
            Destroy(item);
        }
        MadeBList.Clear();

        slotCover.SetActive(true);
        growInfoArea.SetActive(false);

        List<Player> playerList = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.GetUsingPlayers();
        foreach (var item in usingPlayers)
        {
            if (item.code != -1 && item.level < item.maxLevel)
            {
                playerList.Add(item);
            }
        }
        foreach (var item in GamePlayerInfo.instance.havePlayers)
        {
            if (item.level < item.maxLevel)
            {
                playerList.Add(item);
            }
        }

        sortedPlayerList = playerList.OrderByDescending(p => p.level)
    .ThenByDescending(p => p.breakthrough)
    .ThenByDescending(p => p.grade)
    .ThenByDescending(p => p.type)
    .ThenByDescending(p => p.name)
    .ToList();

        int count = 0;
        foreach (var player in sortedPlayerList)
        {
            var bt = Instantiate(charCardPrefab, charCardTransform);
            var card = bt.GetComponent<PlayerLevelCard>();
            card.image.sprite = pt.GetPlayerSprite(player.code);
            card.level.text = $"Lv. {player.level.ToString("F0")}";

            int index = count++;
            bt.GetComponent<Button>().onClick.AddListener(() => OpenPlayerGrowInfo(index));
            if (player.breakthrough <= 0)
            {
                card.breakImage.gameObject.SetActive(false);
            }
            else
            {
                card.breakImage.gameObject.SetActive(true);
                card.breakImage.sprite = player.breakthrough switch
                {
                    1 => pt.berakSprites[0],
                    2 => pt.berakSprites[1],
                    3 => pt.berakSprites[2],
                    _ => pt.berakSprites[0],
                };
            }

            card.typeIcon.sprite = pt.playerTypeSprites[player.type - 1];
            card.stars.sprite = pt.starsSprites[player.grade - 3];
            card.isUsing.color = Color.red;
            foreach (var item in GamePlayerInfo.instance.usingPlayers)
            {
                if (item.ID == player.ID)
                {
                    card.isUsing.color = Color.green;
                    break;
                }
            }
            card.playerName.text = player.name;

            MadeBList.Add(bt);
        }

        foreach (var item in growItemUseCounter)
        {
            item.SetActive(false);
        }
    }

    public void OpenPlayerGrowInfo(int index)
    {
        growInfoArea.SetActive(true);
        swipeDetector.isSwipeable = true;
        currIndex = index;

        currPlayer = sortedPlayerList[index];
        currPlayerInfo = pt.GetPlayerInfo(currPlayer.code);

        currXp = currPlayer.xp;
        currMaxXp = currPlayer.maxXp;
        currlevel = currPlayer.level;
        currCost = 0;
        for (int i = 0; i < currXpItemUse.Count; i++)
        {
            currXpItemUse[i] = 0;
        }

        slotType.text = st.Get($"type{currPlayer.type.ToString("F0")}");
        slotImage.sprite = pt.GetPlayerSprite(currPlayer.code);
        slotName.text = currPlayer.name;
        slotCover.SetActive(false);

        currXp = currPlayer.xp;
        currMaxXp = currPlayer.maxXp;

        growInfoDatas[0].text = $"Lv.{currPlayer.level.ToString("F0")}";
        growInfoDatas[1].text = $"Lv.{currPlayer.level.ToString("F0")}";
        growInfoDatas[1].color = Color.black;
        growInfoDatas[2].text = $"{currPlayer.xp.ToString("F0")}/{currPlayer.maxXp.ToString("F0")}";
        xpBar.value = currPlayer.xp / currPlayer.maxXp;
        growInfoDatas[3].text = $"{pt.CalculateCurrStats(currPlayerInfo.hp, currPlayer.level).ToString("F0")}";
        growInfoDatas[4].text = $"{pt.CalculateCurrStats(currPlayerInfo.hp, currPlayer.level).ToString("F0")}";
        growInfoDatas[5].text = $"{pt.CalculateCurrStats(currPlayerInfo.atk, currPlayer.level).ToString("F0")}";
        growInfoDatas[6].text = $"{pt.CalculateCurrStats(currPlayerInfo.atk, currPlayer.level).ToString("F0")}";
        growInfoDatas[7].text = $"{pt.CalculateCurrStats(currPlayerInfo.atkRate, currPlayer.level).ToString("F1")}";
        growInfoDatas[8].text = $"{pt.CalculateCurrStats(currPlayerInfo.atkRate, currPlayer.level).ToString("F1")}";
        growInfoDatas[9].text = $"{pt.CalculateCurrStats(currPlayerInfo.accuracy, currPlayer.level).ToString("F0")}";
        growInfoDatas[10].text = $"{pt.CalculateCurrStats(currPlayerInfo.accuracy, currPlayer.level).ToString("F0")}";

        xpItems = GamePlayerInfo.instance.XpItem;
        for (int i = 0; i < xpItems.Count; i++)
        {
            growItemCounts[i].text = xpItems[i].ToString();
            if (xpItems[i] <= 0)
            {
                growItems[i].interactable = false;
            }
            else
            {
                growItems[i].interactable = true;
            }
        }
        growItemUseMoneyText.text = "G : " + currCost.ToString();

        analyzeStartB.interactable = false;
    }

    public void ExitPlayerGrowInfo()
    {
        swipeDetector.isSwipeable = false;
        growInfoArea.SetActive(false);
        LoadPlayers();
    }

    public void SwipeLeftCard()
    {
        if (currIndex - 1 < 0)
        {
            OpenPlayerGrowInfo(sortedPlayerList.Count - 1);
        }
        else
        {
            OpenPlayerGrowInfo(currIndex - 1);
        }
    }
    public void SwipeRightCard()
    {
        if (currIndex + 1 >= sortedPlayerList.Count)
        {
            OpenPlayerGrowInfo(0);
        }
        else
        {
            OpenPlayerGrowInfo(currIndex + 1);
        }
    }

    public void ActivePopupPlayerInfo()
    {
        popupPlayerInfo.SetActive(true);
        playerInfoDatas[0].text = $"{pt.CalculateCurrStats(currPlayerInfo.hp, currPlayer.level).ToString("F0")}";
        playerInfoDatas[1].text = $"{pt.CalculateCurrStats(currPlayerInfo.atk, currPlayer.level).ToString("F0")}";
        playerInfoDatas[2].text = $"{pt.CalculateCurrStats(currPlayerInfo.atkRate, currPlayer.level).ToString("F1")}";
        playerInfoDatas[3].text = $"{pt.CalculateCurrStats(currPlayerInfo.moveSpeed, currPlayer.level).ToString("F0")}";
        playerInfoDatas[4].text = $"{pt.CalculateCurrStats(currPlayerInfo.sight, currPlayer.level).ToString("F0")}";
        playerInfoDatas[5].text = $"{pt.CalculateCurrStats(currPlayerInfo.range, currPlayer.level).ToString("F0")}";
        playerInfoDatas[6].text = $"{pt.CalculateCurrStats(currPlayerInfo.detectionRange, currPlayer.level).ToString("F0")}";
        playerInfoDatas[7].text = $"{pt.CalculateCurrStats(currPlayerInfo.critical, currPlayer.level).ToString("F1")}";
        playerInfoDatas[8].text = $"{currPlayerInfo.magazine.ToString("F0")}";
        playerInfoDatas[9].text = $"{currPlayerInfo.reloadingSpeed.ToString("F0")}";
        playerInfoDatas[10].text = $"{pt.CalculateCurrStats(currPlayerInfo.accuracy, currPlayer.level).ToString("F0")}";
        playerInfoDatas[11].text = $"{pt.CalculateCurrStats(currPlayerInfo.reactionSpeed, currPlayer.level).ToString("F0")}";
    }

    public void UseXpItems(int type)
    {
        int useItemCount = ++currXpItemUse[type];
        analyzeStartB.interactable = true;

        growItemUseCounter[type].SetActive(true);
        growItemUseCounterText[type].text = useItemCount.ToString();
        if (xpItems[type] <= useItemCount)
        {
            growItems[type].interactable = false;
        }

        currXp += type switch
        {
            0 => 50,
            1 => 400,
            2 => 2000,
            3 => 10000,
            _ => 1
        };

        while (currXp >= currMaxXp)
        {
            currlevel++;
            if (currlevel >= currPlayer.maxLevel)
            {
                currXp = 0;
                growInfoDatas[1].color = Color.red;
                foreach (var item in growItems)
                {
                    item.interactable = false;
                }
                if (currPlayer.maxLevel >= 50)
                {
                    currMaxXp = 0;
                    currCost = pt.GetLevelUpCost(50);
                    break;
                }
                else
                {
                    currMaxXp = pt.GetLevelUpXp(currlevel + 1);
                    currCost = pt.GetLevelUpCost(currlevel);
                }
            }
            else
            {
                currXp = currXp - currMaxXp;
                currMaxXp = pt.GetLevelUpXp(currlevel + 1);
                currCost = pt.GetLevelUpCost(currlevel);
            }
            growItemUseMoneyText.text = "G : " + currCost.ToString();
        }


        growInfoDatas[1].text = $"Lv.{currlevel.ToString("F0")}";
        growInfoDatas[2].text = $"{currXp.ToString("F0")}/{currMaxXp.ToString("F0")}";
        if (currMaxXp != 0)
        {
            xpBar.value = currXp / currMaxXp;
        }
        else
        {
            xpBar.value = Mathf.Min(currXp, 1);
        }
        growInfoDatas[4].text = $"{pt.CalculateCurrStats(currPlayerInfo.hp, currlevel).ToString("F0")}";
        growInfoDatas[6].text = $"{pt.CalculateCurrStats(currPlayerInfo.atk, currlevel).ToString("F0")}";
        growInfoDatas[8].text = $"{pt.CalculateCurrStats(currPlayerInfo.atkRate, currlevel).ToString("F1")}";
        growInfoDatas[10].text = $"{pt.CalculateCurrStats(currPlayerInfo.accuracy, currlevel).ToString("F0")}";
    }

    public void TryAnalyze()
    {
        if (!GamePlayerInfo.instance.UseMoney(currCost, 0, 0))
        {
            string messege = "";
            string submessege = st.Get("recruitMoneyLackMessegeCurr");

            messege += $" {st.Get("money")} {currCost - GamePlayerInfo.instance.money}{st.Get("count")}";
            submessege += $" {st.Get("money")} {GamePlayerInfo.instance.money}{st.Get("count")}";

            messege += st.Get("recruitMoneyLackMessegeUpgrade");
            popupMoneyWarningTexts[0].text = messege;
            popupMoneyWarningTexts[1].text = submessege;
            popupMoneyWarning.SetActive(true);
            return;
        }
        else
        {
            StartAnalyze();
        }
    }

    public void StartAnalyze()
    {
        GamePlayerInfo.instance.AnalyzePlayer(currPlayer, currlevel, currXp, currMaxXp);

        for (int i = 0; i < 4; i++)
        {
            GamePlayerInfo.instance.XpItem[i] -= currXpItemUse[i];
        }
        LoadPlayers();
        currIndex = sortedPlayerList.IndexOf(sortedPlayerList.Find(a => a.ID == currPlayer.ID));
        if (currlevel < currPlayer.maxLevel)
        {
            OpenPlayerGrowInfo(currIndex);
        }
        else
        {
            popupFullLevel.SetActive(true);
        }
        LobbyUIManager.instance.UpdateMoneyInfo();
    }
}
