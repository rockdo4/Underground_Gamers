using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Transform> pcBottomLineBuildings;
    public List<Transform> pcTopLineBuildings;
    public List<Transform> npcBottomLineBuildings;
    public List<Transform> npcTopLineBuildings;

    public Transform GetPoint(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch(type)
        {
            case TeamType.PC:
                List<Transform> pcPoints = line switch
                {
                    Line.Bottom => pcBottomLineBuildings,
                    Line.Top => pcTopLineBuildings,
                    _ => pcBottomLineBuildings
                };

                points = pcPoints;
                break;
            case TeamType.NPC:
                List<Transform> npcPoints = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
                };
                points = npcPoints;
                break;
        }

        return points[0];
    }
    public List<Transform> GetPoints(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch(type)
        {
            case TeamType.PC:
                List<Transform> pcPoints = line switch
                {
                    Line.Bottom => pcBottomLineBuildings,
                    Line.Top => pcTopLineBuildings,
                    _ => pcBottomLineBuildings
                };

                points = pcPoints;
                break;
            case TeamType.NPC:
                List<Transform> npcPoints = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
                };
                points = npcPoints;
                break;
        }

        return points;
    }
}
