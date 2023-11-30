using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsPlaying {  get; set; }
    public bool IsPaused { get; set; }
    public bool IsPlayerWin { get; set; }
    public AIManager aiManager;
    public GameEndPannel gameEndPannel;

    public float timer;
    public float endTime;

    private void Awake()
    {
        PlayingGame();
    }

    private void Update()
    {
        if(!IsPlaying)
        {
            if (timer + endTime < Time.time)
                EndGame();
        }
    }

    public void PlayingGame()
    {
        IsPlaying = true;
        gameEndPannel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        IsPlaying = false;
        if(IsPlayerWin)
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
