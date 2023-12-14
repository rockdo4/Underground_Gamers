using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCommand : Command
{
    public override void ExecuteCommand(AIController ai, WayPoint wayPoint)
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

            foreach (var aiController in gameManager.aiManager.pc)
            {
                aiController.isDefend = true;
                aiController.isAttack = false;
                if (aiController.status.IsLive)
                    aiController.SetState(States.Retreat);

                Debug.Log($"{aiController.aiType.text} : Defend Command Execute");
            }
        }         // 개별명령
        else
        {
            ai.isDefend = true;
            ai.isAttack = false;
            if (ai.status.IsLive)
                ai.SetState(States.Retreat);
            if (ai.teamIdentity.teamType == TeamType.PC)
                gameManager.commandManager.SetActiveCommandButton(ai);
            Debug.Log($"{ai.aiType.text} : Defend Command Execute");
        }
    }
}
