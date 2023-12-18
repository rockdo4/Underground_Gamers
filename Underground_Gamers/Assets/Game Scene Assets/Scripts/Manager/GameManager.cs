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
    public bool IsGameEnd = false;

    [Header("메니저 캐싱")]
    public AIManager aiManager;
    public CameraManager cameraManager;
    public CommandManager commandManager;
    public BuildingManager buildingManager;
    public NPCManager npcManager;
    public LineManager lineManager;
    public GameRuleManager gameRuleManager;
    public SkillCoolTimeManager skillCoolTimeManager;

    public GameEndPannel gameEndPannel;

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

        if (!IsPlaying && !IsTimeOut)
        {
            if (endTimer + endTime < Time.time && !IsGameEnd)
                EndGame();
        }
    }

    public void PlayingGame()
    {
        IsPlaying = true;
        IsTimeOut = false;
        IsGameEnd = false;
        gameEndPannel.OffGameEndPanel();
        Time.timeScale = 1f;
        gameTimer = gameTime;
    }

    public void EndGame()
    {
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

        // 게임 업데이트가 멈추는데 멈춰도 되는가 생각
        Time.timeScale = 0f;
        gameEndPannel.OnGameEndPanel();
    }
}
