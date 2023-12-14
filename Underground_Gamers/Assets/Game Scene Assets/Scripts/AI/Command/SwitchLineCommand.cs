using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLineCommand : Command
{
    public override void ExecuteCommand(AIController ai, WayPoint wayPoint)
    {
        //if (!ai.status.IsLive)
        //{
        //    return;
        //}
        int line = (int)ai.currentLine;
        line++;
        ai.currentLine = (Line)(line % (int)Line.Count);

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
    }
}