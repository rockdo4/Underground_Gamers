using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamIdentifier : MonoBehaviour
{
    public LayerMask teamLayer;
    public LayerMask enemyLayer;

    public bool isBuilding;
    public Transform buildingTarget;

    private float targetReleaseTime = 3f;
    private float lastTargetReleaseTime;

    private void Update()
    {
        if(isBuilding && buildingTarget != null && targetReleaseTime + lastTargetReleaseTime < Time.time)
        {
            lastTargetReleaseTime = Time.time;
            buildingTarget = null;
        }
    }

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
