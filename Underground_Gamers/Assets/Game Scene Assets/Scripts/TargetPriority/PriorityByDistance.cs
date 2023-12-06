using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriorityByDistance.Asset", menuName = "TargetPriority/PriorityByDistance")]
public class PriorityByDistance : TargetPriority
{
    public bool IsCloser;
    public override bool SetTargetByPriority(AIController ai, TeamIdentifier targetIdentity)
    {
        float targetDistance = Vector3.Distance(ai.transform.position, targetIdentity.transform.position);

        if (IsCloser)
        {
            if (ai.DistanceToTarget > targetDistance)
            {
                return true;
            }
        }
        else
        {
            if (ai.DistanceToTarget < targetDistance)
            {
                return true;
            }
        }

        return false;
    }

}
