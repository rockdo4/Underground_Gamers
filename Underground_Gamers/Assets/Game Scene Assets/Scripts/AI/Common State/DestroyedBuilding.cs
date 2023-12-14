using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedBuilding : MonoBehaviour, IDestroyable
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public void DestoryObject(GameObject attacker)
    {
        Building building = GetComponent<Building>();
        TeamIdentifier identity = GetComponent<TeamIdentifier>();

        // publish
        // 빌딩List 갱신
        BuildingEventBus.Publish(transform);
        // 해당 라인 AI 갱신
        building.PublishMissionTargetEvent();
        // 빌딩 EventBus 해제
        gameManager.buildingManager.UnsubscribeDestroyEvent(transform);
        gameManager.gameRuleManager.ReleaseBuilding(identity);
    }
}
