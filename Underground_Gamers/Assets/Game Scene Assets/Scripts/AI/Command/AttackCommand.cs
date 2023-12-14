using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
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
                aiController.isAttack = true;
                aiController.isDefend = false;

                Transform attackTarget = aiController.buildingManager.GetAttackPoint(aiController.currentLine, aiController.teamIdentity.teamType);
                aiController.battleTarget = null;
                aiController.SetMissionTarget(attackTarget);
                aiController.SetState(States.MissionExecution);

                Debug.Log($"{aiController.aiType.text} : Attack Command Execute");
            }
        }       // 개별명령
        else
        {
            ai.isAttack = true;
            ai.isDefend = false;
            ai.SetState(States.MissionExecution);

            Transform attackTarget = ai.buildingManager.GetAttackPoint(ai.currentLine, ai.teamIdentity.teamType);
            ai.battleTarget = null;
            ai.SetMissionTarget(attackTarget);
            ai.SetState(States.MissionExecution);

            if (ai.teamIdentity.teamType == TeamType.PC)
                gameManager.commandManager.SetActiveCommandButton(ai);
            Debug.Log($"{ai.aiType.text} : Attack Command Execute");
        }
    }
}
