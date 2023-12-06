using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriorityByOccupation.Asset", menuName = "TargetPriority/PriorityByOccupation")]
public class PriorityByOccupation : TargetPriority
{
    public OccupationType compareType;
    public override bool SetTargetByPriority(AIController ai, AIController target)
    {
        float targetDistance = Vector3.Distance(ai.transform.position, target.transform.position);
        if (target.occupationType == compareType && ai.DistanceToTarget > targetDistance)
        {
            ai.target = target.transform;
            return true;
        }
        return false;
    }
}
