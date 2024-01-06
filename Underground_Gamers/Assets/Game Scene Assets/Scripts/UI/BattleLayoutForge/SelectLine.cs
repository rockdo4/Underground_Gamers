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
        // 저장된 현재 배속 적용받기
        Time.timeScale = 1f;
        gameManager.IsStart = true;
        SoundPlayer.instance.PauseMusic();
        SoundPlayer.instance.EnterBattleMusicPlay(1);

        // 기존 코드
        battleLayoutForgePanel.SetActive(false);
        gameManager.entryManager.EnterGameByEntry();
        gameManager.entryManager.ClearEntry();
        gameManager.commandManager.CreateCommandUI();
        gameManager.aiManager.RegisterMissionTargetEvent();
        gameManager.aiManager.SetAICanvas();
        gameManager.aiManager.ResetAIStatus();
        gameManager.aiManager.ResetAIState();
        gameManager.battleLayoutForge.ClearSlot();
        gameManager.buildingManager.DisplayBuildingHPByReset();
        Time.timeScale = GamePlayerInfo.instance.playSpeed;
        
        switch(GamePlayerInfo.instance.playSpeed)
        {
            case 1f:
                gameManager.gameSpeedFactor.OnSpeedFactorX1();
                break;
            case 2f:
                gameManager.gameSpeedFactor.OnSpeedFactorX2();
                break;
            case 4f:
                gameManager.gameSpeedFactor.OnSpeedFactorX4();
                break;
        }

        gameManager.skillModeButton.SwitchActiveObject(GamePlayerInfo.instance.isAutoMode);
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
