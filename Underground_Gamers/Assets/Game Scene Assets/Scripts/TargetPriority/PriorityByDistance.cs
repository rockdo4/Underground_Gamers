using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriorityByDistance.Asset", menuName = "TargetPriority/PriorityByDistance")]
public class PriorityByDistance : TargetPriority
{
    public bool IsCloser;
    public override bool SetTargetByPriority(AIController ai, CharacterStatus target)
    {
        float targetDistance = Vector3.Distance(ai.transform.position, target.transform.position);

        if (IsCloser)
        {
            if (ai.DistanceToBattleTarget > targetDistance)
            {
                return true;
            }
        }
        else
        {
            if (ai.DistanceToBattleTarget < targetDistance)
            {
                return true;
            }
        }

        return false;
    }

}
