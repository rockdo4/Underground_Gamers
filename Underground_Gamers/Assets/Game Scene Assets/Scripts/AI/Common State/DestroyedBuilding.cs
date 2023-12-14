using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedBuilding : MonoBehaviour, IDestroyable
{
    private BuildingManager buildingManager;
    private void Awake()
    {
        buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
    }
    public void DestoryObject(GameObject attacker)
    {
        Building building = GetComponent<Building>();

        // publish
        // 빌딩List 갱신
        BuildingEventBus.Publish(transform);
        // 해당 라인 AI 갱신
        building.PublishMissionTargetEvent();
        // 빌딩 EventBus 해제
        buildingManager.UnsubscribeDestroyEvent(transform);
    }
}
