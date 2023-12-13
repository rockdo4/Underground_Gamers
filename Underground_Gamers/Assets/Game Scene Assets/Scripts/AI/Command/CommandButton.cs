using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    public CommandManager commandManager;
    public Button commandButton;

    public CommandType type;

    private void Awake()
    {
        commandButton.onClick.AddListener(() => commandManager.ExecuteCommand(type, commandManager.currentAI));
    }
}
