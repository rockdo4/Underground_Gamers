using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pausePanel;

    public void OnResume()
    {
        gameManager.IsPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
