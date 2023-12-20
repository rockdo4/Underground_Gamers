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
    public bool IsPlayerWin { get; set; }
    public bool IsStart { get; set; } = false;
    public bool IsGameEnd = false;

    [Header("¸Þ´ÏÀú Ä³½Ì")]
    public AIManager aiManager;
    public CameraManager cameraManager;
    public CommandManager commandManager;
    public BuildingManager buildingManager;
    public NPCManager npcManager;
    public LineManager lineManager;
    public GameRuleManager gameRuleManager;
    public SkillCoolTimeManager skillCoolTimeManager;
    public EntryManager entryManager;

    [Header("Ä³½Ì")]
    public GameEndPannel gameEndPannel;
    public SkillModeButton skillModeButton;
    public SettingAIID settingAIID;
    public BattleLayoutForge battleLayoutForge;

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

        // Å¸ÀÓ ¾Æ¿ô ±âÁØ
        if (IsTimeOut)
        {
            if (gameRuleManager.IsPlayerWinByTimeOut())
            {
                IsPlayerWin = true;
            }
            else
            {
                IsPlayerWin = false;
            }

            if (endTimer + endTime < Time.time && !IsGameEnd)
                EndGame();
        }

        // ³Ø¼­½º ÆÄ±« ±âÁØ
        if (!IsPlaying && !IsTimeOut)
        {
            if (endTimer + endTime < Time.time && !IsGameEnd)
                EndGame();
        }
    }

    public void PlayingGame()
    {
        IsStart = false;
        IsPlaying = true;
        IsTimeOut = false;
        IsGameEnd = false;
        gameEndPannel.OffGameEndPanel();
        Time.timeScale = 1f;
        gameTimer = gameTime;
    }

    public void EndGame()
    {
        IsStart = false;
        IsGameEnd = true;
        IsPlaying = false;
        if (IsPlayerWin)
        {
            gameEndPannel.winText.gameObject.SetActive(true);
            gameEndPannel.LoseText.gameObject.SetActive(false);
            GameInfo.instance.WinReward();
        }
        else
        {
            gameEndPannel.winText.gameObject.SetActive(false);
            gameEndPannel.LoseText.gameObject.SetActive(true);
        }

        // Â¡Ç¥ ¾ò±â, ½Â¼ö °Ë»ç
        int winEvidenceCount =  gameRuleManager.GetWinEvidence(IsPlayerWin);
        if(gameRuleManager.IsGameWin(IsPlayerWin, winEvidenceCount))
        {
            Debug.Log("°ÔÀÓ Á¾·á!");
        }

        // °ÔÀÓ ¾÷µ¥ÀÌÆ®°¡ ¸ØÃß´Âµ¥ ¸ØÃçµµ µÇ´Â°¡ »ý°¢
        Time.timeScale = 0f;
        gameEndPannel.OnGameEndPanel();
    }
}
