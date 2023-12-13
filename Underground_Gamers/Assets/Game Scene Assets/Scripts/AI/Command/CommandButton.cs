using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        commandButton.onClick.AddListener(() => commandManager.ExecuteCommand(type, commandManager.currentAI));
    }

    public void SetActiveButton(bool active)
    {
        if(active)
        {
            commandButton.GetComponent<Image>().color = activeColor;
        }
        else     // 적용 중인 상태
        {
            commandButton.GetComponent<Image>().color = deactiveColor;
        }
    }
}
