using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDrawer : MonoBehaviour
{
    public GameObject training;

    public Image PlayerImage;
    public Image TypeIcon;
    public TMP_Text PlayerName;
    public Image Stars;
    public TMP_Text breakThrough;

    public TMP_Text[] XpVal = new TMP_Text[2];
    public Slider XpBar;
    public Button growB;
    public Transform PlayerCharPos;

    [HideInInspector]
    public GameObject PlayerChar;
    public Image SkillIcon;
    public TMP_Text SkillName;
    public TMP_Text SkillLevel;
    public TMP_Text SkillText;
    public List<TMP_Text> StatsInfo;

    private Player currPlayer;
    private PlayerTable pt;
    private StringTable st;

    private void OnEnable()
    {
        if (pt == null || st == null)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (GamePlayerInfo.instance.usingPlayers == null)
        {
            return;
        }
        if (LobbyUIManager.instance.isUsingPlayer)
        {
            currPlayer = GamePlayerInfo.instance.usingPlayers[LobbyUIManager.instance.PlayerInfoIndex];
        }
        else
        {
            currPlayer = GamePlayerInfo.instance.havePlayers[LobbyUIManager.instance.PlayerInfoIndex];
        }

        PlayerInfo playerInfo = pt.playerDatabase[currPlayer.code];
        foreach (var item in currPlayer.training)
        {
            var ti = pt.GetTrainingInfo(item);
            ti.AddStats(playerInfo);
        }

        PlayerImage.sprite = pt.GetPlayerFullSprite(currPlayer.code);
        PlayerName.text = st.Get($"playerName{currPlayer.code}");
        breakThrough.text = $"{st.Get("break")}: {currPlayer.breakthrough}";
        XpVal[0].text = $"<color=\"white\">Lv.{currPlayer.level}</color>/";
        XpVal[1].text = $"{currPlayer.maxLevel}";
        XpBar.value = currPlayer.xp / currPlayer.maxXp;
        PlayerChar = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{currPlayer.code}")), PlayerCharPos);
        PlayerChar.layer = 10;
        PlayerChar.GetComponent<RectTransform>().localScale = Vector3.one*170;
        PlayerChar.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,-57,0);
        StatsInfo[0].text = pt.CalculateCurrStats(playerInfo.hp, currPlayer.level).ToString("F0");
        StatsInfo[1].text = pt.CalculateCurrStats(playerInfo.atk, currPlayer.level).ToString("F0");
        StatsInfo[2].text = pt.CalculateCurrStats(playerInfo.atkRate, currPlayer.level).ToString("F1");
        StatsInfo[3].text = pt.CalculateCurrStats(playerInfo.critical, currPlayer.level).ToString("F1");
        StatsInfo[4].text = pt.CalculateCurrStats(playerInfo.accuracy, currPlayer.level).ToString("F1");
        StatsInfo[5].text = pt.CalculateCurrStats(playerInfo.range, currPlayer.level).ToString("F0");
        StatsInfo[6].text = playerInfo.magazine.ToString();
        StatsInfo[7].text = playerInfo.reloadingSpeed.ToString("F0");
        StatsInfo[8].text = pt.CalculateCurrStats(playerInfo.moveSpeed, currPlayer.level).ToString("F0");
        StatsInfo[9].text = pt.CalculateCurrStats(playerInfo.reactionSpeed, currPlayer.level).ToString("F0");
        Stars.sprite = playerInfo.grade switch
        {
            3 => pt.starsSprites[0],
            4 => pt.starsSprites[1],
            5 => pt.starsSprites[2],
            _ => pt.starsSprites[0],
        };
        TypeIcon.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", playerInfo.type.ToString()));

        SkillInfo skillInfo = pt.GetSkillInfo(playerInfo.UniqueSkillCode);
        SkillIcon.sprite = skillInfo.icon;
        SkillName.text = st.Get(skillInfo.name.ToString());
        SkillText.text = st.Get(skillInfo.toolTip.ToString());
        SkillLevel.text = $"Lv.{currPlayer.skillLevel}";

        if (currPlayer.level <currPlayer.maxLevel && !GamePlayerInfo.instance.isOnSchedule)
        {
            growB.interactable = true;
            growB.gameObject.SetActive(true);
        }
        else if(GamePlayerInfo.instance.isOnSchedule)
        {
            growB.gameObject.SetActive(false);
        }
        else 
        {
            growB.interactable = false;
            growB.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Destroy(PlayerChar);
        PlayerChar = null;
    }

    public void ToPlayerAnalyze()
    {
        if (PlayerListManager.instance != null)
        {
            PlayerListManager.instance.OnExit();
        }
        LobbyUIManager.instance.ActivePlayerListAnyway(false);
        training.SetActive(true);
        TrainingUIManager.instance.SetTraining(0);

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

        var sortedPlayerList = playerList.OrderByDescending(p => p.level)
    .ThenByDescending(p => p.breakthrough)
    .ThenByDescending(p => p.grade)
    .ThenByDescending(p => p.type)
    .ThenByDescending(p => p.name)
    .ToList(); ;
        var mta = TrainingUIManager.instance.trainingManagers[0] as ManagerTrainingAnalyze;
        mta.OpenPlayerGrowInfo(sortedPlayerList.IndexOf(currPlayer));
        gameObject.SetActive(false);
        TrainingUIManager.instance.lobbyTopMenu.gameObject.SetActive(true);
        TrainingUIManager.instance.lobbyTopMenu.DeleteAllFunction();
        TrainingUIManager.instance.lobbyTopMenu.AddFunction(TrainingUIManager.instance.OnBack);
        TrainingUIManager.instance.lobbyTopMenu.AddFunction(TrainingUIManager.instance.FunctionBack);
        LobbySceneUIManager.instance.currUIIndex = (int)LobbyType.Upgrade;
    }
}
