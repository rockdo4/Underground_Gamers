using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriorityByOccupation.Asset", menuName = "TargetPriority/PriorityByOccupation")]
public class PriorityByOccupation : TargetPriority
{
    public OccupationType compareType;
    public override bool SetTargetByPriority(AIController ai, CharacterStatus target)
    {
        if (target != null)
        {
            if (target.occupationType == compareType)
            {
                return true;
            }
        }

        return false;
    }
}
