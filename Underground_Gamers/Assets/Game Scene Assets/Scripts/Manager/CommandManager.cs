using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [Header("캐싱")]
    public AIManager aiManager;
    public WayPoint wayPoint;

    [Header("커멘드 UI")]
    public CommandInfo commandInfoPrefab;
    public Transform commandInfoTopParent;
    public Transform commandInfoBottomParent;
    public Button commandButtonPrefab;



    private Queue<(Command, AIController)> records = new Queue<(Command, AIController)>();
    private List<Command> commands = new List<Command>();

    private List<CommandInfo> commandInfos = new List<CommandInfo>();
    private CommandInfo currentCommandInfo = null;
    public bool isCheckInfo = false;


    private void Awake()
    {
        CreateCommands();
        CreateCommandUI();
    }

    private void CreateCommands()
    {
        commands.Add(new SwitchLineCommand());
        //commands.Add(new DefendCommand());
    }

    private void CreateCommandUI()
    {
        int pcNum = 1;

        foreach (var ai in aiManager.pc)
        {
            CommandInfo info = Instantiate(commandInfoPrefab, commandInfoTopParent);
            ai.aiCommandInfo = info;
            info.aiController = ai;
            info.name = $"{info.name}{pcNum}";
            info.aiType = "PC";
            info.aiNum = pcNum++;
            var text = info.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            text.text = $"{info.aiType}{info.aiNum}";
            commandInfos.Add(info);

            // 기능 입력
            var infoButotn = info.GetComponent<Button>();
            infoButotn.onClick.AddListener(() => OnCommadns(info));
            // 커멘드 넣기
            Transform commandParent = info.transform.GetChild(0);

            for (int i = 0; i < commands.Count; ++i)
            {
                int index = i;
                Button commandButton = Instantiate(commandButtonPrefab, commandParent);
                var commandID = commandButton.GetComponentInChildren<TextMeshProUGUI>();
                commandID.text = $"{(CommandType)i}";

                // 기능 입력
                commandButton.onClick.AddListener(() => commands[index].ExecuteCommand(info.aiController, wayPoint));
                commandButton.gameObject.SetActive(false);
                info.commandButtons.Add(commandButton);
            }
        }
    }

    public void ExecuteSwitchLine(int aiIndex)
    {
        commands[(int)CommandType.SwitchLine].ExecuteCommand(aiManager.pc[aiIndex], wayPoint);
    }

    public void OnCommadns(CommandInfo commandInfo)
    {
        DragAndDrop dragAndDrop = commandInfo.GetComponent<DragAndDrop>();

        OffAllCommands();
        if (dragAndDrop != null)
        {
            if (currentCommandInfo == commandInfo && !dragAndDrop.isDragging)
                isCheckInfo = true;
            else if(currentCommandInfo != commandInfo && !dragAndDrop.isDragging)
                isCheckInfo = false;
        }
        else
        {
            if (currentCommandInfo == commandInfo)
                isCheckInfo = true;
            else
                isCheckInfo = false;
        }


        currentCommandInfo = commandInfo;
        foreach (var button in currentCommandInfo.commandButtons)
        {
            button.gameObject.SetActive(true);
        }

        if (isCheckInfo)
        {
            OffCurrentCommads();
        }

    }

    public void OffAllCommands()
    {
        foreach(var info in commandInfos)
        {
            foreach(var button in info.commandButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void OffCurrentCommads()
    {
        foreach(var info in currentCommandInfo.commandButtons)
        {
            info.gameObject.SetActive(false);
        }
        currentCommandInfo = null;
    }
}
