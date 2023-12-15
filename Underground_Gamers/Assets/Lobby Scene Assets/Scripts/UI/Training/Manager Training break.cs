using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTrainingbreak : ManagerTraining
{
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
    private GameObject breakInfoArea;
    [SerializeField]
    private List<TMP_Text> breakInfoTexts = new List<TMP_Text>();
    [SerializeField]
    private Image breakTargetImage;
    [SerializeField]
    private GameObject useCardTogglePrefab;
    [SerializeField]
    private Transform breakIngredientsTransform;
    [SerializeField]
    private GameObject popUpbreakResultArea;
    [SerializeField]
    private Image popUpbreakResultImage;
    [SerializeField]
    private List<TMP_Text> popUpbreakResultTexts = new List<TMP_Text>();
    [SerializeField]
    private GameObject popupWarningUseGrown;

    [SerializeField]
    private Button breakStartB;


    private PlayerTable pt;
    private StringTable st;
    private List<GameObject> MadeBList = new List<GameObject>();
    private List<GameObject> MadeIngredientsList = new List<GameObject>();
    private List<Player> sortedPlayerList = new List<Player>();
    private List<Player> canUsingPlayerList = new List<Player>();

    private List<Player> willDestroyPlayerList = new List<Player>();

    private int currNeedCount;
    private int curruseCount;
    private int currIndex = 0;
    private Player currPlayer;
    private Color disableColor = new Color(0,0,0,0.5f);

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
        breakInfoArea.SetActive(false);

        List<Player> playerList = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.GetUsingPlayers();
        foreach (var item in usingPlayers)
        {
            if (item.code != -1 && item.breakthrough < 3)
            {
                playerList.Add(item);
            }
        }
        foreach(var item in GamePlayerInfo.instance.havePlayers)
        {
            if (item.breakthrough < 3)
            {
                playerList.Add(item);
            }
        }

        UpdateSortedPlayerList();
        

        int count = 0;
        foreach (var player in sortedPlayerList)
        {
            var bt = Instantiate(charCardPrefab, charCardTransform);
            var card = bt.GetComponent<PlayerLevelCard>();
            card.image.sprite = pt.GetPlayerSprite(player.code);
            card.level.text = $"Lv. {player.level.ToString("F0")}";
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
           

            int index = count++;
            bt.GetComponent<Button>().onClick.AddListener(() => OpenPlayerBreakInfo(index));
            MadeBList.Add(bt);
        }
    }

    public void OpenPlayerBreakInfo()
    {
        OpenPlayerBreakInfo(currIndex);
    }
    public void OpenPlayerBreakInfo(int index)
    {
        foreach (var item in MadeIngredientsList)
        {
            Destroy(item);
        }
        MadeIngredientsList.Clear();

        popUpbreakResultArea.SetActive(false);
        breakInfoArea.SetActive(true);
        currIndex = index;

        currPlayer = sortedPlayerList[index];

        slotType.text = st.Get($"type{currPlayer.type.ToString("F0")}");
        slotImage.sprite = pt.GetPlayerSprite(currPlayer.code);
        slotName.text = currPlayer.name;
        slotCover.SetActive(false);

        breakInfoTexts[0].text = "Lv." + currPlayer.maxLevel.ToString();
        breakInfoTexts[1].text = "Lv." + (currPlayer.maxLevel + 5).ToString();
        breakInfoTexts[2].text = currPlayer.breakthrough.ToString();
        breakInfoTexts[3].text = (currPlayer.breakthrough+1).ToString();
        //나중에 스킬테이블 완성되면..
        breakInfoTexts[4].text = currPlayer.name;
        breakInfoTexts[5].text = "Lv." + currPlayer.skillLevel.ToString();
        breakInfoTexts[6].text = "Lv." + currPlayer.breakthrough switch
        {
            1 => "2",
            2 => "3",
            _ => currPlayer.skillLevel.ToString()
        };
        curruseCount = 0;
        currNeedCount = currPlayer.breakthrough switch
        {
            0 => 1,
            1 => 2,
            2 => 4,
            _ => 1
        };
        breakInfoTexts[7].text = $"0 / {currNeedCount}";
        breakStartB.interactable = false;
        breakTargetImage.sprite = slotImage.sprite;
        breakTargetImage.color = disableColor;

        UpdateCanUsingPlayerList();
        int count = 0;
        foreach (var player in canUsingPlayerList)
        {
            var bt = Instantiate(useCardTogglePrefab, breakIngredientsTransform);
            var card = bt.GetComponent<PlayerLevelCard>();
            card.image.sprite = pt.GetPlayerSprite(player.code);
            card.level.text = $"Lv. {player.level.ToString("F0")}";
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

            int useIndex = count++;
            bt.GetComponent<Toggle>().onValueChanged.AddListener(value => SelectBreaking(useIndex,value));
            MadeIngredientsList.Add(bt);
        }
    }

    public void ExitPlayerBreakInfo()
    {
        breakInfoArea.SetActive(false);
        LoadPlayers();
    }

    public void SelectBreaking(int index,bool isOn)
    {
        if (isOn)
        {
            curruseCount++;
            if (curruseCount == currNeedCount)
            {
                breakStartB.interactable = true;
                breakTargetImage.color = Color.white;
                foreach (var item in MadeIngredientsList)
                {
                    var toggle = item.GetComponent<Toggle>();
                    if (!toggle.isOn)
                    {
                        toggle.interactable = false;
                    }
                }
            }
        }
        else
        {
            if (curruseCount == currNeedCount)
            {
                foreach (var item in MadeIngredientsList)
                {
                    item.GetComponent<Toggle>().interactable = true;
                }
            }
            curruseCount--;
            breakStartB.interactable = false;
        }
        breakInfoTexts[7].text = $"{curruseCount} / {currNeedCount}";
    }

    public void TryBreaking()
    {
        willDestroyPlayerList.Clear();
        int index = 0;
        foreach (var item in MadeIngredientsList)
        {
            if (item.GetComponent<Toggle>().isOn ==  true)
            {
                willDestroyPlayerList.Add(canUsingPlayerList[index]);
            }
            index++;
        }

        foreach (var item in willDestroyPlayerList)
        {
            if (item.breakthrough > 0 || item.level > 1)
            {
                popupWarningUseGrown.SetActive(true);
                return;
            }
        }
        StartBreaking();
    }

    public void StartBreaking()
    {
        GamePlayerInfo.instance.BreakPlayer(currPlayer, willDestroyPlayerList);
        GamePlayerInfo.instance.CheckRepresentPlayers();

        LoadPlayers();
        currIndex = sortedPlayerList.IndexOf(currPlayer);

        if (currPlayer.breakthrough < 3)
        {
            OpenPlayerBreakInfo();
        }

        popUpbreakResultImage.sprite = slotImage.sprite;
        popUpbreakResultTexts[0].text = breakInfoTexts[0].text;
        popUpbreakResultTexts[1].text = breakInfoTexts[1].text;
        popUpbreakResultTexts[2].text = breakInfoTexts[2].text;
        popUpbreakResultTexts[3].text = breakInfoTexts[3].text;
        popUpbreakResultTexts[4].text = breakInfoTexts[5].text;
        popUpbreakResultTexts[5].text = breakInfoTexts[6].text;

        popUpbreakResultArea.SetActive(true);
    }

    private void UpdateSortedPlayerList()
    {
        List<Player> playerList = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.GetUsingPlayers();
        foreach (var item in usingPlayers)
        {
            if (item.code != -1 && item.breakthrough < 3)
            {
                playerList.Add(item);
            }
        }
        foreach (var item in GamePlayerInfo.instance.havePlayers)
        {
            if (item.breakthrough < 3)
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
    }
    private void UpdateCanUsingPlayerList()
    {
        List<Player> playerList = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.GetUsingPlayers();
        foreach (var item in usingPlayers)
        {
            if (item.code == currPlayer.code && item.ID != currPlayer.ID)
            {
                playerList.Add(item);
            }
        }
        foreach (var item in GamePlayerInfo.instance.havePlayers)
        {
            if (item.code == currPlayer.code && item.ID != currPlayer.ID)
            {
                playerList.Add(item);
            }
        }

        canUsingPlayerList = playerList.OrderBy(p => p.level).ToList();
    }
}