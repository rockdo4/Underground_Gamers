using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pausePanel;

    public void OnPause()
    {
        gameManager.IsPaused = true;
        gameManager.IsPlaying = false;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
}
