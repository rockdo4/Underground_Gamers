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
            foreach (var aiController in gameManager.aiManager.pc)
            {
                aiController.isDefend = true;
                aiController.isRetreat = false;
                aiController.isAttack = false;
                aiController.isMission = false;

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
                if (aiController.status.IsLive && !aiController.isReloading && !aiController.isStun 
                    && aiController.moveCoroutine == null && aiController.useMoveCoroutine == null)
                    aiController.SetState(States.Retreat);
            }
        }         // 개별명령
        else
        {
            ai.isDefend = true;
            ai.isRetreat = false;
            ai.isAttack = false;
            ai.isMission = false;

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

            if (ai.status.IsLive && !ai.isReloading && !ai.isStun
                && ai.moveCoroutine == null && ai.useMoveCoroutine == null)
                ai.SetState(States.Retreat);
            if (ai.teamIdentity.teamType == TeamType.PC)
                gameManager.commandManager.SetActiveCommandButton(ai);
        }
    }
}
