using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTrainingTrain : ManagerTraining
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
    private GameObject trainInfoArea;
    [SerializeField]
    private List<TMP_Text> potentialTexts = new List<TMP_Text>();
    [SerializeField]
    private List<Toggle> levelToggles = new List<Toggle>();
    [SerializeField]
    private GameObject trainTogglePrefab;
    private List<GameObject> trainToggles = new List<GameObject>();
    [SerializeField]
    private Transform trainTogglesTransform;
    [SerializeField]
    private GameObject trainResultArea;
    [SerializeField]
    private GameObject trainResultPrefab;
    [SerializeField]
    private Transform trainResultTransform;

    [SerializeField]
    private Button trainStartB;

    [SerializeField]
    private GameObject poupHave;
    [SerializeField]
    private Transform haveTransform;
    private List<GameObject> haveLists = new List<GameObject>();

    private PlayerTable pt;
    private StringTable st;
    private List<GameObject> MadeBList = new List<GameObject>();
    private List<GameObject> MadeResultList = new List<GameObject>();
    private List<Player> sortedPlayerList = new List<Player>();

    private int currIndex = 0;
    private Player currPlayer;
    private int currPotential;

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
        trainInfoArea.SetActive(false);

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
            card.level.text = $"Lv. {player.level.ToString("F0")}";

            int index = count++;
            bt.GetComponent<Button>().onClick.AddListener(() => OpenPlayerGrowInfo(index));
            MadeBList.Add(bt);
        }

    }

    public void OpenPlayerGrowInfo()
    {
        OpenPlayerGrowInfo(currIndex);
    }
    public void OpenPlayerGrowInfo(int index)
    {
        trainResultArea.SetActive(false);
        trainInfoArea.SetActive(true);
        currIndex = index;

        currPlayer = sortedPlayerList[index];
        

        if (trainToggles.Count <= 0)
        {
            List<TrainingInfo> ti = pt.trainingDatabase;
            foreach (var info in ti)
            {
                GameObject toggle = Instantiate(trainTogglePrefab, trainTogglesTransform);
                var tt = toggle.GetComponent<TrainToggle>();
                var tg = toggle.GetComponent<Toggle>();
                tt.id = info.id;
                tt.MakeButton();
                tg.onValueChanged.AddListener(value => SelectTraining(value,tt));
                trainToggles.Add(toggle);

                var have = Instantiate(trainResultPrefab, haveTransform);
                have.GetComponentInChildren<TMP_Text>().text = tt.statText.text;
                haveLists.Add(have);
            }
        }
  
        OpenToggle(1);

        slotType.text = st.Get($"type{currPlayer.type.ToString("F0")}");
        slotImage.sprite = pt.GetPlayerSprite(currPlayer.code);
        slotName.text = currPlayer.name;
        slotCover.SetActive(false);

        levelToggles[0].isOn = true;

    }

    public void ExitPlayerTrainInfo()
    {
        trainInfoArea.SetActive(false);
        LoadPlayers();
    }

    public void OpenToggle(int level)
    {
        ResetToggles();
        UpdateToggles(level);
    }
    public void UpdateToggles(int level)
    {
        potentialTexts[0].text = currPlayer.potential.ToString();
        potentialTexts[1].text = currPotential.ToString();

        foreach (var toggle in trainToggles)
        {
            var tt = toggle.GetComponent<TrainToggle>();
            var tg = toggle.GetComponent<Toggle>();
            if (currPlayer.training.Contains(tt.id))
            {
                tg.interactable = false;
            }
            else
            {
                if (currPotential - tt.cost < 0 && !tg.isOn)
                {
                    tg.interactable = false;
                }
                else
                {
                    tg.interactable = true;
                }
            }

            if (tt.id >= level*100 &&
                tt.id < (level+1) * 100)
            {
                toggle.SetActive(true);
            }
            else
            {
                toggle.SetActive(false);
            }
        }
    }

    public void SelectTraining(bool isOn,TrainToggle tt)
    {
        trainStartB.interactable = false;
        foreach (var item in trainToggles)
        {
            if (item.GetComponent<Toggle>().isOn)
            {
                trainStartB.interactable = true;
                break;
            }
        }
        if (isOn)
        {
            currPotential -= tt.cost;
        }
        else
        {
            currPotential += tt.cost;
        }

        int level = 1;
        foreach (var item in levelToggles)
        {
            if (item.isOn)
            {
                break;
            }
            level++;
        }
        UpdateToggles(level);
    }

    public void ResetToggles()
    {
        foreach (var toggle in trainToggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }
        currPotential = currPlayer.potential;

        potentialTexts[0].text = currPlayer.potential.ToString();
        potentialTexts[1].text = potentialTexts[0].text;

        trainStartB.interactable = false;
    }

    public void StartTraining() 
    {
        foreach (var item in MadeResultList)
        {
            Destroy(item);
        }
        MadeResultList.Clear();

        List<int> ids = new List<int>();
        foreach (var toggle in trainToggles)
        {
            if (toggle.GetComponent<Toggle>().isOn)
            {
                var tt = toggle.GetComponent<TrainToggle>();
                var results = Instantiate(trainResultPrefab, trainResultTransform);
                results.GetComponentInChildren<TMP_Text>().text = tt.statText.text;
                MadeResultList.Add(results);
                ids.Add(tt.id);
            }
        }

        GamePlayerInfo.instance.TrainPlayer(currPlayer,ids,currPotential);

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

        trainResultArea.SetActive(true);
    }

    public void ActiveHave()
    {
        foreach (var item in haveLists)
        {
            item.SetActive(false);
        }
        foreach (var train in currPlayer.training)
        {
            haveLists[pt.trainingDatabase.FindIndex(a => a.id == train)].SetActive(true);
        }
        poupHave.SetActive(true);
    }
}
