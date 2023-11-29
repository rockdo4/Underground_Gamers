using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CommandType
{
    SwitchLine,
    Defend,
    Count
}

public class CommandManager : MonoBehaviour
{
    [Header("Ä³½Ì")]
    public AIManager aiManager;

    [Header("Ä¿¸àµå UI")]
    public CommandInfo commandInfoPrefab;
    public Transform commandInfoParent;
    public Button commandButtonPrefab;



    private Queue<(Command, AIController)> records = new Queue<(Command, AIController)>();
    private List<Command> commands = new List<Command>();

    private void Awake()
    {
        CreateCommands();
        CreateCommandUI();
    }

    private void CreateCommands()
    {
        commands.Add(new SwitchLineCommand());
        commands.Add(new DefendCommand());
    }

    private void CreateCommandUI()
    {
        int pcNum = 1;

        foreach (var ai in aiManager.pc)
        {
            CommandInfo info = Instantiate(commandInfoPrefab, commandInfoParent);
            ai.aiCommandInfo = info;
            info.aiController = ai;
            info.name = $"{info.name}{pcNum}";
            info.aiType = "PC";
            info.aiNum = pcNum++;
            var text = info.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            text.text = $"{info.aiType}{info.aiNum}";

            // Ä¿¸àµå ³Ö±â
            Transform commandParent = info.transform.GetChild(0);

            for (int i = 0; i < commands.Count; ++i)
            {
                int index = i;
                Button commandButton = Instantiate(commandButtonPrefab, commandParent);
                var commandID = commandButton.GetComponentInChildren<TextMeshProUGUI>();
                commandID.text = $"{(CommandType)i}";

                // ±â´É ÀÔ·Â
                commandButton.onClick.AddListener(() => commands[index].ExecuteCommand(info.aiController));
            }
        }
    }
}
