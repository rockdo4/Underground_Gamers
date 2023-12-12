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
    public AIManager aiManager;
    public GameEndPannel gameEndPannel;

    public float timer;
    public float endTime;

    public float gameTimer;
    public float gameTime;
    public TextMeshProUGUI gameTimeText;
    public CharacterStatus pcNexus;
    public CharacterStatus npcNexus;

    public CameraManager cameraManager;

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
        if (IsPlaying)
        {
            gameTimer -= Time.deltaTime;
            DisplayGameTimer(gameTimer);
        }

        if (gameTimer < 0f)
        {
            IsTimeOut = true;
        }

        if (IsTimeOut)
        {
            // 같은 경우는?
            if (pcNexus.Hp >= npcNexus.Hp)
            {
                IsPlayerWin = true;
            }
            else
            {
                IsPlayerWin = false;
            }

            EndGame();
        }

        if (!IsPlaying && !IsTimeOut)
        {
            if (timer + endTime < Time.time)
                EndGame();
        }
    }

    public void PlayingGame()
    {
        IsPlaying = true;
        IsTimeOut = false;
        gameEndPannel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        gameTimer = gameTime;

    }

    public void EndGame()
    {
        IsPlaying = false;
        if (IsPlayerWin)
        {
            gameEndPannel.winText.gameObject.SetActive(true);
            gameEndPannel.LoseText.gameObject.SetActive(false);
        }
        else
        {
            gameEndPannel.winText.gameObject.SetActive(false);
            gameEndPannel.LoseText.gameObject.SetActive(true);
        }

        // 게임 업데이트가 멈추는데 멈춰도 되는가 생각
        Time.timeScale = 0f;
        gameEndPannel.gameObject.SetActive(true);
    }
}
