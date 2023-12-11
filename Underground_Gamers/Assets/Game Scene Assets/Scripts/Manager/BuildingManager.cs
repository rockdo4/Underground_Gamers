using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Transform> pcBottomLineBuildings;
    public List<Transform> pcTopLineBuildings;
    public List<Transform> npcBottomLineBuildings;
    public List<Transform> npcTopLineBuildings;

    private void Awake()
    {
        SubScribeDestroyEvent(pcBottomLineBuildings);
        SubScribeDestroyEvent(pcTopLineBuildings);
        SubScribeDestroyEvent(npcBottomLineBuildings);
        SubScribeDestroyEvent(npcTopLineBuildings);
    }

    public void SubScribeDestroyEvent(List<Transform> buildings)
    {
        for(int i =0; i < buildings.Count - 1; ++i)
        {
            TeamIdentifier identity = buildings[i].GetComponent<TeamIdentifier>();
            BuildingEventBus.Subscribe(buildings[i], () => ReleasePoint(identity.line, identity.teamType));
        }
    }

    public void UnsubscribeDestroyEvent(Transform building)
    {
        TeamIdentifier identity = building.GetComponent<TeamIdentifier>();
        BuildingEventBus.Unsubscribe(building, () => ReleasePoint(identity.line, identity.teamType));
    }

    // 적군 포인트 반환
    public Transform GetPoint(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch (type)
        {
            case TeamType.PC:
                List<Transform> pcPoints = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
                };
                points = pcPoints;
                break;

            case TeamType.NPC:
                List<Transform> npcPoints = line switch
                {
                    Line.Bottom => pcBottomLineBuildings,
                    Line.Top => pcTopLineBuildings,
                    _ => pcBottomLineBuildings
                };
                points = npcPoints;
                break;
        }

        return points[0];
    }

    // 적군 포인트 반환
    public List<Transform> GetPoints(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch (type)
        {
            case TeamType.PC:
                List<Transform> npcPoints = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
                };
                points = npcPoints;
                break;

            case TeamType.NPC:
                List<Transform> pcPoints = line switch
                {
                    Line.Bottom => pcBottomLineBuildings,
                    Line.Top => pcTopLineBuildings,
                    _ => pcBottomLineBuildings
                };
                points = pcPoints;
                break;
        }

        return points;
    }

    // 아군 타워 파괴시
    public void ReleasePoint(Line line, TeamType type)
    {
        List<Transform> lineBuilding = null;
        switch (type)
        {
            case TeamType.PC:
                lineBuilding = line switch
                { Line.Bottom => pcBottomLineBuildings,
                Line.Top => pcTopLineBuildings,
                _ => pcBottomLineBuildings
                };
                break;

            case TeamType.NPC:
                lineBuilding = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
                };
                break;
        }

        lineBuilding.Remove(lineBuilding[0]);
    }
}
