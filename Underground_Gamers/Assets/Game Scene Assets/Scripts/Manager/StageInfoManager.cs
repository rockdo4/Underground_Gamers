using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("퍼즈 패널UI")]
    public TextMeshProUGUI gameTypeText;
    public TextMeshProUGUI stageNameText;
    public TextMeshProUGUI pauseEnemyTeamNameText;

    [Header("인게임 UI")]
    public TextMeshProUGUI ourTeamNameText;
    public TextMeshProUGUI enemyTeamNameText;

    public string[] days = new string[7];
    public string[] leagueLevel = new string[4];

    public void InitString()
    {
        days[0] = "mon";
        days[1] = "tue";
        days[2] = "wed";
        days[3] = "thu";
        days[4] = "fri";
        days[5] = "sat";
        days[6] = "sun";
        leagueLevel[0] = "3rd_league";
        leagueLevel[1] = "2nd_league";
        leagueLevel[2] = "1st_league";
        leagueLevel[3] = "champions_league";
    }

    public void SetStageInfo()
    {
        //SetOutTeamNameText();
        switch (GameInfo.instance.gameType)
        {
            case GameType.Story:
                string storyOurTeamName = GamePlayerInfo.instance.teamName;
                string storyEnemyName = GameInfo.instance.storyTeamName;

                SetGameTypeText(gameManager.str.Get("story"));
                SetStageNameText(gameManager.stageTable.GetStageInfo(GameInfo.instance.currentStage).name);
                SetOurTeamNameText(storyOurTeamName);
                SetEnemyTeamNameText(storyEnemyName);
                SetPauseEnemyTeamNameText($"VS {storyEnemyName}");

                break;
            case GameType.Official:
                // 정규전 적 팀 이름
                string officialOurTeamName = GamePlayerInfo.instance.teamName;
                string officialEnemyName;
                if (GamePlayerInfo.instance.officialWeekNum < 7)
                {
                    officialEnemyName = GamePlayerInfo.instance.officialTeamDatas[GamePlayerInfo.instance.officialPlayerMatchInfo[GamePlayerInfo.instance.officialWeekNum]].name;
                }
                else
                {
                    if (GamePlayerInfo.instance.officialTeamDatas[10 - GamePlayerInfo.instance.officialWeekNum].isPlayer)
                    {
                        officialEnemyName = GamePlayerInfo.instance.officialTeamDatas[9 - GamePlayerInfo.instance.officialWeekNum].name;
                    }
                    else
                    {
                        officialEnemyName = GamePlayerInfo.instance.officialTeamDatas[10 - GamePlayerInfo.instance.officialWeekNum].name;
                    }
                }
                SetGameTypeText(gameManager.str.Get("regular match"));
                SetStageNameText($"{(GamePlayerInfo.instance.officialWeekNum + 1).ToString()}{gameManager.str.Get("regular weeks match")}");
                SetOurTeamNameText(officialOurTeamName);
                SetEnemyTeamNameText(officialEnemyName);
                SetPauseEnemyTeamNameText($"VS {officialEnemyName}");

                break;
            case GameType.Scrimmage:
                string scrimmageOurTeamName = GamePlayerInfo.instance.teamName;
                string scrimmageEnemyTeamName = gameManager.str.Get($"random_team_name{UnityEngine.Random.Range(0, 99)}");
                SetGameTypeText(gameManager.str.Get("scrimmage"));
                SetStageNameText($"{gameManager.str.Get(days[((int)DateTime.Now.DayOfWeek + 6) % 7])} {gameManager.str.Get(leagueLevel[GameInfo.instance.screammageLevel])}");
                SetOurTeamNameText(scrimmageOurTeamName);
                SetEnemyTeamNameText(scrimmageEnemyTeamName);
                SetPauseEnemyTeamNameText($"VS {scrimmageEnemyTeamName}");
                break;
        }
    }

    public void SetGameTypeText(string gameType)
    {
        gameTypeText.text = gameType;
    }

    public void SetStageNameText(string stageName)
    {
        stageNameText.text = stageName;
    }
    public void SetOurTeamNameText(string ourTeamName)
    {
        ourTeamNameText.text = ourTeamName;
    }
    public void SetEnemyTeamNameText(string enemyName)
    {
        enemyTeamNameText.text = enemyName;
    }
    public void SetPauseEnemyTeamNameText(string enemyName)
    {
        pauseEnemyTeamNameText.text = enemyName;
    }


}
