using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Transform> pcBottomLineBuildings;
    public List<Transform> pcTopLineBuildings;
    public List<Transform> npcBottomLineBuildings;
    public List<Transform> npcTopLineBuildings;

    public List<Transform> originPCBottomLineBuildings;
    public List<Transform> originPCTopLineBuildings;
    public List<Transform> originNPCBottomLineBuildings;
    public List<Transform> originNPCTopLineBuildings;

    public List<Transform> allBuildings;

    private void Awake()
    {
        AllSubscribeDestroyEvent();
        DisplayBuildingHPByReset();
    }

    public void ResetBuildings()
    {
        // SetActive상태의 건물 넣어주기
        ResetLineBuildings();
        ResetBuildingsStatus();
        AllSubscribeDestroyEvent();
        DisplayBuildingHPByReset();
    }

    public void ResetLineBuildings()
    {
        pcBottomLineBuildings.Clear();
        pcTopLineBuildings.Clear();
        npcBottomLineBuildings.Clear();
        npcTopLineBuildings.Clear();

        foreach (Transform building in originPCBottomLineBuildings)
        {
            building.gameObject.SetActive(true);
            pcBottomLineBuildings.Add(building);
        }
        foreach (Transform building in originPCTopLineBuildings)
        {
            building.gameObject.SetActive(true);
            pcTopLineBuildings.Add(building);
        }
        foreach (Transform building in originNPCBottomLineBuildings)
        {
            building.gameObject.SetActive(true);
            npcBottomLineBuildings.Add(building);
        }
        foreach (Transform building in originNPCTopLineBuildings)
        {
            building.gameObject.SetActive(true);
            npcTopLineBuildings.Add(building);
        }
        //pcBottomLineBuildings = originPCBottomLineBuildings;
        //pcTopLineBuildings = originPCTopLineBuildings;
        //npcBottomLineBuildings = originNPCBottomLineBuildings;
        //npcTopLineBuildings = originNPCTopLineBuildings;

    }

    public void DisplayBuildingHPByReset()
    {
        DisplayBuildingHP(pcBottomLineBuildings[0].GetComponent<Building>(), true);
        DisplayBuildingHP(pcTopLineBuildings[0].GetComponent<Building>(), true);
        DisplayBuildingHP(npcBottomLineBuildings[0].GetComponent<Building>(), true);
        DisplayBuildingHP(npcTopLineBuildings[0].GetComponent<Building>(), true);
        DisplayBuildingHP(pcTopLineBuildings[1].GetComponent<Building>(), false);
        DisplayBuildingHP(npcTopLineBuildings[1].GetComponent<Building>(), false);
    }

    public void AllSubscribeDestroyEvent()
    {
        SubscribeDestroyEvent(pcBottomLineBuildings);
        SubscribeDestroyEvent(pcTopLineBuildings);
        SubscribeDestroyEvent(npcBottomLineBuildings);
        SubscribeDestroyEvent(npcTopLineBuildings);
    }

    public void ResetBuildingsStatus()
    {
        foreach (var building in allBuildings)
        {
            building.gameObject.SetActive(true);
            CharacterStatus buildingStatus = building.GetComponent<CharacterStatus>();
            buildingStatus.ResetStatus();
        }
    }

    public void SubscribeDestroyEvent(List<Transform> buildings)
    {
        for (int i = 0; i < buildings.Count - 1; ++i)
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
    public Transform GetAttackPoint(Line line, TeamType type)
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

    // 적군 포인트s 반환
    public List<Transform> GetAttackPoints(Line line, TeamType type)
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

    // 아군 포인트 반환
    public Transform GetDefendPoint(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch (type)
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

    // 아군 포인트s 반환
    public List<Transform> GetDefendPoints(Line line, TeamType type)
    {
        List<Transform> points = null;
        switch (type)
        {
            case TeamType.PC:
                List<Transform> npcPoints = line switch
                {
                    Line.Bottom => pcBottomLineBuildings,
                    Line.Top => pcTopLineBuildings,
                    _ => pcBottomLineBuildings
                };
                points = npcPoints;
                break;

            case TeamType.NPC:
                List<Transform> pcPoints = line switch
                {
                    Line.Bottom => npcBottomLineBuildings,
                    Line.Top => npcTopLineBuildings,
                    _ => npcBottomLineBuildings
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
                {
                    Line.Bottom => pcBottomLineBuildings,
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
        if (lineBuilding[0] != null)
            DisplayBuildingHP(lineBuilding[0].GetComponent<Building>(), true);
    }

    public void DisplayBuildingHP(Building building, bool isActive)
    {
        building.hpBar.SetActive(isActive);
        building.GetComponent<CharacterStatus>().GetHp();
    }
}
