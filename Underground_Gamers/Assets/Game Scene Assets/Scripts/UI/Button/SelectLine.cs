using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLine : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject battleLayoutForgePanel;
    public Button button;
    public Image buttonImage;
    public TextMeshProUGUI text;

    private void Awake()
    {
        SetActive(false);
    }

    public void FixLine()
    {
        Time.timeScale = 1f;
        gameManager.IsStart = true;
        battleLayoutForgePanel.SetActive(false);
        gameManager.entryManager.EnterGameByEntry();
        gameManager.commandManager.CreateCommandUI();
        gameManager.aiManager.RegisterMissionTargetEvent();
        gameManager.aiManager.SetAICanvas();
        gameManager.aiManager.ResetAIState();
    }

    public void SetActive(bool isActive)
    {
        SetActiveButton(isActive);
        SetActiveColor(isActive);
    }
    public void SetActiveButton(bool isActive)
    {
        button.interactable = isActive;
    }

    public void SetActiveColor(bool isActive)
    {

        Color originButtonColor = buttonImage.color;
        Color originTextColor = text.color;
        if (isActive)
        {
            originButtonColor.a = 1f;
            originTextColor.a = 1f;
        }
        else
        {
            originButtonColor.a = 0.65f;
            originTextColor.a = 0.65f;
        }

        buttonImage.color = originButtonColor;
        text.color = originTextColor;
    }
}
