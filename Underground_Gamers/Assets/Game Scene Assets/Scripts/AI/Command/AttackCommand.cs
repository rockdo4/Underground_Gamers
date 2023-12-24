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
                aiController.isMission = false;
                aiController.isDefend = false;
                aiController.isRetreat = false;

                Transform[] wayPoints = aiController.currentLine switch
                {
                    Line.Bottom => wayPoint.bottomWayPoints,
                    Line.Top => wayPoint.topWayPoints,
                    _ => wayPoint.bottomWayPoints
                };

                Transform lineWayPoint = Utils.FindNearestPoint(aiController, wayPoints);
                if (lineWayPoint != null)
                {
                    // 여기서 타겟만 잡아준다, 죽은 이후 명령 수행하기 위함
                    aiController.missionTarget = lineWayPoint;
                    //ai.SetMissionTarget(lineWayPoint);
                }

                aiController.battleTarget = null;
                aiController.SetState(States.MissionExecution);

                Debug.Log($"{aiController.aiType.text} : Attack Command Execute");
            }
        }       // 개별명령
        else
        {
            ai.isAttack = true;
            ai.isMission = false;
            ai.isDefend = false;
            ai.isRetreat = false;
            ai.SetState(States.MissionExecution);

            Transform[] wayPoints = ai.currentLine switch
            {
                Line.Bottom => wayPoint.bottomWayPoints,
                Line.Top => wayPoint.topWayPoints,
                _ => wayPoint.bottomWayPoints
            };

            Transform lineWayPoint = Utils.FindNearestPoint(ai, wayPoints);
            if (lineWayPoint != null)
            {
                // 여기서 타겟만 잡아준다, 죽은 이후 명령 수행하기 위함
                ai.missionTarget = lineWayPoint;
                //ai.SetMissionTarget(lineWayPoint);
            }

            //Transform attackTarget = ai.buildingManager.GetAttackPoint(ai.currentLine, ai.teamIdentity.teamType);
            //ai.SetMissionTarget(attackTarget);
            ai.battleTarget = null;
            ai.SetState(States.MissionExecution);

            if (ai.teamIdentity.teamType == TeamType.PC)
                gameManager.commandManager.SetActiveCommandButton(ai);
            Debug.Log($"{ai.aiType.text} : Attack Command Execute");
        }
    }
}
