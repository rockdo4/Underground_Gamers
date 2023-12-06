using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriorityByOccupation.Asset", menuName = "TargetPriority/PriorityByOccupation")]
public class PriorityByOccupation : TargetPriority
{
    public OccupationType compareType;
    public override bool SetTargetByPriority(AIController ai, TeamIdentifier targetIdentity)
    {
        AIController target = targetIdentity.GetComponent<AIController>();
        if (target != null)
        {
            if (target.occupationType == compareType)
            {
                ai.target = target.transform;
                return true;
            }
        }

        return false;
    }
}
