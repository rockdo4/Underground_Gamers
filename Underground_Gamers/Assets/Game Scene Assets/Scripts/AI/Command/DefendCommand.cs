using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCommand : Command
{
    public override void ExecuteCommand(AIController ai)
    {
        Debug.Log($"{ai.aiType.text} : Defend Command Execute");
    }
}
