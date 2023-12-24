using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    public CommandManager commandManager;
    public Button commandButton;

    public CommandType type;

    public Color activeColor;
    // 적용 중인 상태
    public Color deactiveColor;

    public Image fillImage;

    public bool isClick;
    public float timer;
    public float time;

    private void Awake()
    {
        commandButton.onClick.AddListener(() => commandManager.ExecuteCommand(type, commandManager.currentAI));
        commandButton.onClick.AddListener(StartCoolButton);
    }

    public void StartCoolButton()
    {
        commandManager.attackButton.commandButton.interactable = false;
        commandManager.defendButton.commandButton.interactable = false;
        commandManager.attackButton.isClick = true;
        commandManager.defendButton.isClick = true;
        commandManager.attackButton.timer = Time.time;
        commandManager.defendButton.timer = Time.time;
    }

    public void EndCoolButton()
    {
        commandManager.attackButton.commandButton.interactable = true;
        commandManager.defendButton.commandButton.interactable = true;
        commandManager.attackButton.isClick = false;
        commandManager.defendButton.isClick = false;
        commandManager.attackButton.timer = Time.time;
        commandManager.defendButton.timer = Time.time;
        DisplayCoolTime(1f);
    }

    private void Update()
    {
        if (isClick)
        {
            DisplayCoolTime((Time.time - timer) / time);
            if (timer + time < Time.time)
            {
                EndCoolButton();
            }
        }
    }

    public void DisplayCoolTime(float value)
    {
        commandManager.attackButton.fillImage.fillAmount = value;
        commandManager.defendButton.fillImage.fillAmount = value;
    }

    public void SetActiveButton(bool active)
    {
        if (active)
        {
            fillImage.color = activeColor;
        }
        else     // 적용 중인 상태
        {
            fillImage.color = deactiveColor;
        }
    }
}
