using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    PC,
    NPC
}

public class TeamIdentifier : MonoBehaviour
{
    public LayerMask teamLayer;
    public LayerMask enemyLayer;

    public bool isBuilding;
    public Transform buildingTarget;
    [Header("건축물 설정 사항")]
    public TeamType teamType;
    public Line line;

    private float targetReleaseTime = 3f;
    private float lastTargetReleaseTime;

    private void Update()
    {
        // 일정시간 동안 타겟이 갱신안되면 해제 / 빌딩이 공격당하지 않으면
        if(isBuilding && buildingTarget != null && targetReleaseTime + lastTargetReleaseTime < Time.time)
        {
            lastTargetReleaseTime = Time.time;
            buildingTarget = null;
        }
    }

    // 건물을 때리는 타겟 설정, 타겟 죽으면 해제
    public void SetBuildingTarget(Transform attacker)
    {
        lastTargetReleaseTime = Time.time;
        Transform prevTarget = this.buildingTarget;
        this.buildingTarget = attacker;
        CharacterStatus status = attacker.GetComponent<CharacterStatus>();

        if(prevTarget != null )
        {
            CharacterStatus prevStatus = prevTarget.GetComponent<CharacterStatus>();
            BattleTargetEventBus.Unsubscribe(prevStatus, ReleaseTarget);
        }

        if (status != null)
        {
            BattleTargetEventBus.Subscribe(status, ReleaseTarget);
        }
    }

    private void ReleaseTarget()
    {
        lastTargetReleaseTime = Time.time;
        buildingTarget = null;
    }
}
