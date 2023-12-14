using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pausePanel;
    public GameSpeedFactor speedFactor;

    public void OnResume()
    {
        gameManager.IsPaused = false;
        Time.timeScale = speedFactor.currentTimeScale;
        pausePanel.SetActive(false);
    }
}
