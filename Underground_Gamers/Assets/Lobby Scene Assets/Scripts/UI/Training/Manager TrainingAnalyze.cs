using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTrainingAnalyze : ManagerTraining
{
    [Header("Left")]
    [SerializeField]
    private TMP_Text slotType;
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private TMP_Text slotName;
    [SerializeField]
    private Button slotCover;

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
    private GameObject popupPlayerInfo;
    [SerializeField]
    private List<TMP_Text> playerInfoDatas = new List<TMP_Text>();
    [SerializeField]
    private Button analyzeStartB;

    private PlayerTable pt;
    private StringTable st;
    private List<GameObject> MadeList = new List<GameObject>();
    private List<Player> sortedPlayerList = new List<Player>();

    private float currXp = 0;
    private float currMaxXp = 100;
    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        LoadPlayers();
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    private void LoadPlayers() 
    {
        slotCover.gameObject.SetActive(true);
        growInfoArea.SetActive(false);

        List<Player> playerList = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.GetUsingPlayers();
        foreach (var item in usingPlayers)
        {
            if (item.code != -1)
            {
                playerList.Add(item);
            }
        }
        playerList.AddRange(GamePlayerInfo.instance.havePlayers);

        sortedPlayerList = playerList.OrderByDescending(p => p.level).ToList();

        int count = 0;
        foreach (var player in sortedPlayerList)
        {
            var bt = Instantiate(charCardPrefab, charCardTransform);
            var card = bt.GetComponent<PlayerLevelCard>();
            card.image.sprite = pt.GetPlayerSprite(player.code);
            card.level.text = $"Lv. {player.level}";

            int index = count++;
            bt.GetComponent<Button>().onClick.AddListener(() => OpenPlayerGrowInfo(index));
        }

        foreach (var item in growItemUseCounter)
        {
            item.SetActive(false);
        }
    }

    public void OpenPlayerGrowInfo(int index)
    {
        growInfoArea.SetActive(true);
        Player player = sortedPlayerList[index];

        slotType.text = st.Get($"type{player.type}");
        slotImage.sprite = pt.GetPlayerSprite(player.code);
        slotName.text = player.name;
        slotCover.gameObject.SetActive(false);

        currXp = player.xp;
        currMaxXp = player.maxXp;

        growInfoDatas[0].text = $"Lv.{player.level}";
        growInfoDatas[1].text = $"Lv.{player.level}";
        growInfoDatas[2].text = $"{player.xp}/{player.maxXp}";
        xpBar.value = player.xp / player.maxXp;
        

        var xpItems = GamePlayerInfo.instance.XpItem;
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

        analyzeStartB.interactable = false;
    }

    public void ExitPlayerGrowInfo()
    {

    }
}
