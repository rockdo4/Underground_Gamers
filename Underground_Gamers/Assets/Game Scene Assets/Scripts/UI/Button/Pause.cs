using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pausePanel;
    public TextMeshProUGUI resumeText;
    public TextMeshProUGUI optionText;
    public TextMeshProUGUI giveUpText;

    private void Start()
    {
        resumeText.text = gameManager.str.Get("resume");
        optionText.text = gameManager.str.Get("setting option");
        giveUpText.text = gameManager.str.Get("give up");
    }

    public void OnPause()
    {
        gameManager.IsPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
}
