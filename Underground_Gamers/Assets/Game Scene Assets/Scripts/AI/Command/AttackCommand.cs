using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{
    public override void ExecuteCommand(AIController ai, WayPoint wayPoint)
    {
        AIManager aiManager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();
        AIController selectAI = ai;

        // 단체 명령
        if (selectAI == null)
        {
            //List<AIController> aIControllers = ai.teamIdentity.teamType switch
            //{
            //    TeamType.PC => ai.gameManager.aiManager.pc,
            //    TeamType.NPC => ai.gameManager.aiManager.npc,
            //    _ => ai.gameManager.aiManager.pc
            //};

            foreach (var aIController in aiManager.pc)
            {
                Debug.Log($"{aIController.aiType.text} : Attack Command Execute");
            }
        }       // 개별명령
        else
        {
            Debug.Log($"{ai.aiType.text} : Attack Command Execute");
        }
    }
}
