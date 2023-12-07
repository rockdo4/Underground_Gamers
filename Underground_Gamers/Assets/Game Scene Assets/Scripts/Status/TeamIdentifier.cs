using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamIdentifier : MonoBehaviour
{
    public LayerMask teamLayer;
    public LayerMask enemyLayer;

    public bool isBuilding;
    public Transform target;

    public void SetBuildingTarget(Transform target)
    {
        Transform prevTarget = this.target;
        this.target = target;
        CharacterStatus status = target.GetComponent<CharacterStatus>();

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
        target = null;
    }
}
