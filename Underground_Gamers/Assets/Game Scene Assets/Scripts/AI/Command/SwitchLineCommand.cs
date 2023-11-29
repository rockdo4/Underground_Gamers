using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLineCommand : Command
{
    public override void ExecuteCommand(AIController ai)
    {
        Debug.Log($"{ai.aiType.text} : SwitchLine Command Execute");
    }
}