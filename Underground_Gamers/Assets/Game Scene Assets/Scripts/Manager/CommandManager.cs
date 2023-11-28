using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public AIManager aiManager;
    public CommandInfo commandInfoPrefab;
    public Transform commandInfoParent;

    private void Awake()
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
        }
    }
}
