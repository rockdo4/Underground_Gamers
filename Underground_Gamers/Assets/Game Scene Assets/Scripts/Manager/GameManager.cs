using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsPlaying { get; set; }
    public bool IsTimeOut { get; set; }
    public bool IsPaused { get; set; }
    public bool IsRoundWin { get; set; }
    public bool IsGameWin { get; set; } = false;
    public bool IsJudgement { get; set; } = false;
    public bool IsStart { get; set; } = false;
    public bool IsGameEnd = false;
    public bool IsRoundEnd = false;

    [Header("메니저 캐싱")]
    public AIManager aiManager;
    public CameraManager cameraManager;
    public CommandManager commandManager;
    public BuildingManager buildingManager;
    public NPCManager npcManager;
    public LineManager lineManager;
    public GameRuleManager gameRuleManager;
    public SkillCoolTimeManager skillCoolTimeManager;
    public EntryManager entryManager;
    public AIRewardManager aiRewardManager;
    public StageInfoManager stageInfoManager;
    public CutsceneManager cutSceneManager;

    [Header("캐싱")]
    public GameEndPannel gameEndPannel;
    public SkillModeButton skillModeButton;
    public SettingAIID settingAIID;
    public BattleLayoutForge battleLayoutForge;
    public Respawner respawner;
    public Transform uiCanvas;
    public EntryPanel entryPanel;
    public WayPoint wayPoint;
    public AutoSelect autoSelect;
    public KillLogPanel killLogPanel;

    [Header("테이블 캐싱")]
    public PlayerTable pt;
    public StringTable str;
    public StageTable stageTable;


    public float endTimer;
    public float endTime;

    public float gameTimer;
    public float gameTime;
    public TextMeshProUGUI gameTimeText;
    public CharacterStatus pcNexus;
    public CharacterStatus npcNexus;

    private void Awake()
    {
        PlayingGame();
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        str = DataTableManager.instance.Get<StringTable>(DataType.String);
        stageTable = DataTableManager.instance.Get<StageTable>(DataType.Stage);
    }

    private void DisplayGameTimer(float time)
    {
        int min = Mathf.RoundToInt(time) / 60;
        int second = Mathf.RoundToInt(time) % 60;
        gameTimeText.text = $"{min:D2} : {second:D2}";
    }

    private void Update()
    {
        if (IsPlaying && !IsTimeOut)
        {
            gameTimer -= Time.deltaTime;
            DisplayGameTimer(gameTimer);
        }

        if (gameTimer < 0f && !IsTimeOut)
        {
            IsTimeOut = true;
            endTimer = Time.time;
        }

        // 타임 아웃 기준
        if (IsTimeOut)
        {
            if (!IsJudgement)
            {
                IsJudgement = true;
                if (gameRuleManager.IsPlayerWinByTimeOut())
                {
                    IsRoundWin = true;
                }
                else
                {
                    IsRoundWin = false;
                }

                Time.timeScale = 1f;
                gameEndPannel.DisplayResultLogo(IsRoundWin);
                GetWinner();
            }

            if (endTimer + endTime < Time.time && !IsGameEnd)
                EndGame();
        }

        // 넥서스 파괴 기준
        if (!IsPlaying && !IsTimeOut)
        {
            if (!IsJudgement)
            {
                IsJudgement = true;
            }

            if (endTimer + endTime < Time.time && !IsGameEnd)
                EndGame();
        }
    }

    // 징표 얻기, 승수 검사
    public void GetWinner()
    {
        //gameRuleManager.GetWinEvidence(IsRoundWin);
        int winEvidence = gameRuleManager.GetWinEvidence(IsRoundWin);
        if (IsRoundWin && winEvidence >= gameRuleManager.WinningCount)
        {
            IsGameWin = true;
            IsRoundEnd = true;
        }

        if (!IsRoundWin && winEvidence >= gameRuleManager.WinningCount)
        {
            IsGameWin = false;
            IsRoundEnd = true;
        }
    }

    // 라운드 재시작시 실행
    public void PlayingGame()
    {
        IsStart = false;
        IsPlaying = true;
        IsTimeOut = false;
        IsRoundEnd = false;
        IsGameEnd = false;
        IsGameWin = false;
        IsJudgement = false;
        gameEndPannel.OffGameEndPanel();
        Time.timeScale = 1f;
        gameTimer = gameTime;
        SoundPlayer.instance.EnterGameMusic();
    }

    public void EndGame()
    {
        IsGameEnd = true;
        IsStart = false;
        IsPlaying = false;
        if (IsRoundWin)
        {
            gameEndPannel.victoryLogo.gameObject.SetActive(true);
            gameEndPannel.defeatLogo.gameObject.SetActive(false);
        }
        else
        {
            gameEndPannel.victoryLogo.gameObject.SetActive(false);
            gameEndPannel.defeatLogo.gameObject.SetActive(true);
        }

        gameEndPannel.youWinLogo.gameObject.SetActive(false);
        gameEndPannel.youLoseLogo.gameObject.SetActive(false);
        gameEndPannel.youWinLogo.transform.position = gameEndPannel.originPos;
        gameEndPannel.youLoseLogo.transform.position = gameEndPannel.originPos;
        gameEndPannel.endPanel.SetActive(false);
        // 게임 업데이트가 멈추는데 멈춰도 되는가 생각
        Time.timeScale = 0f;
        gameEndPannel.OnGameEndPanel();
    }
}
