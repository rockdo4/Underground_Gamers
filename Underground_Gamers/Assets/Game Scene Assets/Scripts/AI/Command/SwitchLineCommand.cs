using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLineCommand : Command
{
    public override void ExecuteCommand(AIController ai, WayPoint wayPoint)
    {
        Debug.Log($"{ai.aiType.text} : SwitchLine Command Execute");

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
        if(lineWayPoint != null )
        {
            ai.SetTarget(lineWayPoint);
        }
    }
}